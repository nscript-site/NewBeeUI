using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using NStyles.Controls;
using NStyles.MeterialIcons;
using System;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
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

    protected override object Build()
    {
        Control content;
        Build(out content);
        return content;
    }

    protected virtual void Build(out Control content)
    {
        content = new TextBlock().Text("BaseView");
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

    public static Image Image(Stream imageFileStream)
    {
        imageFileStream.Position = 0;
        return new Image().Source(new Avalonia.Media.Imaging.Bitmap(imageFileStream));
    }

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

    public static TextBlock TextBlock(Func<string>? textFunc = null, bool wrap = false)
    {
        var tb = new TextBlock() { TextWrapping = wrap ? TextWrapping.Wrap : TextWrapping.NoWrap };
        if (textFunc != null) tb.Text(textFunc);
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

    protected TopLevel? GetTopLevel()
    {
        return TopLevel.GetTopLevel(this);
    }

    protected int WindowsWidth()
    {
        var topLevel = GetTopLevel();
        if (topLevel != null)
            return (int)topLevel.Bounds.Width;
        return 0;
    }

    protected int WindowsHeight()
    {
        var topLevel = GetTopLevel();
        if (topLevel != null)
            return (int)topLevel.Bounds.Height;
        return 0;
    }

    protected Size WindowsSize()
    {
        var topLevel = GetTopLevel();
        if (topLevel != null)
            return topLevel.Bounds.Size;
        return new Size(0, 0);
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

    public static ScrollViewer Scrollable(Control? content = null)
    {
        var sv = new ScrollViewer();
        if (content != null) sv.Content = content;
        return sv;
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

    public static Control DemoViewCodeView([CallerFilePath] string filePath = "", string text = "<code>", string baseUrl = "https://github.com/nscript-site/NewBeeUI/tree/main/demo/NewBeeUI.Demo/Views/")
    {
        void ShowSourceCode(string fileName, string code)
        {
            var sourceView = new SourceCodeView()
            {
                FileName = fileName,
                Codes = code
            };
            sourceView.ShowDialog();
        }

        var fileName = Path.GetFileName(filePath);
            String codes = "";
            if (File.Exists(filePath)) codes = File.ReadAllText(filePath);
            return HStack([
                    TextBlock($"[{fileName}]").Align(0,0).Foreground(R("SukiPrimaryColor")).Margin(0,0,10,0),
                    IconButton(ContentCopyIcon.Instance, "复制源代码", iconSize:14).Size(24).WhenClick(_=>{ CopyToClipboard(codes); }).Align(0,0),
                    IconButton(FileCodeOutlineIcon.Instance, "查看源代码", iconSize:14).Size(24).WhenClick(_=>{ ShowSourceCode(fileName, codes); }).Align(0,0),
                    IconButton(LinkIcon.Instance, "使用浏览器在线浏览源代码", iconSize:14).Size(24).WhenClick(_=>{ OpenUrl($"{baseUrl}{fileName}"); }).Align(0,0),
                ]).Align(0, -1).Margin(0,10).Spacing(2);
    }

    public static HyperlinkButton Hyperlink(string text, string url, string baseUrl = "")
    {
        return new HyperlinkButton()
            .Text(text)
            .NavigateUri(new Uri($"{baseUrl}{url}"));
    }

    public const string BaseView_Classes_HStack = "BaseView_HStack";
    public static StackPanel HStack(Control[] controls, bool useDefaultClasses = true)
    {
        var stack = new StackPanel() { Orientation = Orientation.Horizontal, Spacing = Globals.HStackDefaultSpacing };
        stack.Children.AddRange(controls);
        if (useDefaultClasses) stack.Classes(BaseView_Classes_HStack);
        return stack;
    }

    public static StackPanel HStack(int? hAlign = -1, int? vAlign = 0, bool useDefaultClasses = true)
    {
        var stack = new StackPanel() { Orientation = Orientation.Horizontal, Spacing = Globals.HStackDefaultSpacing }.Align(hAlign,vAlign);
        if (useDefaultClasses) stack.Classes(BaseView_Classes_HStack);
        return stack;
    }

    public const string BaseView_Classes_VStack = "BaseView_VStack";

    public static StackPanel VStack(Control[] controls, bool useDefaultClasses = true)
    {
        var stack = new StackPanel() { Orientation = Orientation.Vertical, Spacing = Globals.VStackDefaultSpacing };
        stack.Children.AddRange(controls);
        if(useDefaultClasses) stack.Classes(BaseView_Classes_VStack);
        return stack;
    }

    public static StackPanel VStack(int? hAlign = -1, int? vAlign = 0, bool useDefaultClasses = true)
    {
        var stack = new StackPanel() { Orientation = Orientation.Vertical, Spacing = Globals.VStackDefaultSpacing }.Align(hAlign, vAlign);
        if (useDefaultClasses) stack.Classes(BaseView_Classes_VStack);
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

    public static Control VLine(int width = 1, double opacity = 0.6, string borderBrush = "SukiBorderBrush", Thickness? margin = null, Action<Control>? onCreate = null)
    {
        var line = new Avalonia.Controls.Shapes.Rectangle().Width(width).VerticalAlignment(VerticalAlignment.Stretch);
        line.Opacity = opacity;
        line.Fill(BaseView.R(borderBrush));
        if (onCreate != null)
            onCreate(line);
        if (margin == null)
            line.Margin(0, 0, 0, 0);
        else
            line.Margin(margin.Value);
        return line;
    }

    public static Control HLine(double height = 1, double opacity = 0.6, string borderBrush = "SukiBorderBrush", Thickness? margin = null, Action<Control>? onCreate = null)
    {
        var shape = new Avalonia.Controls.Shapes.Rectangle().Height(height).Opacity(opacity).Fill(BaseView.R(borderBrush)).HorizontalAlignment(HorizontalAlignment.Stretch);
        if (onCreate != null)
            onCreate(shape);
        if (margin == null)
            shape.Margin(0, 0, 0, 0);
        else
            shape.Margin(margin.Value);
        return shape;
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

    public static GroupBox GroupBox(string header, Control? content = null, int? hAlign = null)
    {
        var gb = new GroupBox() { Header = header }.Align(hAlign).BorderThickness(1);
        if (content != null) gb.Content = content;
        return gb;
    }

    public static Grid SettingRow(StreamGeometry? icon, string text, Control content, bool canClick = false, Action<Grid>? onClick = null)
    {
        if(canClick == false) return HGrid("Auto,Auto, *",
            [
                icon == null ? null : new PathIcon().Data(icon).Size(16).Margin(10,0,10,0),
                TextBlock(text).Align(-1,0),
                content.Align(1,0)
            ]);
        else
        {
            var grid = HGrid("Auto,Auto, *",
            [
                icon == null ? null : new PathIcon().Data(icon).Size(16).Margin(10, 0, 10, 0).IsHitTestVisible(false),
                TextBlock(text).Align(-1, 0).IsHitTestVisible(false),
                content.Align(1, 0).IsHitTestVisible(false)
            ]).Background(Brushes.Transparent);

            if (onClick != null) grid.WhenClick(onClick);
            return grid;
        }
    }

    public static Border SettingVGrid(Control?[] contents, string borderBrush = "SukiBorderBrush")
    {
        Control CreateLine()
        {
            return HLine(1, 1, borderBrush).Margin(0);
        }

        var controls = new List<Control>(contents.Length * 2);
        var sb = new StringBuilder();
        foreach (var item in contents)
        {
            if (item != null)
            {
                if (sb.Length > 0)
                {
                    sb.Append(",1,");
                    controls.Add(CreateLine());
                }
                sb.Append("*");
                controls.Add(item.Margin(6));
            }
        }

        return Border(VGrid(sb.ToString(), controls.ToArray()), thickness: 1)
            .BorderBrush(R(borderBrush));
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

    public static DynamicResourceExtension R_LowTextColor
    {
        get => new DynamicResourceExtension("SukiLowText");
    }

    public static DynamicResourceExtension R_DisabledTextColor
    {
        get => new DynamicResourceExtension("SukiDisabledText");
    }

    public static DynamicResourceExtension R_LightBorderBrush
    {
        get => new DynamicResourceExtension("SukiLightBorderBrush");
    }

    public static DynamicResourceExtension R_SukiBackground
    {
        get => new DynamicResourceExtension("SukiBackground");
    }

    public static DynamicResourceExtension R_SukiStrongBackground
    {
        get => new DynamicResourceExtension("SukiStrongBackground");
    }

    public static DynamicResourceExtension R_SukiCardBackground
    {
        get => new DynamicResourceExtension("SukiCardBackground");
    }

    public static DynamicResourceExtension R_SukiLightBackground
    {
        get => new DynamicResourceExtension("SukiLightBackground");
    }

    public static DynamicResourceExtension R_SukiPopupBackground
    {
        get => new DynamicResourceExtension("SukiPopupBackground");
    }

    public static DynamicResourceExtension R_SukiGlassCardBackground
    {
        get => new DynamicResourceExtension("SukiGlassCardBackground");
    }

    public static DynamicResourceExtension R_SukiGlassCardOpaqueBackground
    {
        get => new DynamicResourceExtension("SukiGlassCardOpaqueBackground");
    }

    public static DynamicResourceExtension R_SukiControlTouchBackground
    {
        get => new DynamicResourceExtension("SukiControlTouchBackground");
    }

    public static DynamicResourceExtension R_SukiDialogBackground
    {
        get => new DynamicResourceExtension("SukiDialogBackground");
    }

    public static DynamicResourceExtension R_SukiBorderBrush
    {
        get => new DynamicResourceExtension("SukiBorderBrush");
    }
    public static DynamicResourceExtension R_SukiControlBorderBrush
    {
        get => new DynamicResourceExtension("SukiControlBorderBrush");
    }
    public static DynamicResourceExtension R_SukiMediumBorderBrush
    {
        get => new DynamicResourceExtension("SukiMediumBorderBrush");
    }
    public static DynamicResourceExtension R_SukiLightBorderBrush
    {
        get => new DynamicResourceExtension("SukiLightBorderBrush");
    }
    public static DynamicResourceExtension R_SukiMenuBorderBrush
    {
        get => new DynamicResourceExtension("SukiMenuBorderBrush");
    }
    public static DynamicResourceExtension R_GlassBorderBrush
    {
        get => new DynamicResourceExtension("GlassBorderBrush");
    }

    #endregion

    public static void OpenUrl(string url)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // 必须为 true 才能用默认浏览器打开
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            // 处理异常
        }
    }

    public static void CopyToClipboard(string text, Action? onComplete = null)
    {
        TopLevel? topLevel = null;
        Dispatcher.UIThread.Post(
            async () => {
                topLevel ??= TopLevel.GetTopLevel(Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
        ? desktop.MainWindow
        : null);

                if (topLevel != null)
                {
                    var clipboard = topLevel.Clipboard;
                    if (clipboard != null)
                    {
                        await clipboard.SetTextAsync(text);
                        onComplete?.Invoke();
                    }
                }
            }
        );
    }

    public int GetDesktopWindowBarHeight()
    {
        var topLevel = GetTopLevel();
        if (topLevel is Window window)
        {
            return 44;
        }
        return 0;
    }

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

    public void ShowLoading(Control? centerOfContainer = null, Action<Loading>? onCreate = null)
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return;

        this.IsEnabled = false;
        var loading = new Loading().Align(0, 0);

        if (centerOfContainer != null)
        {
            // 获取 OverlayHosts 的屏幕位置
            Visual overlay = (this.GetVisualRoot() as Visual) ?? centerOfContainer;
            var overlaySize = overlay.Bounds;

            // 获取 centerOfContainer 的屏幕位置
            var centerOrigin = centerOfContainer.PointToScreen(new Point(0, 0));
            var overlayOrigin = overlay.PointToScreen(new Point(0, 0));

            // 获取 centerOfContainer 的中心点
            var centerSize = centerOfContainer.Bounds;
            var centerPoint = new Point(
                centerOrigin.X + centerSize.Width / 2,
                centerOrigin.Y + centerSize.Height / 2
            );

            var titleBarHeight = GetDesktopWindowBarHeight();

            var overlayCenterPoint = new Point(
                overlayOrigin.X + overlaySize.Width / 2,
                overlayOrigin.Y + overlaySize.Height / 2 + titleBarHeight/2
            );

            // 计算相对于 OverlayHosts 的偏移
            var offsetX = centerPoint.X - overlayCenterPoint.X;
            var offsetY = centerPoint.Y - overlayCenterPoint.Y;

            // 假设 Loading 控件有默认宽高（如 80x80），可根据实际情况获取
            double loadingWidth = loading.Width > 0 ? loading.Width : 50;
            double loadingHeight = loading.Height > 0 ? loading.Height : 50;

            // 设置 Margin，使 Loading 居中于 centerOfContainer
            loading.Margin = new Thickness(
                0,
                0,
                - offsetX * 2,
                - offsetY * 2
            );
        }

        onCreate?.Invoke(loading);
        hosts.Add(loading);
    }

    private InnerModalView? FindModalBackgroundControl(Controls hosts)
    {
        foreach(var ctrl in hosts)
        {
            if(ctrl is InnerModalView r)
            {
                return r;
            }
        }
        return null;    
    }

    public bool AddOverlay(Control control)
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return false;
        hosts.Add(control);
        return false;
    }

    public bool ClearOverlays()
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return false;
        hosts.Clear();
        return false;
    }

    public bool RemoveOverlay(Control control)
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return false;

        Control? match = null;
        foreach (var ctrl in hosts)
        {
            if (ctrl == control)
            {
                match = ctrl; break;
            }
        }

        if (match != null) hosts.Remove(match);

        return match != null;
    }

    public bool RemoveOverlayFirst(Func<Control,bool> pred)
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return false;

        Control? match = null;
        foreach (var ctrl in hosts)
        {
            if (pred(ctrl))
            {
                match = ctrl; break;
            }
        }

        if (match != null) hosts.Remove(match);
        
        return match != null;
    }

    public bool RemoveOverlays(Func<Control, bool>? pred = null)
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return false;

        if(pred == null)
        {
            hosts.Clear();
            return true;
        }

        var list = new List<Control>();
        foreach (var ctrl in hosts)
        {
            if (pred(ctrl))
            {
                list.Add(ctrl);
            }
        }

        foreach(var ctrl in list)
        {
            hosts.Remove(ctrl);
        }

        return list.Count > 0;
    }

    public bool RemoveOverlayAt(int index)
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return false;
        hosts.RemoveAt(index);
        return true;
    }

    public bool ShowInOverlay(BaseView owner, bool modal = false, double modalBgOpacity = 0.7)
    {
        var hosts = owner.OverlayHosts();
        if (hosts == null) return false;

        if(modal == true)
        {
            InnerModalView? bg = FindModalBackgroundControl(hosts);
            if(bg == null)
            {
                var border = new InnerModalView();
                border.Opacity = modalBgOpacity;
                hosts.Add(border);
            }
        }

        hosts.Add(this);
        return false;
    }

    public bool RemoveFromOverlay()
    {
        var hosts = this.OverlayHosts();
        if (hosts == null) return false;
        InnerModalView? bg = FindModalBackgroundControl(hosts);
        if(bg != null) hosts.Remove(bg);
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

    public static T TextStyle<T>(this T ctrl, float? fontSize = null) where T: Control
    {
        if(ctrl is TemplatedControl tCtrl)
            tCtrl.Foreground(BaseView.R("SukiText"));
        else if (ctrl is TextBlock tbCtrl)
        {
            tbCtrl.Foreground(BaseView.R("SukiText"));
            if (fontSize != null)
                tbCtrl.FontSize = fontSize.Value;
        }
        return ctrl;
    }

    public static T LowTextStyle<T>(this T ctrl, float? fontSize = null) where T : Control
    {
        if (ctrl is TemplatedControl tCtrl)
            tCtrl.Foreground(BaseView.R("SukiLowText"));
        else if (ctrl is TextBlock tbCtrl)
        {
            tbCtrl.Foreground(BaseView.R("SukiLowText"));
            if (fontSize != null)
                tbCtrl.FontSize = fontSize.Value;
        }
        return ctrl;
    }

    public static T DisabledTextStyle<T>(this T ctrl, float? fontSize = null) where T : Control
    {
        if (ctrl is TemplatedControl tCtrl)
            tCtrl.Foreground(BaseView.R("SukiDisabledText"));
        else if (ctrl is TextBlock tbCtrl)
        {
            tbCtrl.Foreground(BaseView.R("SukiDisabledText"));
            if(fontSize != null)
                tbCtrl.FontSize = fontSize.Value;
        }
        return ctrl;
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
                    owner.ShowLoading(owner, onCreate);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Return<TElement>(this TElement control, out TElement field)
    {
        field = control;
    }

    public static Control[] ConcatWith(this Control[] first, params Control[] second)
    {
        if (first == null || first.Length == 0)
            return second ?? Array.Empty<Control>();
        if (second == null || second.Length == 0)
            return first ?? Array.Empty<Control>();

        var result = new Control[first.Length + second.Length];
        Array.Copy(first, 0, result, 0, first.Length);
        Array.Copy(second, 0, result, first.Length, second.Length);
        return result;
    }

    #region 简化动作回调

    public static T WhenLoaded<T>(this T ctrl, Action<T> action) where T : Control
    {
        ctrl.OnLoaded((Avalonia.Interactivity.RoutedEventArgs _) => action(ctrl));
        return ctrl;
    }

    public static T WhenUnloaded<T>(this T ctrl, Action<T> action) where T : Control
    {
        ctrl.OnUnloaded((Avalonia.Interactivity.RoutedEventArgs _) => action(ctrl));
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

    public static T WhenIsCheckedChanged<T>(this T ctrl, Action<T> action) where T : ToggleSwitch
    {
        ctrl.OnIsCheckedChanged(_ => action(ctrl));
        return ctrl;
    }

    #endregion


    #region 动画

    /// <summary>
    /// 设置平移变换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ctrl"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static T Translate<T>(this T ctrl, double x, double y) where T : Visual
    {
        if (ctrl.RenderTransform is TransformGroup group)
        {
            var translate = new TranslateTransform(x, y);
            group.Children.Add(translate);
        }
        else if (ctrl.RenderTransform == null)
        {
            // 没有变换或是默认的
            ctrl.RenderTransform = new TranslateTransform(x, y);
        }
        else
        {
            var newGroup = new TransformGroup();
            var oldTransform = ctrl.RenderTransform as Transform;
            if(oldTransform != null)
                newGroup.Children.Add(oldTransform);
            var translate = new TranslateTransform(x, y);
            newGroup.Children.Add(translate);
            ctrl.RenderTransform = newGroup;
        }
        return ctrl;
    }

    public static T Opacity<T>(this T ctrl, double duration, double from, double to, Action? onComplete = null) where T : Visual
    {
        ctrl.Animate<double>(Visual.OpacityProperty, from, to, TimeSpan.FromSeconds(duration), onComplete: onComplete);
        return ctrl;
    }

    public static T Transform<T>(this T ctrl, double duration, Transform from, Transform to, Action? onComplete = null) where T : Visual
    {
        ctrl.Animate<Transform>(Visual.RenderTransformProperty, from, to, TimeSpan.FromSeconds(duration), onComplete: onComplete);
        return ctrl;
    }

    public static T Move<T>(this T ctrl, double duration, double fromX, double fromY, double toX, double toY, Action? onComplete = null) where T : Visual
    {
        if(fromX != toX)
            ctrl.Animate<double>(TranslateTransform.XProperty, fromX, toX, TimeSpan.FromSeconds(duration),onComplete:onComplete);
    
        if(fromY != toY)
            ctrl.Animate<double>(TranslateTransform.YProperty, fromY, toY, TimeSpan.FromSeconds(duration),onComplete:onComplete);
        return ctrl;
    }

    public static T Rotate<T>(this T ctrl, double duration, double fromAngle, double toAngle, Action? onComplete = null) where T : Visual
    {
        ctrl.Animate<double>(RotateTransform.AngleProperty, fromAngle, toAngle, TimeSpan.FromSeconds(duration), onComplete: onComplete);
        return ctrl;
    }

    public static T Scale<T>(this T ctrl, double duration, double fromScale, double toScale, Action? onComplete = null) where T : Visual
    {
        return ctrl.Scale(duration, fromScale, fromScale, toScale, toScale, onComplete: onComplete);
    }

    public static T Scale<T>(this T ctrl, double duration, double fromScaleX, double fromScaleY, double toScaleX, double toScaleY, Action? onComplete = null) where T : Visual
    {
        if (fromScaleX != toScaleX)
            ctrl.Animate<double>(ScaleTransform.ScaleXProperty, fromScaleX, toScaleX, TimeSpan.FromSeconds(duration), onComplete: onComplete);

        if (fromScaleY != toScaleY)
            ctrl.Animate<double>(ScaleTransform.ScaleYProperty, fromScaleY, toScaleY, TimeSpan.FromSeconds(duration), onComplete: onComplete);
        return ctrl;
    }

    #endregion
}