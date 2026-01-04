using Avalonia.LogicalTree;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using NewBeeUI.Platforms;

namespace NewBeeUI.Demo.Views;

public class CameraInfoView : BaseView
{
    private int ImageWidth = 0;
    private int ImageHeight = 0;
    private int FrameIndex = 0;

    private Image image = default!;

    protected override object Build()
    {
        if(App.CameraService is null)
        {
            return TextBlock("Camera service is not available.");
        }

        App.CameraService.FrameArrived -= CameraService_FrameArrived;
        App.CameraService.FrameArrived += CameraService_FrameArrived;

        App.CameraService.StartPreview();

        return VStack([
            TextBlock("Camera View Loading"),
            TextBlock(()=> $"Position:{App.CameraService.Position.ToString()}, Frame: {FrameIndex}, Image Size: {ImageWidth}x{ImageHeight}"),
            new Image().Size(400,400).Ref(out image),
            ]);
    }

    private void CameraService_FrameArrived(Platforms.PooledPixelFrame frame)
    {
        ImageWidth = frame.Width;
        ImageHeight = frame.Height;
        FrameIndex++;
        
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            // 这里没清理上一个 Bitmap，实际应用中应该注意内存管理
            var bitmap = ConvertToBitmap(frame);
            image.Source = bitmap;
            this.UpdateState();
        });
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        App.CameraService?.StopPreview();
    }

    public unsafe static Bitmap ConvertToBitmap(PooledPixelFrame frame)
    {
        // Avalonia 需要 PixelFormat.Bgra8888
        var pixelFormat = Avalonia.Platform.PixelFormat.Bgra8888;
        var alphaFormat = Avalonia.Platform.AlphaFormat.Unpremul;

        fixed (byte* pData = frame.Data)
        {
            // 创建 Bitmap
            return new Bitmap(
                pixelFormat,
                alphaFormat,
                (nint)pData,
                new PixelSize(frame.Width, frame.Height),
                new Vector(96, 96),
                frame.Stride
            );
        }
    }
}
