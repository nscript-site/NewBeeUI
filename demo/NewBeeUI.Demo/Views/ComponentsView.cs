namespace NewBeeUI.Demo.Views;

public class ComponentsView : BaseView
{
    public ViewRouter? Router { get; set; }

    protected override void Build(out Control content)
    {
        VStack([
            VStack([TextButton("返回").OnClick(_=>{ Router?.GoBack(); }),
                new ToggleSwitch().Align(0,0),
                IconButton(SearchIcon.Instance),
                IconButton(Icons.ScaleToOriginal),
                IconButton("InnerText", SearchIcon.Instance),
                IconButton("InnerText", SearchIcon.Instance, iconSize:12),
                IconButton(NStyles.MeterialIcons.SearchWebIcon.Instance),
                TextButton("显示加载").WhenClick(_ => MockLoading()),
            ]).Align(0,0),
            HStack([
                IconButton(SearchIcon.Instance, "ToolTip", ToolTipPosition.Top ),
                SelectableIconButton(SearchIcon.Instance, "ToolTip", "ToolTip2", ToolTipPosition.Top),
                SelectableIconButton(CogBoxIcon.Instance).OnClick(v=>{ v.Selected = !v.Selected; v.UpdateState(); }),
            ]).Align(0,0),
            GroupBox("动画",
                VStack([
                    TextButton("动画演示").OnClick(_=>{
                        var v = new AnimateView();
                        v.ShowInOverlay(this,true);
                    })
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
