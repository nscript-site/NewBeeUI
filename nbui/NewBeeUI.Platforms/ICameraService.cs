using System.Buffers;

namespace NewBeeUI.Platforms;

public enum CameraPosition
{
    Unspecified,
    Front,
    Back
}

/// <summary>
/// Camera service that delivers raw BGRA frames.
/// Consumer MUST call Release() on the PooledPixelFrame when done.
/// </summary>
public interface ICameraService
{
    CameraPosition Position { get; }
    void StartPreview();
    void StopPreview();

    void SwitchCamera();

    /// <summary>
    /// Raised on background thread when a new frame is available.
    /// Frame.Data is a rented array from ArrayPool{byte}.
    /// Consumer must call frame.Release() after copying/consuming it.
    /// </summary>
    event Action<PooledPixelFrame>? FrameArrived;
}

public sealed class PooledPixelFrame
{
    public byte[] Data { get; }
    public int Width { get; }
    public int Height { get; }
    public int Stride { get; }

    readonly Action<byte[]> _releaseAction;

    public PooledPixelFrame(byte[] data, int width, int height, int stride, Action<byte[]> releaseAction)
    {
        Data = data;
        Width = width;
        Height = height;
        Stride = stride;
        _releaseAction = releaseAction;
    }

    public void Release() => _releaseAction?.Invoke(Data);
}

public class MockCameraService : ICameraService
{
    public CameraPosition Position => CameraPosition.Back;
    public event Action<PooledPixelFrame>? FrameArrived;

    private bool _isPreviewing = false;

    public void StartPreview()
    {
        // Simulate frame arrival
        Task.Run(async () =>
        {
            Random random = new Random();
            _isPreviewing = true;
            var b = (byte)random.Next(0, 256);
            var g = (byte)random.Next(0, 256);
            var r = (byte)random.Next(0, 256);

            while (_isPreviewing)
            {
                await Task.Delay(100); // Simulate frame rate

                b = (byte)((b + 1) % 255);
                g = (byte)((g + 2) % 255);
                r = (byte)((r + 3) % 255);

                // Create a dummy frame (e.g., 1080x1920 BGRA)
                var width = 1080;
                var height = 1920;
                var stride = width * 4; // BGRA
                var data = ArrayPool<byte>.Shared.Rent(stride * height);
                // Fill with dummy data (e.g., solid color)
                for (int i = 0; i < data.Length; i += 4)
                {
                    data[i] = b;     // B
                    data[i + 1] = g;   // G
                    data[i + 2] = r;   // R
                    data[i + 3] = 255; // A
                }
                var frame = new PooledPixelFrame(data, width, height, stride, arr => ArrayPool<byte>.Shared.Return(arr));
                FrameArrived?.Invoke(frame);
            }
        });
    }
    public void StopPreview()
    {
        _isPreviewing = false;
    }

    public void SwitchCamera()
    {
        // No-op for mock
    }
}
