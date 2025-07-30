using Avalonia.Threading;

namespace NewBeeUI;

public class ToastView : BaseView
{
    TextBlock? toast;
    Border? border;
    PathIcon? icon;

    bool CompactMode;

    /// <summary>
    /// 是否是临时创建的。如果是，则在关闭时，将自动从父容器中移除。
    /// </summary>
    public bool IsTemporary { get; set; }

    protected override object Build()
    {
        return new Border().Align(0, 0).CornerRadius(4)
                    .Margin(80)
                    .MaxWidth(600)
                    .BorderBrush(R("SukiMenuBorderBrush"))
                    .BorderThickness(1)
                    .Background(R("SukiCardBackground"))
                    .Ref(out border)!
                    .IsVisible(false)
                    .Child(
                        Grid(cols: "Auto, *").Margin(10).Children([
                            PathIcon(NStyles.MeterialIcons.InformationOutlineIcon.Instance)
                                .Ref(out icon)!.Margin(0,0,10,0)
                                .IsVisible(CompactMode == false)
                                .Foreground(R("SukiPrimaryColor")),
                            new TextBlock().Col(1).Align(1,0)
                                .TextWrapping(TextWrapping.Wrap).Foreground(R("SukiText"))
                                .Ref(out toast)!
                        ])
                    );
    }

    System.Timers.Timer? toastTimer;

    public void ShowToast(string message, double seconds = 2, double opacity = 1, bool compactMode = false, int hAlign = 0, int vAlign = -1)
    {
        if (border == null || toast == null || String.IsNullOrEmpty(message)) return;

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
        toast.Text = message;
        toastTimer = new System.Timers.Timer(miniseconds);
        toastTimer.Elapsed += (s, e) =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                toastTimer?.Dispose();
                toastTimer = null;
                if(IsTemporary)
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

    public void Hide()
    {
        if (border == null) return;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            border.IsVisible = false;
        });
    }
}

