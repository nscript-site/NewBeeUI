# NewBeeUI

NewBeeUI(NB UI，又叫菜鸟UI) 是基于 Avalonia 的 mvu gui 库，它专门针对不会/不喜欢 axaml 的程序员开发，是一个入门非常简单的跨平台 gui 库。

设计原则：

- 支持 MVU 模式
- 支持 NativeAOT
- 支持跨平台
- 尽可能轻量、简单

相关库：

- [nmarkup](https://github.com/nscript-site/Avalonia.Markup.Declarative): fork 自 Avalonia.Markup.Declarative，进行了少量的修改 [![NuGet](https://img.shields.io/nuget/v/nmarkup.svg)](https://www.nuget.org/packages/nmarkup)

- nstyles: 一套可在 NativeAOT 下编译的主题，改自 sukiui。[![NuGet](https://img.shields.io/nuget/v/nstyles.svg)](https://www.nuget.org/packages/nstyles)

- NStyles.MeterialIcons: 一套可在 NativeAOT 下编译的图标库 [![NuGet](https://img.shields.io/nuget/v/NStyles.MeterialIcons.svg)](https://www.nuget.org/packages/NStyles.MeterialIcons)

## MVU 开发模式

### 基础机制

```
public class HelloView : BaseView
{
    int count = 0;

    protected override object Build()
    {
        return VStack([
                TextBlock().Align(0).Text(() => $"Click {count} times"),
                TextButton("Hello").OnClick(_=>{
                    count++;
                    this.UpdateState();
                })
            ]).Margin(20);
    }
}

```

NewBeeUI 只约定了状态的更新动作，状态更新内容由数据绑定来触发。通过扩展方法注册 labmda 表达式的属性(上例中 .Text(() => $"Click {count} times"))，会自动添加绑定，当调用 `UpdateState()` 时，会重新计算表达式的值，并更新 UI。

### 集合控件

通过 ItemTemplate 扩展方法，可以很方便的创建集合式控件，示例如下：

```csharp
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
```

### 观察机制

默认情况下，UpdateState 只更新当前 view 的状态。可通过 Observe 方法，实现不同 view 之间的级联状态更新。当 A Observe B 时，如果 B 调用了 UpdateState 方法，也会触发 A 调用 UpdateState 放大。

循环绑定也没关系，内部会自动处理，保障 UpdateState 触发时，调用链中的相关 view 只调用一次 UpdateState 方法。

### 状态初始化及更新

NewBeeUI 系统对什么是状态，并没有约定。你可以在 build 发生之前，通过合适的方式，进行初始化动作。

也可以利用 `SetState` 扩展方法，来进行状态的初始化（或更新），方法原型：

```csharp
public static TViewModel SetState<TViewModel>(this TViewModel targetView, Action<TViewModel> action, bool setOnce = true) where TViewModel : MvuView
```

如果通过 SetState 设置了 action，则在 build view 之前，会调用该 action。如果 setOnce = false，则，在每次 UpdateState 调用时，都会调用该 action（如果状态依赖于外部环境，这样做，可以在每次更新之前，同步数据，确保状态保持最新）。

### 一些简化写法

通过  Align 扩展方法，简化对 HorizontalAlignment 和 VerticalAlignment 的设置，自动将 int? 转换为 HorizontalAlignment 或 VerticalAlignment。对该值的约定如下：

- <0: 近 (Left/Top)
- 0: 居中 (Center)
- \>0: 远 (Right/Bottom)
- null: Stretch

## 路由机制

通过 `ViewRouter` 提供了路由机制。支持两种路由机制：

- 跳转到 IRoutedViewBuilder 构建的界面
    - 通过 `Goto(IRoutedViewBuilder locator, bool remember = false)` 直接跳转到新构建的界面
- 先通过注册路径，再跳转跳转，如果路径不存在，则显示错误界面
    - 注册：通过 `AddRoute` 扩展方法将 IRoutedViewBuilder 注册到指定的路径
    - 跳转: 通过 `Goto(string route, bool remember = false)` 来跳转

提供了 `RoutedViewBuilder` 实现接口 `IRoutedViewBuilder`，可直接传入要显示的 view，或产生 view 的方法，如果是后者，则，在路由器跳转时，再创造待显示的 view。 

如果跳转时，remember 为 true，则会将路由记录到历史中，用户可以通过 `GoBack()` 方法返回。

`ViewRouter` 提供了下面事件，响应路由的改变：

```csharp
    public Action<ViewRouteUpdateEvent>? OnRouteUpdate { get; set; }
    public Action<RoutedView>? OnViewLeave { get; set; }
```

## 桌面端应用示例

在 avalonia 项目中加入 nstyles 主题:

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="NewBeeUI.Demo.App"
             xmlns:ns ="nstyles"
             RequestedThemeVariant="Default">

    <Application.Styles>
        <FluentTheme />
        <ns:NTheme />
    </Application.Styles>
</Application>
```

启动时，指定相关的 View：

```csharp
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using NewBeeUI.Demo.Views;

namespace NewBeeUI.Demo;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            new MainView().ShowDialog();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}

```

下面是带有完整的菜单及路由跳转的窗口示例：

```csharp
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
            new RoutedViewBuilder("按钮", () => new ButtonsView()),
            new RoutedViewBuilder("Windows", () => new WindowsView()),
            new RoutedViewBuilder("Test", () => new TestView()),
            new RoutedViewBuilder("Overlay", new OverlayView())
        ];
    }
}
```