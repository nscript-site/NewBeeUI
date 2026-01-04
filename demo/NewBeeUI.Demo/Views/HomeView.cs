namespace NewBeeUI.Demo.Views;

public class HomeView : BaseView
{
    int count = 0;

    protected override void Build(out Control content)
    {
        VStack([
                GroupBox("平台信息",
                    VStack([
                        BuildRow("运行环境", GetRuntime()),
                        BuildRow("运行模式", App.IsMobileApp ? "Mobile" : "Desktop"),
                        BuildRow("渲染引擎", $"SkiaSharp {SkiaSharp.SkiaSharpVersion.Native.ToString()}"),
                    ])
                ),
                GroupBox("基础示例",
                       HStack([
                        TextButton("Click Me").Align(-1).WhenClick(_=>{
                                count++;
                                this.UpdateState();
                            }),
                        TextBlock(() => $"Click {count} times").Align(-1),
                        ])
                ),
                //GroupBox("Buttons",
                //       VStack([
                //        TextBlock(() => $"Click {count} times").Align(-1),
                //        TextButton("Click Me").Align(-1).WhenClick(_=>{
                //                count++;
                //                this.UpdateState();
                //            }),
                //        new ToggleSwitch().Align(0,0),
                //        IconButton(SearchIcon.Instance),
                //        IconButton(Icons.ScaleToOriginal),
                //        IconButton("InnerText", SearchIcon.Instance),
                //        ])
                //),
                GroupBox("文本输入",
                     VStack([
                        new TextBox(),
                        new TextBox().Watermark("请输入内容").ListenIME(),
                        ])
                ),
                //GroupBox("Drawing",VStack([
                //    new DrawingRect(){ Width = 200, Height = 200}.Align(0,0)
                //    ])),
                //GroupBox("3D",VStack([
                //    new Glb3DView(){ Width = 300, Height = 300}.Align(0,0)
                //    ])),
                DemoViewCodeView(),
            ]).Spacing(32).Return(out content);
    }

    private Control BuildRow(string title, string value)
    {
        return HStack([
            TextBlock($"{title} :").Width(80),
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
