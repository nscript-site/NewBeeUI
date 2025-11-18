using AVFoundation;
using CoreFoundation;
using CoreMedia;
using CoreVideo;
using NewBeeUI.Platforms;
using System.Buffers;
using System.Runtime.InteropServices;

namespace NewBeeUI.iOS;

public class AvCaptureCameraService : NSObject, ICameraService, IAVCaptureVideoDataOutputSampleBufferDelegate
{
    AVCaptureSession? _session;
    AVCaptureDeviceInput? _input;
    AVCaptureVideoDataOutput? _videoOutput;

    // Throttle frames to reduce CPU (optional): deliver every Nth frame
    readonly int _frameSkip = 0; // 0 = deliver all frames
    int _frameCounter = 0;

    CameraPosition _currentPosition = CameraPosition.Back;

    private static NSString Video = new NSString("vide");

    public CameraPosition Position { get => _currentPosition; }

    public event Action<PooledPixelFrame>? FrameArrived;

    private AVCaptureDevicePosition ToAVCaptureDevicePosition(CameraPosition position)
    {
        return position == CameraPosition.Front ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
    }

    private AVCaptureDevice? GetCameraDevice(CameraPosition position)
    {
        // 获取所有视频类型的摄像头
        var discoverySession = AVCaptureDeviceDiscoverySession.Create([AVCaptureDeviceType.BuiltInUltraWideCamera], AVMediaTypes.Video, ToAVCaptureDevicePosition(position));

        // 没找到则返回第一个可用摄像头
        return discoverySession.Devices.FirstOrDefault();
    }

    public void StartPreview()
    {
        if (_session != null && _session.Running) return;
        
        var device = GetCameraDevice(_currentPosition);
        if (device == null) return;

        NSError? err;
        var input = AVCaptureDeviceInput.FromDevice(device, out err);
        if (err != null || input == null) return;

        var session = new AVCaptureSession { SessionPreset = AVCaptureSession.PresetHigh };
        session.BeginConfiguration();

        try
        {
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
            if (_videoOutput != null)
            {
                var connection = _videoOutput.ConnectionFromMediaType(Video);
                if (connection != null && connection.IsVideoRotationAngleSupported(new NFloat(90)))
                {
                    connection.VideoRotationAngle = 90;
                }
            }
        }
        finally
        {
            session.CommitConfiguration();
        }

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

    public void SwitchCamera()
    {
        // 切换目标摄像头
        _currentPosition = _currentPosition == CameraPosition.Back ? CameraPosition.Front : CameraPosition.Back;

        // 停止当前预览
        StopPreview();

        // 重新启动预览
        StartPreview();
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
                    try
                    {
                        FrameArrived?.Invoke(frame);
                    }
                    finally
                    {
                        frame.Release();
                    }
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