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

    public SKCanvasDrawOperation(Rect bounds, IBrush? backgroundBrush)
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
        // 普通笔刷交给 Avalonia 绘制
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
            var rect = SKRect.Create((float)_bounds.Left, (float)_bounds.Top, (float)_bounds.Width, (float)_bounds.Height);

            OnDrawCanvas?.Invoke(canvas, rect);
        }
    }
}

public class SKCanvasView : Control
{
    private IBrush? _backgroundBrush;

    /// <summary>
    /// 背景笔刷（Avalonia 内置笔刷类型）。
    /// </summary>
    public IBrush? BackgroundBrush
    {
        get => _backgroundBrush;
        set
        {
            _backgroundBrush = value;
            InvalidateVisual();
        }
    }

    public Action<SKCanvas, SKRect>? OnDrawCanvas { get; set; }

    public override void Render(DrawingContext context)
    {
        context.Custom(new SKCanvasDrawOperation(new Rect(0, 0, Bounds.Width, Bounds.Height), BackgroundBrush)
        {
            OnDrawCanvas = OnDrawCanvas
        });
    }
}
