namespace NewBeeUI.Demo.Views;

public class CounterModel
{
    public int Count { get; set; }
}

public class InnerBindingView : BaseView
{
    public CounterModel? Model { get; set; }

    protected override void Build(out Control content)
    {
        HStack([TextBlock(() => $"[{Name}] Count: {Model?.Count}")])
            .Return(out content);
    }
}

public class CounterBindingView : BaseView
{
    protected CounterModel Model = new CounterModel();

    protected override void Build(out Control content)
    {
        BuildContent(out Control body);

        VGrid("42,1,*,Auto", [
            HGrid("40,*,40",[
                IconButton(ArrowLeftIcon.Instance).OnClick(_ => {
                this.RemoveFromOverlay();
                }),
                TextBlock(this.Name).Align(0,0)
            ]),
            HLine(1,1).Margin(0),
            body,
            DemoViewCodeView(),
        ]).Background(R("SukiStrongBackground")).Return(out content);
    }

    protected void BuildContent(out Control content)
    {
        VStack([
            TextBlock($"[未绑定] Count: {Model.Count}"),
            TextBlock(()=>$"[绑定] Count: {Model.Count}"),
            new InnerBindingView(){ Model = Model, Name = "未级联绑定" },
            new InnerBindingView(){ Model = Model, Name = "级联绑定" }
                .Observe(this),  // 当 view 更新时，会触发 CounterBindingView 的更新
            TextButton("增加").OnClick(_=> { Model.Count ++; this.UpdateState(); }),
        ])
        .Margin(20)
        .Return(out content);
    }
}
