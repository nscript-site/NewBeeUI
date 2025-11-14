using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class MobileMainView : BaseView, IWindowView
{
    ViewRouter? Router;

    TextBlock? SubtitleTextBlock;

    #region IWindowView

    public WindowInfo WindowInfo { get; }

    public class DockRightDisableIcon
    {
        private static readonly Lazy<DockRightDisableIcon> _instance = new Lazy<DockRightDisableIcon>(() => new DockRightDisableIcon());

        private StreamGeometry g;

        public static StreamGeometry Instance => _instance.Value.g;

        private DockRightDisableIcon()
        {
            g = StreamGeometry.Parse("M20 4H4A2 2 0 0 0 2 6V18A2 2 0 0 0 4 20H20A2 2 0 0 0 22 18V6A2 2 0 0 0 20 4M15 18H4V6H15ZM20 18H17V6H20Z");
        }
    }

    protected bool IsDockRightEnabled = true;

    protected WindowInfo CreateWindowInfo()
    {
        Button? dockIcon = null;
        Button? dockIconCollapse = null;

        return new NWindowInfo()
        {
            WindowTitle = "移动端示例",
            CanResize = true,
            CanMinimize = true,
            CanClose = true,
            WindowMinWidth = 400,
            WindowMinHeight = 900,
            WindowWidth = 400,
            WindowHeight = 900,
            IsWindowAnimationEnable = true,
            Subtitle = BuildSubtitle(),
            RightWindowsBar = HStack([
                this.CreateWindowIcon(CogOutlineIcon.Instance).OnClick(_=>{ new SettingView().ShowDialog("设置"); }),
            ]).Ref(out var rightBar)!,
        };
    }

    protected Control BuildSubtitle()
    {
        var tb = new TextBlock().Ref(out SubtitleTextBlock)!.TextTrimming(TextTrimming.CharacterEllipsis)
                .FontSize(12).Align(null, 0);
        var grid = HGrid("*", [tb]).ClipToBounds(true);
        return grid;
    }

    #endregion

    public MobileMainView() : base()
    {
        this.WindowInfo = CreateWindowInfo();
    }

    #region build

    protected override object Build()
    {
        var router = BuildViewRouter().Margin(20).Ref(out Router)!;

        var grid = Grid(rows: "*, Auto")
            .Children([
                router,
                BuildMenu().Row(1),
            ]);

        if (App.IsMobileApp && OperatingSystem.IsLinux())  //鸿蒙手机
        {
            grid.Margin(0, 48, 0, 0);
        }

        return grid;
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
        Control BuildTabItem(RoutedViewBuilder? builder)
        {
            if (builder == null || builder.IsEmpty())
            {
                return Border().Width(100).Height(1).Align(null, 0).Margin(10, 0)
                    .Background(Brushes.Gray).IsHitTestVisible(false);
            }
            else
            {
                return VGrid("40,100", [Border(builder.Icon?.Align(0, 0)), new TextBlock().Text(builder.Name)]);
            }
        }

        var listBox = new TabControl()
            .HorizontalAlignment(HorizontalAlignment.Center)
            .ItemsSource(() => GetMenuItems())
            .ItemTemplate<RoutedViewBuilder, TabControl>(BuildTabItem)
            .OnSelectionChanged((e) =>
            {
                if (e.FirstItem() is RoutedViewBuilder builder)
                {
                    if (builder.IsEmpty() == false)
                        Router?.Goto(builder);    // 跳转
                }
            });

        return listBox;
    }

    #endregion

    public List<RoutedViewBuilder> GetMenuItems()
    {
        return
        [
            new RoutedViewBuilder("Dashboard", () => new DashboardView()).Icon(ViewDashboardOutlineIcon.Instance),
            new RoutedViewBuilder("Buttons", () => new ButtonsView()),
            new RoutedViewBuilder("Windows", () => new WindowsView()),
            new RoutedViewBuilder("Test", () => new TestView()),
            new RoutedViewBuilder("Overlay", new OverlayView()),
            //new RoutedViewBuilder("Animate", ()=> new AnimateView()),
            //new RoutedViewBuilder("Slider", ()=> new SliderView()),
            //new RoutedViewBuilder("Menu", ()=> new MenuView()),
            //new RoutedViewBuilder("Flyout", ()=>new FlyoutView()),
            //new RoutedViewBuilder("Styles", ()=>new StyleView()),
            //new RoutedViewBuilder("ComboBox", ()=>new ComboBoxView()),
            //new RoutedViewBuilder("TextBox", ()=>new TextBoxView()),
        ];
    }
}