using System.Diagnostics.Metrics;

namespace NewBeeUI.Demo.Views;

public class DashboardView : BaseView
{
    int count = 0;
    protected override object Build()
    {
        return 
            VStack([
                GroupBox("Platform Info",
                    VStack([
                        BuildRow("Runtime", GetRuntime()),
                        BuildRow("Mode", App.IsMobileApp ? "Mobile" : "Desktop"),
                    ])
                ),
                GroupBox("Click Button",
                   VStack([
                    TextBlock().Align(-1).Text(() => $"Click {count} times"),
                    TextButton("Click Me").Align(-1).WhenClick(_=>{
                            count++;
                            this.UpdateState();
                        })
                    ])
                ),
                GroupBox("TextBox",
                     VStack([
                        new TextBox(),
                        new TextBox().Watermark("请输入内容").ListenIME(),
                        ])
                ),
            ]).Spacing(32);
    }

    private Control BuildRow(string title, string value)
    {
        return HStack([
            TextBlock($"{title}:").Width(100),
            TextBlock(value)
        ]).Spacing(10);
    }

    private string GetRuntime()
    {
        if (OperatingSystem.IsAndroid()) return "Android";
        if (OperatingSystem.IsIOS()) return "iOS";
        if (OperatingSystem.IsWasi()) return "WASI";
        if (OperatingSystem.IsWindows()) return "Windows";
        if (OperatingSystem.IsLinux()) return "Linux";
        if (OperatingSystem.IsMacOS()) return "MacOS";
        return "Unknown";
    }
}
