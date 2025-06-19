using Avalonia.Controls.ApplicationLifetimes;
using NStyles.Controls;

namespace NewBeeUI;

public class WindowInfo
{
    public int WindowMinWidth { get; set; } = 400;
    public int WindowMinHeight { get; set; } = 300;
    public int WindowWidth { get; set; } = 400;
    public int WindowHeight { get; set; } = 300;

    public string? WindowTitle { get; set; } = String.Empty;

    public bool CanResize { get; set; } = true;
    public bool CanMinimize { get; set; } = true;
    public bool CanClose { get; set; } = true;

    public Stream? WindowsIcon { get; set; } = null;
    public Control? LogoContent { get; set; } = null;

    public bool IsWindowAnimationEnable { get; set; } = false;

    public Window? Window { get; protected set; } = null;

    public double? CornerRadius { get; set; } = null;

    public WindowStartupLocation StartupLocation { get; set; } = WindowStartupLocation.CenterOwner;

    public PixelPoint? Position { get; set; }

    public Action<BaseView>? OnCreate { get; set; }

    internal void CloseWindow()
    {
        Window?.Close();
        Window = null;
    }

    internal async Task ShowDialogAsync(BaseView content, BaseView? owner = null, string? newTitle = null)
    {
        Window? desktopWindow = owner?.GetDesktopWindow() ?? content.GetDesktopMainWindow();

        var newWin = CreateNWindow(content, newTitle);
        Window = newWin;

        if (desktopWindow == null)
        {
            var lifetime = Application.Current?.ApplicationLifetime;

            // 判断并转换为 IClassicDesktopStyleApplicationLifetime
            if (lifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = newWin;
            }
        }
        else
        {
            await newWin.ShowDialog(desktopWindow);
        }
    }

    protected virtual NWindow CreateNWindow(BaseView content, string? newTitle = null)
    {
        var newWin = new NWindow()
        {
            Content = content,
            Title = newTitle ?? WindowTitle ?? String.Empty,
            Width = WindowWidth,
            Height = WindowHeight,
            MinWidth = WindowMinWidth,
            MinHeight = WindowMinHeight,
            WindowStartupLocation = StartupLocation,
            BackgroundAnimationEnabled = IsWindowAnimationEnable,
            TitleBarAnimationEnabled = IsWindowAnimationEnable,
        };

        if(CornerRadius.HasValue)
        {
            newWin.CornerRadius = new CornerRadius(CornerRadius.Value);
        }

        if (Position != null)
        {
            newWin.Position = Position.Value;
        }

        newWin.CanMaximize = CanResize;
        newWin.CanResize = CanResize;
        newWin.CanMinimize = CanMinimize;

        if (WindowsIcon != null) newWin.Icon = new WindowIcon(WindowsIcon);
        if (LogoContent != null) newWin.LogoContent = LogoContent;

        return newWin;
    }
}

public class NWindowInfo : WindowInfo
{
    public Control? Subtitle { get; set; }

    public Control? RightWindowsBar { get; set; } = null;

    protected override NWindow CreateNWindow(BaseView content, string? newTitle = null)
    {
        var w = base.CreateNWindow(content, newTitle);
        w.SubTitleContent = Subtitle;
        if(RightWindowsBar != null)
            w.RightWindowTitleBarControls = RightWindowsBar;
        return w;
    }
}