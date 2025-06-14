using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using System;
using Avalonia.Collections;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia;

namespace NStyles.Controls;

public class NWindow : Window
{
    protected override Type StyleKeyOverride => typeof(NWindow);

    public static readonly StyledProperty<double> TitleFontSizeProperty =
        AvaloniaProperty.Register<NWindow, double>(nameof(TitleFontSize), defaultValue: 13);

    public double TitleFontSize
    {
        get => GetValue(TitleFontSizeProperty);
        set => SetValue(TitleFontSizeProperty, value);
    }

    public static readonly StyledProperty<ContextMenu> TitleBarContextMenuProperty =
        AvaloniaProperty.Register<NWindow, ContextMenu>(nameof(TitleBarContextMenu), defaultValue: null);

    public ContextMenu TitleBarContextMenu
    {
        get => GetValue(TitleBarContextMenuProperty);
        set => SetValue(TitleBarContextMenuProperty, value);
    }

    public static readonly StyledProperty<FontWeight> TitleFontWeightProperty =
        AvaloniaProperty.Register<NWindow, FontWeight>(nameof(TitleFontWeight), defaultValue: FontWeight.Bold);

    public FontWeight TitleFontWeight
    {
        get => GetValue(TitleFontWeightProperty);
        set => SetValue(TitleFontWeightProperty, value);
    }

    public static readonly StyledProperty<Control?> LogoContentProperty =
        AvaloniaProperty.Register<NWindow, Control?>(nameof(LogoContent));

    public Control? LogoContent
    {
        get => GetValue(LogoContentProperty);
        set => SetValue(LogoContentProperty, value);
    }

    public static readonly StyledProperty<Control?> SubTitleContentProperty =
    AvaloniaProperty.Register<NWindow, Control?>(nameof(SubTitleContent));

    public Control? SubTitleContent
    {
        get => GetValue(SubTitleContentProperty);
        set => SetValue(SubTitleContentProperty, value);
    }

    public static readonly StyledProperty<bool> ShowBottomBorderProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(ShowBottomBorder), defaultValue: true);

    public bool ShowBottomBorder
    {
        get => GetValue(ShowBottomBorderProperty);
        set => SetValue(ShowBottomBorderProperty, value);
    }

    public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(IsTitleBarVisible), defaultValue: true);

    public bool IsTitleBarVisible
    {
        get => GetValue(IsTitleBarVisibleProperty);
        set => SetValue(IsTitleBarVisibleProperty, value);
    }

    public static readonly StyledProperty<bool> TitleBarAnimationEnabledProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(TitleBarAnimationEnabled), defaultValue: true);

    public bool TitleBarAnimationEnabled
    {
        get => GetValue(TitleBarAnimationEnabledProperty);
        set => SetValue(TitleBarAnimationEnabledProperty, value);
    }

    public static readonly StyledProperty<bool> IsMenuVisibleProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(IsMenuVisible), defaultValue: false);

    public bool IsMenuVisible
    {
        get => GetValue(IsMenuVisibleProperty);
        set => SetValue(IsMenuVisibleProperty, value);
    }

    public static readonly StyledProperty<AvaloniaList<MenuItem>?> MenuItemsProperty =
        AvaloniaProperty.Register<NWindow, AvaloniaList<MenuItem>?>(nameof(MenuItems));

    public AvaloniaList<MenuItem>? MenuItems
    {
        get => GetValue(MenuItemsProperty);
        set => SetValue(MenuItemsProperty, value);
    }

    public static readonly StyledProperty<bool> CanMinimizeProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(CanMinimize), defaultValue: true);

    public bool CanMinimize
    {
        get => GetValue(CanMinimizeProperty);
        set => SetValue(CanMinimizeProperty, value);
    }

    public static readonly StyledProperty<bool> CanCloseProperty =
    AvaloniaProperty.Register<NWindow, bool>(nameof(CanClose), defaultValue: true);

    public bool CanClose
    {
        get => GetValue(CanCloseProperty);
        set => SetValue(CanCloseProperty, value);
    }

    public static readonly StyledProperty<bool> ShowTitlebarBackgroundProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(ShowTitlebarBackground), defaultValue: true);
    public bool ShowTitlebarBackground
    {
        get => GetValue(ShowTitlebarBackgroundProperty);
        set => SetValue(ShowTitlebarBackgroundProperty, value);
    }

    public static readonly StyledProperty<bool> CanMaximizeProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(CanMaximize), defaultValue: true);
    public bool CanMaximize
    {
        get => GetValue(CanMaximizeProperty);
        set => SetValue(CanMaximizeProperty, value);
    }

    public static readonly StyledProperty<bool> CanMoveProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(CanMove), defaultValue: true);

    public bool CanMove
    {
        get => GetValue(CanMoveProperty);
        set => SetValue(CanMoveProperty, value);
    }

    // Background properties
    public static readonly StyledProperty<bool> BackgroundAnimationEnabledProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(BackgroundAnimationEnabled), defaultValue: false);

    /// <inheritdoc cref="SukiBackground.AnimationEnabled"/>
    public bool BackgroundAnimationEnabled
    {
        get => GetValue(BackgroundAnimationEnabledProperty);
        set => SetValue(BackgroundAnimationEnabledProperty, value);
    }

    //public static readonly StyledProperty<SukiBackgroundStyle> BackgroundStyleProperty =
    //    AvaloniaProperty.Register<NWindow, SukiBackgroundStyle>(nameof(BackgroundStyle),
    //        defaultValue: SukiBackgroundStyle.GradientSoft);

    ///// <inheritdoc cref="SukiBackground.Style"/>
    //public SukiBackgroundStyle BackgroundStyle
    //{
    //    get => GetValue(BackgroundStyleProperty);
    //    set => SetValue(BackgroundStyleProperty, value);
    //}

    public static readonly StyledProperty<string?> BackgroundShaderFileProperty =
        AvaloniaProperty.Register<NWindow, string?>(nameof(BackgroundShaderFile));

    /// <inheritdoc cref="SukiBackground.ShaderFile"/>
    public string? BackgroundShaderFile
    {
        get => GetValue(BackgroundShaderFileProperty);
        set => SetValue(BackgroundShaderFileProperty, value);
    }

    public static readonly StyledProperty<string?> BackgroundShaderCodeProperty =
        AvaloniaProperty.Register<NWindow, string?>(nameof(BackgroundShaderCode));

    /// <inheritdoc cref="SukiBackground.ShaderCode"/>
    public string? BackgroundShaderCode
    {
        get => GetValue(BackgroundShaderCodeProperty);
        set => SetValue(BackgroundShaderCodeProperty, value);
    }

    //public static readonly StyledProperty<bool> BackgroundTransitionsEnabledProperty =
    //    AvaloniaProperty.Register<SukiBackground, bool>(nameof(BackgroundTransitionsEnabled), defaultValue: false);

    ///// <inheritdoc cref="SukiBackground.TransitionsEnabled"/>
    //public bool BackgroundTransitionsEnabled
    //{
    //    get => GetValue(BackgroundTransitionsEnabledProperty);
    //    set => SetValue(BackgroundTransitionsEnabledProperty, value);
    //}

    //public static readonly StyledProperty<double> BackgroundTransitionTimeProperty =
    //    AvaloniaProperty.Register<SukiBackground, double>(nameof(BackgroundTransitionTime), defaultValue: 1.0);

    ///// <inheritdoc cref="SukiBackground.TransitionTime"/>
    //public double BackgroundTransitionTime
    //{
    //    get => GetValue(BackgroundTransitionTimeProperty);
    //    set => SetValue(BackgroundTransitionTimeProperty, value);
    //}

    public static readonly StyledProperty<Avalonia.Controls.Control> RightWindowTitleBarControlsProperty =
        AvaloniaProperty.Register<NWindow, Avalonia.Controls.Control>(nameof(RightWindowTitleBarControls),
            defaultValue: new Avalonia.Controls.Control());

    public static readonly StyledProperty<bool> BackgroundForceSoftwareRenderingProperty =
        AvaloniaProperty.Register<NWindow, bool>(nameof(BackgroundForceSoftwareRendering));

    /// <summary>
    /// Forces the background of the window to utilise software rendering.
    /// This prevents use of any advanced effects or animations and provides only a flat background colour that changes with the theme.
    /// </summary>
    public bool BackgroundForceSoftwareRendering
    {
        get => GetValue(BackgroundForceSoftwareRenderingProperty);
        set => SetValue(BackgroundForceSoftwareRenderingProperty, value);
    }

    /// <summary>
    /// Controls that are displayed on the right side of the title bar,
    /// to the left of the normal window control buttons. (Displays provided controls right-to-left)
    /// </summary>
    public Avalonia.Controls.Control RightWindowTitleBarControls
    {
        get => GetValue(RightWindowTitleBarControlsProperty);
        set => SetValue(RightWindowTitleBarControlsProperty, value);
    }

    public static readonly StyledProperty<Avalonia.Controls.Controls> HostsProperty =
        AvaloniaProperty.Register<NWindow, Avalonia.Controls.Controls>(nameof(Hosts),
            defaultValue: new Avalonia.Controls.Controls());

    /// <summary>
    /// These controls are displayed above all others and fill the entire window.
    /// You can include <see cref="SukiDialogHost"/> and <see cref="SukiToastHost"/> or create your own custom implementations.
    /// </summary>
    public Avalonia.Controls.Controls Hosts
    {
        get => GetValue(HostsProperty);
        set => SetValue(HostsProperty, value);
    }

    public NWindow()
    {
        MenuItems = new AvaloniaList<MenuItem>();
        RightWindowTitleBarControls = new Avalonia.Controls.Control();
        Hosts = new Avalonia.Controls.Controls();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        if (desktop.MainWindow is NWindow window && window != this)
        {
            Icon ??= window.Icon;
            // This would be nice to do, but obviously LogoContent is a control and you can't attach it twice.
            // if (LogoContent is null) LogoContent = s.LogoContent;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == WindowStateProperty && change.NewValue is WindowState windowState)
            OnWindowStateChanged(windowState);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        OnWindowStateChanged(WindowState);
        try
        {
            // Create handlers for buttons
            if (e.NameScope.Get<Button>("PART_MaximizeButton") is { } maximize)
            {
                maximize.Click += OnMaximizeButtonClicked;
                EnableWindowsSnapLayout(maximize);
            }

            if (e.NameScope.Get<Button>("PART_MinimizeButton") is { } minimize)
                minimize.Click += (_, _) => WindowState = WindowState.Minimized;

            if (e.NameScope.Get<Button>("PART_CloseButton") is { } close)
                close.Click += (_, _) => Close();

            if (e.NameScope.Get<Panel>("PART_TitleBarBackground") is { } titleBar)
            {
                titleBar.PointerPressed += OnTitleBarPointerPressed;
                titleBar.DoubleTapped += OnMaximizeButtonClicked;
            }
        }
        catch
        {
            // ignored
        }
    }

    private void OnMaximizeButtonClicked(object? sender, RoutedEventArgs args)
    {
        if (!CanMaximize) return;
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void EnableWindowsSnapLayout(Button maximize)
    {
        var pointerOnMaxButton = false;
        var setter = typeof(Button).GetProperty("IsPointerOver");
        var proc = (nint hWnd, uint msg, nint wParam, nint lParam, ref bool handled) =>
        {
            switch (msg)
            {
                case 533:
                    if (!pointerOnMaxButton) break;
                    if (!CanMaximize) break;
                    WindowState = WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;
                    break;
                case 0x0084:
                    var point = new PixelPoint(
                        (short)(ToInt32(lParam) & 0xffff),
                        (short)(ToInt32(lParam) >> 16));
                    var desiredSize = maximize.DesiredSize;
                    var buttonLeftTop = maximize.PointToScreen(FlowDirection == FlowDirection.LeftToRight
                                                               ? new Point(desiredSize.Width, 0)
                                                               : new Point(0, 0));
                    var x = (buttonLeftTop.X - point.X) / RenderScaling;
                    var y = (point.Y - buttonLeftTop.Y) / RenderScaling;
                    if (new Rect(0, 0,
                            desiredSize.Width,
                            desiredSize.Height)
                        .Contains(new Point(x, y)))
                    {
                        setter?.SetValue(maximize, true);
                        pointerOnMaxButton = true;
                        handled = true;
                        return 9;
                    }

                    pointerOnMaxButton = false;
                    setter?.SetValue(maximize, false);
                    break;
            }

            return nint.Zero;

            static int ToInt32(nint ptr) => nint.Size == 4
                ? ptr.ToInt32()
                : (int)(ptr.ToInt64() & 0xffffffff);
        };

        Win32Properties.AddWndProcHookCallback(this, new Win32Properties.CustomWndProcHookCallback(proc));
    }

    bool oldShowBottomBorder = true;
    private void OnWindowStateChanged(WindowState state)
    {
        if (state == WindowState.FullScreen)
        {
            oldShowBottomBorder = ShowBottomBorder;
            CanResize = CanMove = false;
            ShowBottomBorder = false;
        }
        else if (state == WindowState.Maximized)
        {
            Margin = new Thickness(7);
            CanResize = CanMove = true;
            ShowBottomBorder = oldShowBottomBorder;
        }
        else
        {
            Margin = new Thickness(0);
            CanResize = CanMove = true;
            ShowBottomBorder = oldShowBottomBorder;
        }
    }

    private void OnTitleBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        BeginMoveDrag(e);
    }
}
