using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Rendering.Composition;
using NStyles.Utils.Effects;
using SkiaSharp;

namespace NStyles.Controls;

public class Loading : Control
{
    public static readonly StyledProperty<LoadingStyle> LoadingStyleProperty =
        AvaloniaProperty.Register<Loading, LoadingStyle>(nameof(LoadingStyle), defaultValue: LoadingStyle.Pellets);

    public LoadingStyle LoadingStyle
    {
        get => GetValue(LoadingStyleProperty);
        set => SetValue(LoadingStyleProperty, value);
    }

    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        AvaloniaProperty.Register<Loading, IBrush?>(nameof(Foreground));

    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    private CompositionCustomVisual? _customVisual;

    public Loading()
    {
        Width = 50;
        Height = 50;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var comp = ElementComposition.GetElementVisual(this)?.Compositor;
        if (comp == null || _customVisual?.Compositor == comp) return;
        var visualHandler = new LoadingEffectDraw() { LoadingStyle = LoadingStyle };
        _customVisual = comp.CreateCustomVisual(visualHandler);
        ElementComposition.SetElementChildVisual(this, _customVisual);
        _customVisual.SendHandlerMessage(EffectDrawBase.StartAnimations);
        if (Foreground is null)
            this[!ForegroundProperty] = new DynamicResourceExtension("SukiPrimaryColor");
        if (Foreground is ImmutableSolidColorBrush brush)
            brush.Color.ToFloatArrayNonAlloc(_color);
        _customVisual.SendHandlerMessage(_color);
        Update();
    }

    private void Update()
    {
        if (_customVisual == null) return;
        _customVisual.Size = new Vector(Bounds.Width, Bounds.Height);
    }

    private readonly float[] _color = new float[3];

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == BoundsProperty)
            Update();
        else if (change.Property == ForegroundProperty && Foreground is ImmutableSolidColorBrush brush)
        {
            brush.Color.ToFloatArrayNonAlloc(_color);
            _customVisual?.SendHandlerMessage(_color);
        }
        else if(change.Property == IsVisibleProperty)
        {
            if (IsVisible)
                _customVisual?.SendHandlerMessage(EffectDrawBase.StartAnimations);
            else
                _customVisual?.SendHandlerMessage(EffectDrawBase.StopAnimations);
        }
        else if(change.Property == LoadingStyleProperty)
        {
            _customVisual?.SendHandlerMessage(LoadingStyle);
        }
    }

    public class LoadingEffectDraw : EffectDrawBase
    {
        private float[] _color = { 1.0f, 0f, 0f };

        public LoadingStyle LoadingStyle { get; set; } = LoadingStyle.Simple;

        public LoadingEffectDraw()
        {
            AnimationSpeedScale = 2f;
        }

        protected override void Render(SKCanvas canvas, SKRect rect)
        {
            if(LoadingStyle == LoadingStyle.Pellets)
                RenderPellets(canvas, rect);
            else
                RenderSimple(canvas, rect);
        }

        // I'm not really sure how to render this properly in software fallback scenarios.
        // This is likely to cause issues with the previewer.
        // Might be worth just drawing a circle or something...
        protected override void RenderSoftware(SKCanvas canvas, SKRect rect)
        {
            Render(canvas, rect);
        }

        protected void RenderPellets(SKCanvas canvas, SKRect rect)
        {
            int dotCount = 8; // 圆点数量
            float dotRadius = 4f; // 小圆点半径
            float ringRadius = Math.Min(rect.Width, rect.Height) / 2 - dotRadius * 2;
            var center = new SKPoint(rect.MidX, rect.MidY);

            // 动画进度
            float t = (float)(AnimationSeconds * 10); // 控制速度
            int activeIndex = (int)(t % dotCount);

            for (int i = 0; i < dotCount; i++)
            {
                // 计算每个点的角度
                float angle = (float)(2 * Math.PI * i / dotCount - Math.PI / 2);
                float x = center.X + ringRadius * (float)Math.Cos(angle);
                float y = center.Y + ringRadius * (float)Math.Sin(angle);

                // 亮度渐变：当前点最亮，前后点次亮，其余更暗
                float alpha = 0.3f;
                if (i == activeIndex)
                    alpha = 1.0f;
                else if ((i + 1) % dotCount == activeIndex || (i - 1 + dotCount) % dotCount == activeIndex)
                    alpha = 0.6f;

                using var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    IsAntialias = true,
                    Color = new SKColor(
                        (byte)(_color[0] * 255),
                        (byte)(_color[1] * 255),
                        (byte)(_color[2] * 255),
                        (byte)(alpha * 255))
                };

                canvas.DrawCircle(x, y, dotRadius, paint);
            }
        }

        protected void RenderSimple(SKCanvas canvas, SKRect rect)
        {
            // 计算圆心和半径
            float strokeWidth = 4f;
            float radius = Math.Min(rect.Width, rect.Height) / 2 - strokeWidth;
            var center = new SKPoint(rect.MidX, rect.MidY);

            // 创建画笔
            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round,
                Color = new SKColor(
                    (byte)(_color[0] * 255),
                    (byte)(_color[1] * 255),
                    (byte)(_color[2] * 255))
            };

            // 计算动画进度（假设 AnimationSeconds 是 EffectDrawBase 提供的动画时间）
            float sweepAngle = 280f; // 圆环长度（度），可调整
            float startAngle = (float)(AnimationSeconds * 180) % 360; // 旋转动画

            // 画弧
            canvas.DrawArc(
                new SKRect(
                    center.X - radius,
                    center.Y - radius,
                    center.X + radius,
                    center.Y + radius),
                startAngle,
                sweepAngle,
                false,
                paint);
        }

        public override void OnMessage(object message)
        {
            base.OnMessage(message);
            if (message is float[] color)
                _color = color;
            else if (message is LoadingStyle loadingStyle)
                LoadingStyle = loadingStyle;
        }
    }
}

public enum LoadingStyle
{
    Simple,
    Pellets
}