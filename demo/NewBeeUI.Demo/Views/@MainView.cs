using NStyles.MeterialIcons;

namespace NewBeeUI.Demo.Views;

public class MainView : BaseView, IWindowView
{
    ViewRouter? Router;

    public WindowInfo WindowInfo { get; }

    protected WindowInfo CreateWindowInfo()
    {
        var window = new NWindowInfo()
        {
            WindowTitle = "标题",
            CanResize = true,
            CanMinimize = true,
            CanClose = true,
            WindowMinWidth = 800,
            WindowMinHeight = 600,
            WindowWidth = 800,
            WindowHeight = 600,
            IsWindowAnimationEnable = true,
            Subtitle = this.BuildSubtitle(),
            RightWindowsBar = new TextBlock()
                .Text("这里可以放置你的按钮")
                .FontSize(14)
                .Margin(10, 0)
                .HorizontalAlignment(HorizontalAlignment.Right)
                .VerticalAlignment(VerticalAlignment.Center)
        };
        window.RightWindowsBar = HStack([
            IconButton(MessageSettingsOutlineIcon.Instance, "设置", ToolTipPosition.Top).Width(24).Height(24).OnClick(_=>{
                new SettingView().ShowDialog("设置");
            }),
            ]);
        return window;
    }

    protected Control BuildSubtitle()
    {
        return new TextBlock().Ref(out SubtitleTextBlock)!
            .FontSize(14)
            .Margin(10, 0)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center);
    }

    TextBlock? SubtitleTextBlock;

    public MainView():base()
    {
        this.WindowInfo = CreateWindowInfo();
    }

    protected override object Build()
    {
        var router = BuildViewRouter().Ref(out Router)!;

        return Grid(cols: "Auto, *")
                .Children([
                    BuildMenu(),
                    router.Col(1)
                ]);
    }

    protected ViewRouter BuildViewRouter()
    {
        var r = new ViewRouter().Align(null, null);
        r.OnRouteUpdate = (e) =>
        {
            if (SubtitleTextBlock != null)
            {
                SubtitleTextBlock.Text = $"{e.New?.Name ?? "No Title"}";
            }
        };
        return r;
    }

    protected Control BuildMenu()
    {
        Control BuildMenuItem(RoutedViewBuilder builder)
        {
            if(builder.IsEmpty())
            {
                return Border().Width(100).Height(1).Align(null,0).Margin(10,0)
                    .Background(Brushes.Gray).IsHitTestVisible(false);
            }
            else
            {
                return Grid(cols: "40,100")
                        .Children(
                        [
                            Border(builder.Icon?.Align(0,0)),
                            new TextBlock().Text(builder.Name).Col(1)
                        ]);
            }
        }

        var listBox = new ListBox()
            .HorizontalAlignment(HorizontalAlignment.Center)
            .ItemsSource(() => GetMenuItems())
            .ItemTemplate<RoutedViewBuilder, ListBox>(BuildMenuItem)
            .OnSelectionChanged((e) =>
            {
                if (e.FirstItem() is RoutedViewBuilder builder)
                {
                    if(builder.IsEmpty() == false)
                        Router?.Goto(builder);    // 跳转
                }
            });

        return listBox;
    }

    public List<RoutedViewBuilder> GetMenuItems()
    {
        return
        [
            new RoutedViewBuilder("Dashboard", () => new DashboardView())
                .Icon(ViewDashboardOutlineIcon.Instance),
            new RoutedViewBuilder("Hello", () => new HelloView()),
            new RoutedViewBuilder("按钮", () => new ButtonsView()),
            new RoutedViewBuilder("Windows", () => new WindowsView()),
            new RoutedViewBuilder("Test", () => new TestView()),
            new RoutedViewBuilder("Overlay", new OverlayView())
        ];
    }
}


