using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Markup.Declarative;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Threading;
using HarfBuzzSharp;
using NStyles;
using NStyles.Controls;
using System.Windows.Input;

namespace NewBeeUI;

public enum ToolTipPosition
{
    Auto,
    Top,
    Bottom,
    Left,
    Right
}

public abstract class BaseView : MvuView
{
    public const string Classed_Icon_Button = "IconButton";

    //public static I18N I18N => I18N.Instance;

    public BaseView() : base(true)
    {
    }

    protected override void InitializeState()
    {
        base.InitializeState();
        var topLevel = TopLevel.GetTopLevel(this)!;
        if(topLevel != null && this.KeyBindings?.Count > 0)
            topLevel.KeyBindings.AddRange(this.KeyBindings);
    }

    protected void InvokeByUIThread(Action action)
    {
        Dispatcher.UIThread.InvokeAsync(action);
    }

    protected Button TextButton(string text)
    {
        return new Button() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center }.Text(text);
    }

    protected CheckBox CheckBox(string? text = null)
    {
        var cb = new CheckBox();
        if(text != null) cb.Text(text);
        return cb;
    }

    protected Border Border(Control? child = null, 
        double thickness = 0,
        double? width = null, double? height = null)
    {
        var b = new Border();
        b.BorderThickness = new Thickness(thickness);
        if (child != null) b.Child(child);
        if (width != null) b.Width(width.Value);
        if (height != null) b.Height(height.Value);
        return b;
    }

    protected TextBox TextBox(string? text = null)
    {
        var tb = new TextBox();
        if (text != null) tb.Text(text);
        return tb;
    }

    protected TextBlock TextBlock(string? text = null, bool wrap = false)
    {
        var tb = new TextBlock() { TextWrapping = wrap ? TextWrapping.Wrap : TextWrapping.NoWrap };
        if (text != null) tb.Text(text);
        return tb;
    }

    protected Button IconButton(StreamGeometry g, string? tooltip = null, ToolTipPosition toolTipPosition = ToolTipPosition.Auto, double scale = 1.0)
    {
        return CreateIconButton(new PathIcon().Data(g), tooltip, scale, GetSetTooltipPosition(toolTipPosition));
    }

    protected Button IconIconButton(Func<StreamGeometry> g, string? tooltip = null, ToolTipPosition toolTipPosition = ToolTipPosition.Auto, double scale = 1.0)
    {
        return CreateIconButton(new PathIcon().Data(g), tooltip, scale, GetSetTooltipPosition(toolTipPosition));
    }

    protected Button CreateIconButton(PathIcon path, string? tooltip, double scale, Action<Button>? onSetTooltipPosition)
    {
        var button = new Button().Classes("Icon").Classes(Classed_Icon_Button)
            .Content(path.Ref(out PathIcon icon))
            .Observable(Button.ForegroundProperty, fg => icon.Foreground = fg).RenderTransform(new ScaleTransform(scale, scale));

        if (string.IsNullOrEmpty(tooltip) == false)
        {
            onSetTooltipPosition?.Invoke(button);
            ToolTip.SetTip(button, tooltip);
        }

        return button;
    }

    private static Action<Button>? GetSetTooltipPosition(ToolTipPosition toolTipPosition)
    {
        return toolTipPosition switch
        {
            ToolTipPosition.Top => DisplayToolTipAtTop,
            ToolTipPosition.Bottom => DisplayToolTipAtBottom,
            ToolTipPosition.Left => DisplayToolTipAtLeft,
            ToolTipPosition.Right => DisplayToolTipAtRight,
            ToolTipPosition.Auto => null, // Use default position
            _ => null
        };
    }

    protected static void DisplayToolTipAtTop(Control ctrl)
    {
        ToolTip.SetPlacement(ctrl, PlacementMode.Top);
        ToolTip.SetVerticalOffset(ctrl, -5);
    }

    protected static void DisplayToolTipAtBottom(Control ctrl)
    {
        ToolTip.SetPlacement(ctrl, PlacementMode.Bottom);
        ToolTip.SetVerticalOffset(ctrl, 5);
    }

    protected static void DisplayToolTipAtLeft(Control ctrl)
    {
        ToolTip.SetPlacement(ctrl, PlacementMode.Left);
    }

    protected static void DisplayToolTipAtRight(Control ctrl)
    {
        ToolTip.SetPlacement(ctrl, PlacementMode.Right);
    }

    protected PathIcon PathIcon(StreamGeometry g)
    {
        return new PathIcon().Data(g);
    }

    protected PathIcon PathIcon(Func<StreamGeometry> g)
    {
        return new PathIcon().Data(g);
    }

    protected Grid Grid(string? rows = null, string? cols = null)
    {
        var g = new Grid();
        if (rows != null) g.Rows(rows);
        if (cols != null) g.Cols(cols);
        return g;
    }

    protected StackPanel HStack(Control[] controls)
    {
        var stack = new StackPanel() { Orientation = Orientation.Horizontal, Spacing = 10 };
        stack.Children.AddRange(controls);
        return stack;
    }

    protected StackPanel HStack(int? hAlign = -1, int? vAlign = 0)
    {
        return new StackPanel() { Orientation = Orientation.Horizontal, Spacing = 10 }.Align(hAlign,vAlign);
    }

    protected StackPanel VStack(Control[] controls)
    {
        var stack = new StackPanel() { Orientation = Orientation.Vertical, Spacing = 10 };
        stack.Children.AddRange(controls);
        return stack;
    }

    protected StackPanel VStack(int? hAlign = -1, int? vAlign = 0)
    {
        return new StackPanel() { Orientation = Orientation.Vertical, Spacing = 10 }.Align(hAlign, vAlign);
    }

    public static IconView SelectableIconButton(StreamGeometry g, string? tooltip = null, string? selectedTooltip = null, ToolTipPosition toolTipPosition = ToolTipPosition.Auto, double scale = 1.0)
    {
        return CreateSelectableIcon(new PathIcon().Data(g), tooltip, selectedTooltip, toolTipPosition, scale);
    }

    public static IconView CreateSelectableIcon(PathIcon path, string? tooltip, string? selectedTooltip, ToolTipPosition toolTipPosition, double scale)
    {
        var iconView = new IconView();
        iconView.RenderTransform = new ScaleTransform(scale, scale);
        iconView.Path = path;
        iconView.Tooltip = tooltip;
        iconView.SelectedTooltip = selectedTooltip;
        iconView.OnSetTooltipPosition = GetSetTooltipPosition(toolTipPosition);
        return iconView;
    }

    public static DynamicResourceExtension R(string key)
    {
        return new DynamicResourceExtension(key);
    }

    protected Panel VLine()
    {
        return new Panel().Width(1).VerticalAlignment(VerticalAlignment.Stretch);
    }
}

public static class BaseViewExtensions
{
    public class KeyActionCommand : ICommand
    {
        private readonly Action _action;

        public KeyActionCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            _action();
        }

        public event EventHandler? CanExecuteChanged;
    }

    public static T Size<T>(this T ctrl, double size) where T : Control
    {
        ctrl.Width = size;
        ctrl.Height = size;
        return ctrl;
    }

    public static T Size<T>(this T ctrl, double w, double h) where T : Control
    {
        ctrl.Width = w;
        ctrl.Height = h;
        return ctrl;
    }

    public static Point GetCanvasPosition<T>(this T ctrl) where T : Control
    {
        double x = ctrl.GetValue(Canvas.LeftProperty);
        double y = ctrl.GetValue(Canvas.TopProperty);
        return new Point(x, y);
    }

    public static T Canvas_Position<T>(this T ctrl, double x, double y) where T : Control
    {
        ctrl.SetValue(Canvas.LeftProperty, x);
        ctrl.SetValue(Canvas.TopProperty, y);
        return ctrl;
    }

    public static TButton Text<TButton>(this TButton button, string text) where TButton : Button
    {
        button.Content(new TextBlock().Text(text));
        return button;
    }

    public static T OnKey<T>(this T ctrl, Key key, Action action) where T : Control
    {
        ctrl.KeyBindings.Add(new KeyBinding()
        {
            Gesture = new KeyGesture(key),
            Command = new KeyActionCommand(action)
        });

        return ctrl;
    }

    public static T OnKey<T>(this T ctrl, (KeyModifiers, Key) key, Action action) where T : Control
    {
        ctrl.KeyBindings.Add(new KeyBinding()
        {
            Gesture = new KeyGesture(key.Item2, key.Item1),
            Command = new KeyActionCommand(action)
        });

        return ctrl;
    }

    public static T OnKey<T>(this T ctrl, Key[] keys, Action action) where T : Control
    {
        var command = new KeyActionCommand(action);

        foreach (var key in keys)
        {
            ctrl.KeyBindings.Add(new KeyBinding()
            {
                Gesture = new KeyGesture(key),
                Command = command
            });
        }

        return ctrl;
    }

    public static (KeyModifiers, Key) With(this Key key, KeyModifiers modifiers)
    {
        return (modifiers, key);
    }

    public static T OnKey<T>(this T ctrl, (KeyModifiers, Key)[] keys, Action action) where T : Control
    {
        var command = new KeyActionCommand(action);

        foreach (var key in keys)
        {
            ctrl.KeyBindings.Add(new KeyBinding()
            {
                Gesture = new KeyGesture(key.Item2,key.Item1),
                Command = command
            });
        }

        return ctrl;
    }

    public static T SuccessStyle<T>(this T button) where T : Control
    {
        return button.Classes("Success");
    }

    public static T DangerStyle<T>(this T button) where T : Control
    {
        return button.Classes("Danger");
    }

    public static T AccentStyle<T>(this T button) where T : Control
    {
        return button.Classes("Accent");
    }

    public static T OutlinedStyle<T>(this T button) where T : Control
    {
        return button.Classes("Outlined");
    }

    public static T FlatStyle<T>(this T button) where T : Control
    {
        return button.Classes("Flat");
    }

    public static T BasicStyle<T>(this T button) where T : Control
    {
        return button.Classes("Basic");
    }

    public static ScrollViewer ScrollViewer<T>(this T button) where T : Control
    {
        return new ScrollViewer().Content(button);
    }

    public static T Align<T>(this T ctrl, int? hAlign = 0, int? vAlign = 0) where T : Control
    {
        if (hAlign == null) ctrl.HorizontalAlignment = HorizontalAlignment.Stretch;
        else if (hAlign == 0) ctrl.HorizontalAlignment = HorizontalAlignment.Center;
        else if (hAlign < 0) ctrl.HorizontalAlignment = HorizontalAlignment.Left;
        else ctrl.HorizontalAlignment = HorizontalAlignment.Right;
        if (vAlign == null) ctrl.VerticalAlignment = VerticalAlignment.Stretch;
        else if (vAlign == 0) ctrl.VerticalAlignment = VerticalAlignment.Center;
        else if (vAlign < 0) ctrl.VerticalAlignment = VerticalAlignment.Top;
        else ctrl.VerticalAlignment = VerticalAlignment.Bottom;
        return ctrl;
    }

    public static TPanel Children<TPanel>(this TPanel container, params Control[]?[]? arrs) where TPanel : Panel
    {
        if (arrs == null) return container;

        foreach (var arr in arrs)
        {
            if (arr == null || arr.Length == 0) continue;

            foreach(var item in arr)
            {
                if (item is null) continue;

                container.Children.Add(item);
            }
        }

        return container;
    }

    public static TPanel Children<TPanel>(this TPanel container, params Box<Control>[] arrs) where TPanel : Panel
    {
        if (arrs == null) return container;

        foreach (var arr in arrs)
        {
            var ctrl = arr.Unbox();
            if(ctrl != null)
                container.Children.Add(ctrl);
        }

        return container;
    }

    public static Object? FirstItem(this SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            return e.AddedItems[0];
        }
        return null;
    }

    public static T Icon<T>(this T builder, Func<PathIcon?> iconBuilder)
        where T : RoutedViewBuilder
    {
        builder.Icon = iconBuilder();
        return builder;
    }

    public static T Icon<T>(this T builder, PathIcon? icon)
        where T : RoutedViewBuilder
    {
        builder.Icon = icon;
        return builder;
    }

    public static T Icon<T>(this T builder, Func<StreamGeometry> iconBuilder)
        where T : RoutedViewBuilder
    {
        builder.Icon = new PathIcon().Data(iconBuilder());
        return builder;
    }

    public static T Icon<T>(this T builder, StreamGeometry? icon)
        where T : RoutedViewBuilder
    {
        if (icon == null) builder.Icon = null;
        else builder.Icon = new PathIcon().Data(icon);
        return builder;
    }

    public static T TryChild<T>(this T control, Control? value) where T : Decorator
    {
        if(value == null) return control;
        control.Child = value;
        return control;
    }

    public static void ShowDialog<TWindowView>(this TWindowView windowView, string? newTitle = null)
        where TWindowView : BaseView, IWindowView
    {
        _ = windowView.WindowInfo.ShowDialogAsync(windowView, null, newTitle);
    }

    public static void ShowDialog<TWindowView>(this TWindowView windowView, BaseView? owner, string? newTitle = null)
        where TWindowView : BaseView, IWindowView
    {
        _ = windowView.WindowInfo.ShowDialogAsync(windowView, owner, newTitle);
    }

    public static async Task ShowDialogAsync<TWindowView>(this TWindowView windowView, string? newTitle = null)
        where TWindowView : BaseView, IWindowView
    {
        await windowView.WindowInfo.ShowDialogAsync(windowView, null, newTitle);
    }

    public static async Task ShowDialogAsync<TWindowView>(this TWindowView windowView, BaseView? owner, string? newTitle = null)
        where TWindowView : BaseView, IWindowView
    {
        await windowView.WindowInfo.ShowDialogAsync(windowView, owner, newTitle);
    }

    public static void CloseWindow<TWindowView>(this TWindowView windowView)
        where TWindowView : IWindowView
    {
        windowView.WindowInfo.CloseWindow();
    }

    public static Controls? OverlayHosts(this Control ctrl)
    {
        NWindow? window = ctrl.GetDesktopWindow() as NWindow;
        if (window == null)
        {
            // 如果没有找到宿主窗口，则返回空
            return null;
        }

        return window.Hosts;
    }

    public static Window? GetDesktopWindow(this Control ctrl)
    {
        return TopLevel.GetTopLevel(ctrl) as Window;
    }

    public static Window? GetDesktopMainWindow(this Control ctrl)
    {
        var lifetime = Application.Current?.ApplicationLifetime;

        // 判断并转换为 IClassicDesktopStyleApplicationLifetime
        if (lifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }

        return null;
    }
}