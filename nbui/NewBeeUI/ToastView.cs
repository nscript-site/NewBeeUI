using Avalonia.Threading;

namespace NewBeeUI;

public class ToastView : BaseView
{
    TextBlock? toast;
    Border? border;
    PathIcon? icon;

    bool CompactMode;

    protected override object Build()
    {
        return new Border().Align(0, 0).CornerRadius(4)
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

    public void ShowToast(string message, double seconds = 2, double opacity = 1, bool compactMode = false)
    {
        if (border == null || toast == null || String.IsNullOrEmpty(message)) return;

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
                border.IsVisible = false;
                toastTimer?.Dispose();
                toastTimer = null;
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

