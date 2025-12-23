using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace NewBeeUI;

public class SKCanvasDrawOperation : ICustomDrawOperation
{
    private readonly Rect _bounds;
    private readonly IBrush? _backgroundBrush;

    public Action<SKCanvas, SKRect>? OnDrawCanvas { get; set; }

    public SKCanvasDrawOperation(Rect bounds,IBrush? backgroundBrush)
    {
        _bounds = bounds;
        _backgroundBrush = backgroundBrush;
    }

    public void Dispose()
    {
    }

    public Rect Bounds => _bounds;

    public bool HitTest(Point p) => _bounds.Contains(p);

    public bool Equals(ICustomDrawOperation? other) => false;

    public void Render(ImmediateDrawingContext context)
    {
        if (_backgroundBrush != null)
        {
            context.FillRectangle(_backgroundBrush.ToImmutable(),
                _bounds);
        }

        var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
        if (leaseFeature is null)
        {
            return;
        }

        using var lease = leaseFeature.Lease();
        var canvas = lease?.SkCanvas;
        if (canvas is { })
        {
            Console.WriteLine(canvas.DeviceClipBounds);
            var rect = SKRect.Create((float)_bounds.Left, (float)_bounds.Top, (float)_bounds.Width, (float)_bounds.Height);

            OnDrawCanvas?.Invoke(canvas, rect);
        }
    }
}

public class SKCanvasView : Control
{
    public IBrush? BackgroundBrush { get; set; }

    public Action<SKCanvas, SKRect>? OnDrawCanvas { get; set; }

    public override void Render(DrawingContext context)
    {
        context.Custom(new SKCanvasDrawOperation(new Rect(0, 0, Bounds.Width, Bounds.Height), BackgroundBrush) { OnDrawCanvas = OnDrawCanvas });
    }
}
