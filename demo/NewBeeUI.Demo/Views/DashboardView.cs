using SkiaSharp;
using System.Diagnostics.Metrics;

namespace NewBeeUI.Demo.Views;

public class DashboardView : BaseView
{
    int count = 0;
    protected override object Build()
    {
        var stack =  
            VStack([
                GroupBox("Platform Info",
                    VStack([
                        BuildRow("Runtime", GetRuntime()),
                        BuildRow("Mode", App.IsMobileApp ? "Mobile" : "Desktop"),
                        BuildRow("SkiaSharp", SkiaSharp.SkiaSharpVersion.Native.ToString()),
                    ])
                ),
                GroupBox("Buttons",
                       VStack([
                        TextBlock().Align(-1).Text(() => $"Click {count} times"),
                        TextButton("Click Me").Align(-1).WhenClick(_=>{
                                count++;
                                this.UpdateState();
                            }),
                        new ToggleSwitch().Align(0,0),
                        IconButton(SearchIcon.Instance),
                        IconButton(Icons.ScaleToOriginal),
                        IconButton("InnerText", SearchIcon.Instance),
                        ])
                ),
                GroupBox("TextBox",
                     VStack([
                        new TextBox(),
                        new TextBox().Watermark("请输入内容").ListenIME(),
                        ])
                ),
                GroupBox("Drawing",VStack([
                    new DrawingRect(){ Width = 200, Height = 200}.Align(0,0)
                    ])),
                GroupBox("3D",VStack([
                    new Glb3DView(){ Width = 300, Height = 300}.Align(0,0)
                    ])),
            ]).Spacing(32);

        return stack;
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
