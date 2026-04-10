using Avalonia.Threading;

namespace NewBeeUI;

public class ToastView : BaseView
{
    TextBlock? toast;
    Border? border;
    PathIcon? icon;
    Border? toastContent;
    bool CompactMode;

    /// <summary>
    /// 是否是临时创建的。如果是，则在关闭时，将自动从父容器中移除。
    /// </summary>
    public bool IsTemporary { get; set; }

    protected override void Build(out Control content)
    {
        Border(HGrid("Auto, *", [
                    PathIcon(NStyles.MeterialIcons.InformationOutlineIcon.Instance)
                        .Ref(out icon)!.Margin(0,0,10,0)
                        .IsVisible(CompactMode == false)
                        .Foreground(R("SukiPrimaryColor")),
                    Border().Col(1).Align(1,0).Ref(out toastContent)!,
                    //new TextBlock().Col(1).Align(1,0)
                    //    .TextWrapping(TextWrapping.Wrap).Foreground(R("SukiText"))
                    //    .Ref(out toast)!
                ]).Margin(10))
            .Align(0, 0).CornerRadius(4)
            .Margin(80)
            .MaxWidth(600).IsHitTestVisible(false)
            .BorderBrush(R("SukiMenuBorderBrush"))
            .BorderThickness(1)
            .Background(R("SukiCardBackground"))
            .Ref(out border)!
            .IsVisible(false)
            .Return(out content);
    }

    System.Timers.Timer? toastTimer;

    public void ShowInUIThread(Control content, double seconds = 2, double opacity = 1, bool compactMode = false, int hAlign = 0, int vAlign = -1)
    {
        Dispatcher.UIThread.Post(async () =>
        {
            this.Show(content, seconds, opacity, compactMode, hAlign, vAlign);
        });
    }

    public void ShowInUIThread(string message, double seconds = 2, double opacity = 1, bool compactMode = false, int hAlign = 0, int vAlign = -1)
    {
        Dispatcher.UIThread.Post(async () =>
        {
            this.Show(message, seconds, opacity, compactMode, hAlign, vAlign);
        });
    }

    public void Show(string message, double seconds = 2, double opacity = 1, bool compactMode = false, int hAlign = 0, int vAlign = -1)
    {
        if (String.IsNullOrEmpty(message)) return;

        var msg = TextBlock(message).Align(1, 0)
                        .TextWrapping(TextWrapping.Wrap).Foreground(R("SukiText"));

        Show(msg, seconds, opacity, compactMode, hAlign, vAlign);
    }

    public void Show(Control content, double seconds = 2, double opacity = 1, bool compactMode = false, int hAlign = 0, int vAlign = -1)
    {
        if (border == null || toastContent == null) return;

        this.Align(hAlign, vAlign);

        if (toastTimer != null)
        {
            toastTimer.Stop();
            toastTimer.Dispose();
        }

        CompactMode = compactMode;

        var miniseconds = (int)Math.Max(100, seconds * 1000);

        border.Opacity = opacity;
        border.IsVisible = true;
        if (icon != null) icon.IsVisible = CompactMode == false;

        toastContent?.Child = content;

        toastTimer = new System.Timers.Timer(miniseconds);
        toastTimer.Elapsed += (s, e) =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                toastTimer?.Dispose();
                toastTimer = null;
                if (IsTemporary)
                {
                    var hosts = this.OverlayHosts();
                    hosts?.Remove(this);
                }
                else
                {
                    border.IsVisible = false;
                }
            });
        };
        toastTimer.Start();
    }

    internal void StopTimer()
    {
        var timer = toastTimer;
        if (timer == null) return;
        timer.Stop();
    }

    public void Hide()
    {
        if (border == null) return;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            border.IsVisible = false;
        });
    }
}

