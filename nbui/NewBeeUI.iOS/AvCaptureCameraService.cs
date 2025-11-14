using AVFoundation;
using CoreFoundation;
using CoreMedia;
using CoreVideo;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NewBeeUI.Platforms;

namespace NewBeeUI.iOS;

public class AvCaptureCameraService : NSObject, ICameraService, IAVCaptureVideoDataOutputSampleBufferDelegate
{
    AVCaptureSession? _session;
    AVCaptureDeviceInput? _input;
    AVCaptureVideoDataOutput? _videoOutput;

    // Throttle frames to reduce CPU (optional): deliver every Nth frame
    readonly int _frameSkip = 0; // 0 = deliver all frames
    int _frameCounter = 0;

    public event Action<PooledPixelFrame>? FrameArrived;

    public void StartPreview()
    {
        if (_session != null && _session.Running) return;

        var device = AVCaptureDevice.GetDefaultDevice(AVMediaTypes.Video);
        if (device == null) return;

        NSError? err;
        var input = AVCaptureDeviceInput.FromDevice(device, out err);
        if (err != null || input == null) return;

        var session = new AVCaptureSession { SessionPreset = AVCaptureSession.PresetHigh };

        if (session.CanAddInput(input))
            session.AddInput(input);

        var output = new AVCaptureVideoDataOutput();

        // Request BGRA pixel format
        output.UncompressedVideoSetting = new AVVideoSettingsUncompressed
        {
            PixelFormatType = CVPixelFormatType.CV32BGRA
        };

        // Deliver on background queue
        var queue = new DispatchQueue("videoQueue");
        output.SetSampleBufferDelegate(this, queue);

        if (session.CanAddOutput(output))
            session.AddOutput(output);

        // Prefer the camera orientation / mirroring settings if necessary
        _session = session;
        _input = input;
        _videoOutput = output;

        session.StartRunning();
    }

    public void StopPreview()
    {
        try
        {
            _session?.StopRunning();
        }
        catch { /* ignore */ }

        _videoOutput = null;
        _input = null;
        _session = null;
    }

    // Called on videoQueue
    [Export("captureOutput:didOutputSampleBuffer:fromConnection:")]
    public void DidOutputSampleBuffer(AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
    {
        try
        {
            if (_frameSkip > 0)
            {
                _frameCounter++;
                if (_frameCounter % (_frameSkip + 1) != 0)
                {
                    sampleBuffer.Dispose();
                    return;
                }
            }

            using (var imageBuffer = sampleBuffer.GetImageBuffer() as CVPixelBuffer)
            {
                if (imageBuffer == null) return;

                // Lock the base address for reading
                imageBuffer.Lock(CVPixelBufferLock.None);

                try
                {
                    var width = (int)imageBuffer.Width;
                    var height = (int)imageBuffer.Height;
                    var bytesPerRow = (int)imageBuffer.BytesPerRow;
                    var totalBytes = bytesPerRow * height;

                    // Rent a buffer to avoid allocations
                    var arr = ArrayPool<byte>.Shared.Rent(totalBytes);

                    var baseAddr = imageBuffer.BaseAddress;
                    if (baseAddr == IntPtr.Zero)
                    {
                        ArrayPool<byte>.Shared.Return(arr);
                        return;
                    }

                    // Copy native BGRA buffer into rented managed array
                    Marshal.Copy(baseAddr, arr, 0, totalBytes);

                    // Raise event - consumer must call Release()
                    var frame = new PooledPixelFrame(arr, width, height, bytesPerRow, OnRelease);
                    FrameArrived?.Invoke(frame);
                }
                finally
                {
                    imageBuffer.Unlock(0);
                }
            }
        }
        catch
        {
            // swallow - avoid crashing the capture queue
        }
        finally
        {
            sampleBuffer?.Dispose();
        }
    }

    void OnRelease(byte[] buffer)
    {
        // Return to pool
        ArrayPool<byte>.Shared.Return(buffer, clearArray: false);
    }
}