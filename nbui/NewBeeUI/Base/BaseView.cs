using Avalonia.Input;
using Avalonia.Markup.Declarative;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Threading;
using System.Windows.Input;

namespace NewBeeUI;

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

    protected Button IconButton(StreamGeometry g, string? tooltip = null, double scale = 0.8)
    {
        return CreateIconButton(new PathIcon().Data(g), tooltip, scale);
    }

    protected Button IconIconButton(Func<StreamGeometry> g, string? tooltip = null, double scale = 0.8)
    {
        return CreateIconButton(new PathIcon().Data(g), tooltip, scale);
    }

    protected Button CreateIconButton(PathIcon path, string? tooltip, double scale)
    {
        var button = new Button().Classes("Icon").Classes(Classed_Icon_Button)
            .Content(path.Ref(out PathIcon icon))
            .Observable(Button.ForegroundProperty, fg => icon.Foreground = fg).RenderTransform(new ScaleTransform(scale, scale));

        if (string.IsNullOrEmpty(tooltip) == false)
        {
            ToolTip.SetPlacement(button, PlacementMode.Top);
            ToolTip.SetVerticalOffset(button, -5);
            ToolTip.SetTip(button, tooltip);
        }

        return button;
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

    protected StackPanel HStack(int? hAlign = -1, int? vAlign = 0)
    {
        return new StackPanel() { Orientation = Orientation.Horizontal, Spacing = 8 }.Align(hAlign,vAlign);
    }

    protected StackPanel VStack(int? hAlign = -1, int? vAlign = 0)
    {
        return new StackPanel() { Orientation = Orientation.Vertical }.Align(hAlign, vAlign);
    }

    public static IconView SelectableIconButton(StreamGeometry g, string? tooltip = null, string? selectedTooltip = null, double scale = 0.8)
    {
        return CreateSelectableIcon(new PathIcon().Data(g), tooltip, selectedTooltip, scale);
    }

    public static IconView CreateSelectableIcon(PathIcon path, string? tooltip, string? selectedTooltip, double scale)
    {
        var iconView = new IconView();
        iconView.Path = path;
        iconView.Tooltip = tooltip;
        iconView.SelectedTooltip = selectedTooltip;
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

    //public static T Observable<T,TProperty>(this T ctrl, AvaloniaProperty<TProperty?> property,Action<TProperty?> onUpdate ) where T : Control
    //{
    //    ctrl.GetObservable(property).Subscribe(onUpdate);
    //    return ctrl;
    //}

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
}