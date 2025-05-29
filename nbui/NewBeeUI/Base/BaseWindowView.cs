using NStyles.Controls;

namespace NewBeeUI;

/// <summary>
/// 基本的窗体类别
/// </summary>
public class BaseWindowView : BaseView
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

    public bool DialogResult { get; protected set; } = false;

    private Window? Window { get; set; } = null;

    protected override object Build()
    {
        throw new NotImplementedException();
    }

    public void CloseWindow()
    {
        Window?.Close();
    }

    public async Task ShowDialog(MvuView owner)
    {
        var win = TopLevel.GetTopLevel(owner) as Window;

        if (win == null) throw new NotImplementedException("TopLevel is not a Window");

        var newWin = new NWindow()
        {
            Content = this,
            Title = WindowTitle ?? String.Empty,
            Width = WindowWidth,
            Height = WindowHeight,
            MinWidth = WindowMinWidth,
            MinHeight = WindowMinHeight,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };

        newWin.CanMaximize = CanResize;
        newWin.CanResize = CanResize;
        newWin.CanMinimize = CanMinimize;

        if(WindowsIcon != null) newWin.Icon = new WindowIcon(WindowsIcon);
        if (LogoContent != null) newWin.LogoContent = LogoContent;

        Window = newWin;

        await newWin.ShowDialog(win);

        Window = null;
    }
}
