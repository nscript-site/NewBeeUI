using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Declarative;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
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

    #region Tooltip Helpers

    public static Action<Button>? GetSetTooltipPosition(ToolTipPosition toolTipPosition)
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

    public static void DisplayToolTipAtTop(Control ctrl)
    {
        ToolTip.SetPlacement(ctrl, PlacementMode.Top);
        ToolTip.SetVerticalOffset(ctrl, -5);
    }

    public static void DisplayToolTipAtBottom(Control ctrl)
    {
        ToolTip.SetPlacement(ctrl, PlacementMode.Bottom);
        ToolTip.SetVerticalOffset(ctrl, 5);
    }

    public static void DisplayToolTipAtLeft(Control ctrl)
    {
        ToolTip.SetPlacement(ctrl, PlacementMode.Left);
    }

    public static void DisplayToolTipAtRight(Control ctrl)
    {
        ToolTip.SetPlacement(ctrl, PlacementMode.Right);
    }

    #endregion

    #region Control Create Helpers

    public static Button TextButton(string text, double? fontSize = null)
    {
        return new Button() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center }.Text(text, fontSize);
    }

    public static CheckBox CheckBox(string? text = null)
    {
        var cb = new CheckBox();
        if (text != null) cb.Text(text);
        return cb;
    }

    public static Border Border(Control? child = null,
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

    public static TextBox TextBox(string? text = null)
    {
        var tb = new TextBox();
        if (text != null) tb.Text(text);
        return tb;
    }

    public static TextBlock TextBlock(string? text = null, bool wrap = false)
    {
        var tb = new TextBlock() { TextWrapping = wrap ? TextWrapping.Wrap : TextWrapping.NoWrap };
        if (text != null) tb.Text(text);
        return tb;
    }

    public static PathIcon Icon(StreamGeometry g, double? size = null)
    {
        var p = new PathIcon().Data(g);
        if(size != null) p.Size(size.Value);
        return p;
    }

    public static Button IconButton(StreamGeometry g, string? tooltip = null, ToolTipPosition toolTipPosition = ToolTipPosition.Auto,
        double scale = 1.0, double? iconSize = null)
    {
        return CreateButton(new PathIcon().Data(g), null, tooltip, scale, iconSize, onSetTooltipPosition: GetSetTooltipPosition(toolTipPosition));
    }

    public static Button IconButton(Func<StreamGeometry> g, string? tooltip = null, ToolTipPosition toolTipPosition = ToolTipPosition.Auto, 
        double scale = 1.0, double? iconSize = null)
    {
        return CreateButton(new PathIcon().Data(g), null, tooltip, scale, iconSize, onSetTooltipPosition: GetSetTooltipPosition(toolTipPosition));
    }

    public static Button IconButton(string text, StreamGeometry g, string? tooltip = null, ToolTipPosition toolTipPosition = ToolTipPosition.Auto, 
        double scale = 1.0, double? iconSize = null)
    {
        return CreateButton(new PathIcon().Data(g), text, tooltip, scale, iconSize, onSetTooltipPosition: GetSetTooltipPosition(toolTipPosition));
    }

    public static Button IconButton(string text, Func<StreamGeometry> g, string? tooltip = null, ToolTipPosition toolTipPosition = ToolTipPosition.Auto, 
        double scale = 1.0, double? iconSize = null)
    {
        return CreateButton(new PathIcon().Data(g), text, tooltip, scale, iconSize, onSetTooltipPosition: GetSetTooltipPosition(toolTipPosition));
    }

    public static Button CreateButton(PathIcon? icon, string? text, string? tooltip, 
        double scale, double? iconSize = null, Action<Button>? onSetTooltipPosition = null)
    {
        Control? textControl = text == null ? null : TextBlock(text);
        Control? content = null;
        
        if (text != null && icon != null)
            content = HStack([icon, textControl!]).Align(null,0);
        else if (icon != null)
            content = icon;
        else if(text != null)
            content = textControl;

        if(icon != null)
        {
            if(iconSize != null)
            {
                icon.Width = iconSize.Value;
                icon.Height = iconSize.Value;
            }
        }

        var button = new Button().RenderTransform(new ScaleTransform(scale, scale));

        if (icon != null)
        {
            button.Observable(Button.ForegroundProperty, fg => icon.Foreground = fg);
            if (textControl == null)
                button.Classes("Icon").Classes(Classed_Icon_Button);
        }

        if(content != null) button.Content(content);

        if (string.IsNullOrEmpty(tooltip) == false)
        {
            onSetTooltipPosition?.Invoke(button);
            ToolTip.SetTip(button, tooltip);
        }

        return button;
    }

    protected MenuItem Menu(string txt, StreamGeometry? g, Action? action)
    {
        var menu = new MenuItem().Header(txt);
        if (g != null) menu.Icon = new PathIcon().Data(g);
        if (action != null) menu.OnClick(_ => { action(); });
        return menu;
    }

    protected MenuItem MenuF(string txt, Func<StreamGeometry?> g, Action? action)
    {
        var menu = new MenuItem().Header(txt);
        if (g != null) menu.Icon = new PathIcon().Data(g);
        if (action != null) menu.OnClick(_ => { action(); });
        return menu;
    }

    public static PathIcon PathIcon(StreamGeometry g)
    {
        return new PathIcon().Data(g);
    }

    public static PathIcon PathIcon(Func<StreamGeometry> g)
    {
        return new PathIcon().Data(g);
    }

    public static Panel Panel(params Control?[] controls)
    {
        var panel = new Panel();
        if (controls != null)
        {
            foreach (var c in controls)
            {
                if (c == null) continue;
                panel.Children.Add(c);
            }
        }
        return panel;
    }

    public static Grid Grid(string? rows = null, string? cols = null, Control?[]? controls = null)
    {
        var g = new Grid();
        if (rows != null) g.Rows(rows);
        if (cols != null) g.Cols(cols);

        if(controls != null)
        {
            foreach (var c in controls)
            {
                if (c == null) continue;
                g.Children.Add(c);
            }
        }

        return g;
    }

    public static Grid HGrid(string? cols, Control?[] controls)
    {
        var g = new Grid();
        if (cols != null) g.Cols(cols);
        int idx = 0;
        foreach (var c in controls)
        {
            if (c != null)
            {
                c.Col(idx);
                g.Children.Add(c);
            }
            idx++;
        }
        return g;
    }

    public static Grid VGrid(string? rows, Control?[] controls)
    {
        var g = new Grid();
        if (rows != null) g.Rows(rows);
        int idx = 0;
        foreach (var c in controls)
        {
            if (c != null)
            {
                c.Row(idx);
                g.Children.Add(c);
            }
            idx++;
        }
        return g;
    }

    public static StackPanel HStack(Control[] controls)
    {
        var stack = new StackPanel() { Orientation = Orientation.Horizontal, Spacing = 10 };
        stack.Children.AddRange(controls);
        return stack;
    }

    public static WrapPanel WrapPanel(Control[] controls)
    {
        var wrap = new WrapPanel();
        wrap.Children.AddRange(controls);
        return wrap;
    }

    public static Border Border(Control? control)
    {
        var border = new Border();
        if (control != null) border.Child = control;
        return border;
    }

    public static StackPanel HStack(int? hAlign = -1, int? vAlign = 0)
    {
        return new StackPanel() { Orientation = Orientation.Horizontal, Spacing = 10 }.Align(hAlign,vAlign);
    }

    public static StackPanel VStack(Control[] controls)
    {
        var stack = new StackPanel() { Orientation = Orientation.Vertical, Spacing = 10 };
        stack.Children.AddRange(controls);
        return stack;
    }

    public static Control[] Build(Control?[]?[] controls)
    {
        var list = new List<Control>();
        foreach (var arr in controls)
        {
            if (arr == null) continue;
            foreach (var c in arr)
            {
                if (c != null) list.Add(c);
            }
        }
        return list.ToArray();
    }

    public static StackPanel VStack(int? hAlign = -1, int? vAlign = 0)
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

    public static Panel VLine(int width = 1)
    {
        return new Panel().Width(width).VerticalAlignment(VerticalAlignment.Stretch);
    }

    public static Control HLine(double height = 1, double opacity = 0.6, Thickness? margin = null, Action<Control>? onCreate = null)
    {
        var panel = new Panel().Background(BaseView.R("SukiBorderBrush")).Opacity(opacity)
            .Height(height).HorizontalAlignment(HorizontalAlignment.Stretch);
        if (margin == null)
            panel.Margin(0, 10, 0, 0);
        else
            panel.Margin(margin.Value);

        if (onCreate != null)
            onCreate(panel);

        return panel;
    }

    /// <summary>
    /// 水平向的文字 TabItem
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="textAlign"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static TabItem HTextTabItem(string txt, Control? content = null, int textAlign = 0, float size = 14)
    {
        var h = new TextBlock
        {
            Text = txt,
            HorizontalAlignment = textAlign switch
            {
                < 0 => HorizontalAlignment.Left,
                0 => HorizontalAlignment.Center,
                > 0 => HorizontalAlignment.Right
            },
            FontSize = size // 设置字体大小
        };

        return new TabItem() { Header = h, Content = content };
    }

    public static Grid HTabs(TabItem[] tabItems, int selectedIndex = 0)
    {
        TabItem? selected = null;

        if(selectedIndex >= 0 && selectedIndex < tabItems.Length)
            selected = tabItems[selectedIndex];

        // 右侧内容
        var right = new ContentPresenter()
            .VerticalAlignment(VerticalAlignment.Top);

        Object BuildRightContent()
        {
            return selected?.Content ?? TextBlock("No Content");
        }

        return new Grid().Cols("Auto,1,*")
            .Children([
                // 左侧选项卡
                new TabHeaderList()
                    .ItemsPanel(new StackPanel().Margin(10,10).Orientation(Orientation.Vertical))
                    .HorizontalAlignment(HorizontalAlignment.Right).Items(tabItems)
                    .OnSelectionChanged(
                        e=>{
                            if (e.AddedItems.Count == 0) return;
                            selected = e.AddedItems[0] as TabItem;
                            if(right != null) right.Content = BuildRightContent();
                        }),
                
                // 分隔线
                new Panel().Col(1).Background(R("SukiBorderBrush")).Width(1).VerticalAlignment(VerticalAlignment.Stretch),

                right.Col(2).Content(BuildRightContent())
        ]);
    }

    #endregion

    #region DynamicResource and Colors

    public static DynamicResourceExtension R(string key)
    {
        return new DynamicResourceExtension(key);
    }

    public static DynamicResourceExtension R_PrimaryColor
    {
        get => new DynamicResourceExtension("SukiPrimaryColor");
    }

    public static DynamicResourceExtension R_TextColor
    {
        get => new DynamicResourceExtension("SukiText");
    }

    public static DynamicResourceExtension R_LightBorderBrush
    {
        get => new DynamicResourceExtension("SukiLightBorderBrush");
    }

    #endregion

    /// <summary>
    /// 显示 ToastView
    /// </summary>
    /// <param name="message"></param>
    /// <param name="seconds"></param>
    /// <param name="opacity"></param>
    /// <param name="compactMode"></param>
    /// <param name="hAlign"></param>
    /// <param name="vAlign"></param>
    /// <param name="onCreate"></param>
    public void ShowToastView(string message, double seconds = 2, double opacity = 1, bool compactMode = false, int hAlign = 0, int vAlign = -1, Action<ToastView>? onCreate = null)
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return;
        var toast = new ToastView() { IsTemporary = true };
        onCreate?.Invoke(toast);
        hosts.Add(toast);
        toast.ShowToast(message, seconds, opacity, compactMode, hAlign, vAlign);
    }

    public void ShowLoading(Action<Loading>? onCreate = null)
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return;

        this.IsEnabled = false;
        var Loading = new Loading().Align(0, 0);
        onCreate?.Invoke(Loading);
        hosts.Add(Loading);
    }

    /// <summary>
    /// 展示在窗口的 Overlay 上, 如果窗口不存在，则返回 false。
    /// 本方法仅用于桌面环境。
    /// </summary>
    /// <returns></returns>
    public bool ShowInOverlay(BaseView owner)
    {
        var hosts = owner.OverlayHosts();
        if (hosts == null) return false;
        hosts.Add(this);
        return false;
    }

    public bool RemoveFromOverlay()
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return false;
        return hosts.Remove(this);
    }

    public void RemoveLoading()
    {
        if(this.IsEnabled == false) this.IsEnabled = true;

        var hosts = this.OverlayHosts();
        if (hosts == null || hosts.Count == 0) return;
        for(int i = hosts.Count - 1; i >= 0; i--)
        {
            var ctrl = hosts[i];
            if (ctrl is Loading)
            {
                hosts.RemoveAt(i);
            }
        }
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

    public static TButton Text<TButton>(this TButton button, string text, double? fontSize = null) where TButton : Button
    {
        var tb = new TextBlock().Text(text);
        if(fontSize != null) tb.FontSize = fontSize.Value;
        button.Content(tb);
        return button;
    }

    public static T RunInBackground<T>(this T ctrl, Action action) where T : Control
    {
        Task.Run(action);
        return ctrl;
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

    //public static TPanel Children<TPanel>(this TPanel container, params Box<Control>[] arrs) where TPanel : Panel
    //{
    //    if (arrs == null) return container;

    //    foreach (var arr in arrs)
    //    {
    //        var ctrl = arr.Unbox();
    //        if(ctrl != null)
    //            container.Children.Add(ctrl);
    //    }

    //    return container;
    //}

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

    public static double Clamp(this double val, double min, double max)
    {
        val = Math.Min(max, val);
        val = Math.Max(min, val);
        return val;
    }

    public static float Clamp(this float val, float min, float max)
    {
        val = Math.Min(max, val);
        val = Math.Max(min, val);
        return val;
    }

    public static int Clamp(this int val, int min, int max)
    {
        val = Math.Min(max, val);
        val = Math.Max(min, val);
        return val;
    }

    public static T TryChild<T>(this T control, Control? value) where T : Decorator
    {
        if(value == null) return control;
        control.Child = value;
        return control;
    }

    public static T OnClick<T>(this T control, Action action, RoutingStrategies? routes = null) where T : Button
    {
        return control.OnClick(_ =>
        {
            action();
        }, routes);
    }

    public static T? AppendTo<T>(this T? control, IList<T> list) where T : Control
    {
        if (control == null) return control;

        list.Add(control);
        return control;
    }

    public static T AppendWithTo<T,TItem2>(this T control, TItem2 item2, IList<Tuple<T,TItem2>> list) where T : Control
    {
        list.Add(new Tuple<T,TItem2>(control, item2));
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
        if (window != null)
        {
            return window.Hosts;
        }

        var root = ctrl.GetVisualRoot() as TopLevel;
        if (root == null) return null;

        HostView? hostView = root.Content as HostView;
        if (hostView != null) return hostView.Hosts.Children;

        return null;
    }

    public static T Flyout<T>(this T control, Control? flyoutContent, Action<Flyout>? onFlyout = null) where T : Button
    {
        if(flyoutContent == null) return control;

        var flyout = new Flyout();
        flyout.Content = flyoutContent;
        onFlyout?.Invoke(flyout);
        return control.Flyout(flyout);
    }

    /// <summary>
    ///   当  BaseView 为 null 时，直接执行操作。不为空时，如果执行超时，在 BaseView 上显示加载状态。
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="action"></param>
    /// <param name="minDelayMilliseconds"></param>
    /// <param name="onCreate"></param>
    public static T? RunWithDelayedLoading<T>(this T? owner, Action action, int minDelayMilliseconds = 200, bool runAtBackground = false, Action<Loading>? onCreate = null) where T:BaseView
    {
        if(runAtBackground == false)
            owner?.RunWithDelayedLoadingCore(action, minDelayMilliseconds,onCreate);
        else
            Task.Run(() => owner?.RunWithDelayedLoadingCore(action, minDelayMilliseconds, onCreate));
        return owner;
    }

    private static void RunWithDelayedLoadingCore<T>(this T? owner, Action action, int minDelayMilliseconds = 200, Action<Loading>? onCreate = null) where T : BaseView
    {
        if (owner == null)
        {
            action();
            return;
        }

        bool focused = owner.IsFocused;

        bool isLoading = true;

        try
        {
            // 过少许时间后再显示
            Task.Delay(minDelayMilliseconds).ContinueWith(_ =>
            {
                if (isLoading == false) return;

                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (isLoading == false) return;
                    owner.ShowLoading(onCreate);
                });
            });

            action();
        }
        finally
        {
            isLoading = false;

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                owner.RemoveLoading();
                if(focused == true) owner.Focus();          // 不 focus 会失去焦点
            });
        }
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

    public static Style<TElement> Select<TElement>(this Style<TElement> style, Func<Selector, Selector> selector) where TElement : StyledElement
    {
        if(style.Selector != null)
            style.Selector = selector(style.Selector);
        else
            style.Selector = selector(TypeSelector(null));

        return style;
        static Selector TypeSelector(Selector? s)
        {
            return s.OfType<TElement>();
        }
    }

    public static T Scale<T>(this T control, double scale) where T : Visual
    {
        return control.RenderTransform(new ScaleTransform(scale, scale));
    }

    public static bool TryEquals<T>(this T a, T? b) where T : struct
    {
        if(b == null) return false;
        return a.Equals(b.Value);
    }

    public static bool TryEquals<T>(this T a, T? b) where T : class
    {
        if (b == null) return false;
        return a.Equals(b);
    }

    public static string? TryAddLeftContent(this string? content, string prefix)
    {
        if (content == null) return null;
        return prefix + content;
    }

    public static Button CreateWindowIcon(this IWindowView win, StreamGeometry g, string? tooltip = null, double scale = 0.8)
    {
        return win.CreateWindowIcon(new PathIcon().Data(g), tooltip, scale);
    }

    public static Button CreateWindowIcon(this IWindowView win, Func<StreamGeometry> g, string? tooltip = null, double scale = 0.8)
    {
        return win.CreateWindowIcon(new PathIcon().Data(g), tooltip, scale);
    }

    private static Button CreateWindowIcon(this IWindowView win, PathIcon path, string? tooltip, double scale)
    {
        var button = new Button().Classes("Basic").Classes("WindowControlsButton")
            .Content(path.Ref(out PathIcon icon).Width(16).Height(16));

        button.Tag = path;

        if (string.IsNullOrEmpty(tooltip) == false)
        {
            ToolTip.SetTip(button, tooltip);
        }

        return button;
    }

    #region 简化动作回调

    public static T WhenLoaded<T>(this T ctrl, Action<T> action) where T : Control
    {
        ctrl.OnLoaded((Avalonia.Interactivity.RoutedEventArgs _) => action(ctrl));
        return ctrl;
    }

    public static T WhenClick<T>(this T ctrl, Action<T> action) where T : Control
    {
        ctrl.OnTapped(_ => action(ctrl));
        return ctrl;
    }

    public static T WhenDoubleClick<T>(this T ctrl, Action<T> action) where T : Control
    {
        ctrl.OnDoubleTapped(_ => action(ctrl));
        return ctrl;
    }

    #endregion


    #region 动画

    public static T Opacity<T>(this T ctrl, double duration, double from, double to) where T : Visual
    {
        ctrl.Animate<double>(Visual.OpacityProperty, from, to, TimeSpan.FromSeconds(duration));
        return ctrl;
    }

    public static T Transform<T>(this T ctrl, double duration, Transform from, Transform to) where T : Visual
    {
        ctrl.Animate<Transform>(Visual.RenderTransformProperty, from, to, TimeSpan.FromSeconds(duration));
        return ctrl;
    }

    public static T Move<T>(this T ctrl, double duration, double fromX, double fromY, double toX, double toY) where T : Visual
    {
        if(fromX != toX)
            ctrl.Animate<double>(TranslateTransform.XProperty, fromX, toX, TimeSpan.FromSeconds(duration));
    
        if(fromY != toY)
            ctrl.Animate<double>(TranslateTransform.YProperty, fromY, toY, TimeSpan.FromSeconds(duration));
        return ctrl;
    }

    public static T Rotate<T>(this T ctrl, double duration, double fromAngle, double toAngle) where T : Visual
    {
        ctrl.Animate<double>(RotateTransform.AngleProperty, fromAngle, toAngle, TimeSpan.FromSeconds(duration));
        return ctrl;
    }

    public static T Scale<T>(this T ctrl, double duration, double fromScale, double toScale) where T : Visual
    {
        return ctrl.Scale(duration, fromScale, fromScale, toScale, toScale);
    }

    public static T Scale<T>(this T ctrl, double duration, double fromScaleX, double fromScaleY, double toScaleX, double toScaleY) where T : Visual
    {
        if (fromScaleX != toScaleX)
            ctrl.Animate<double>(ScaleTransform.ScaleXProperty, fromScaleX, toScaleX, TimeSpan.FromSeconds(duration));

        if (fromScaleY != toScaleY)
            ctrl.Animate<double>(ScaleTransform.ScaleYProperty, fromScaleY, toScaleY, TimeSpan.FromSeconds(duration));
        return ctrl;
    }

    #endregion
}