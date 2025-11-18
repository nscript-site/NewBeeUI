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
