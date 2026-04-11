namespace NewBeeUI.Demo.Views;

public class ComponentsView : BaseView
{
    public ViewRouter? Router { get; set; }

    protected override void Build(out Control content)
    {
        VStack([
            GroupBox("文本输入",
                VStack([
                    new TextBox().PlaceholderText("请输入内容").ListenIME(),
                    new TextBox().PasswordChar('*').PlaceholderText("请输入密码"),
                    new TextBox().Height(100).TextWrapping(TextWrapping.Wrap)
                        .AcceptsReturn(true).VerticalContentAlignment(VerticalAlignment.Top).Align(null,0),
                ])
            ),
            GroupBox("按钮",
                VStack([
                    HStack([
                        new ToggleSwitch().Align(0,0),
                        IconButton(Icons.ScaleToOriginal),
                        IconButton(NStyles.MeterialIcons.SearchWebIcon.Instance),
                        IconButton(SearchIcon.Instance, "ToolTip", ToolTipPosition.Top ),
                        SelectableIconButton(CogBoxIcon.Instance).OnClick(v=>{ v.Selected = !v.Selected; v.UpdateState(); }),
                    ]),
                    HStack([
                        IconButton("InnerText", SearchIcon.Instance),
                        IconButton("InnerText", SearchIcon.Instance, iconSize:12),
                    ]),
                    HStack([
                        ToggleIconButton(LinkVariantIcon.Instance, LinkVariantOffIcon.Instance, "Semantic search is on. Click to switch to keyword search", "Keyword search is on. Click to switch to semantic search")
                    ]),
                    ])
            ),
            GroupBox("Styles",
                VStack([
                    HStack([
                            TextButton("Flat").FlatStyle(),
                            TextButton("Success").SuccessStyle(),
                            TextButton("Danger").DangerStyle(),
                        ]),
                    HStack([
                            TextButton("Click").FlatStyle().CornerRadius(24),
                            TextButton("Basic").BasicStyle(),
                            TextButton("Accent").AccentStyle(),
                        ]),
                ])
            ),
            GroupBox("动画与3D",
                HStack([
                    TextButton("动画演示").OnClick(_=>{
                        var v = new AnimateView();
                        v.ShowInOverlay(this,true);
                    }).Align(-1,0),
                    TextButton("3D演示").OnClick(_=>{
                        var v = new ThreeDView();
                        v.ShowInOverlay(this,true);
                    }).Align(-1,0)
                ])
            ),
            GroupBox("数据绑定",
                HStack([
                    TextButton("数据绑定演示").OnClick(_=>{
                        var v = new CounterBindingView();
                        v.ShowInOverlay(this,true);
                    }).Align(-1,0),
                    TextButton("后台绑定演示").OnClick(_=>{
                        var v = new BackgroundCounterBindingView();
                        v.ShowInOverlay(this,true);
                    }).Align(-1,0),
                ])
            ),
            GroupBox("生成图像和视频",
                HStack([
                    TextButton("生成演示").OnClick(_=>{
                        var v = new GenVideoView();
                        v.ShowInOverlay(this,true);
                    }).Align(-1,0),
                ])
            ),
            DemoViewCodeView(),
        ]).Return(out content);
    }

    protected void MockLoading()
    {
        this.RunWithDelayedLoading(() =>
        {
            Thread.Sleep(3000);
        }, runAtBackground: true, onCreate: l => { l.Margin(App.IsMobileLayout ? 0 : 200, 0, 0, 0); });
    }
}
