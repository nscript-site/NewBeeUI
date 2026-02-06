namespace NewBeeUI.Demo.Views;

public class BackgroundCounterModel
{
    public static BackgroundCounterModel Instance { get; } = new BackgroundCounterModel();

    public int Count
    {   
        get;
        set {
            if (field == value) return;
            field = value;
            CountUpdate?.Invoke();
        }
    }

    public Action? CountUpdate { get; set; }
}

public static class BackgroundCounterModel_Binding_Extentions
{
    internal static void BindCountUpdate(this BaseView view)
    {
        void OnUpdate()
        {
            view.UpdateStateByUIThread();
        }

        view.OnLoaded(_ => { BackgroundCounterModel.Instance.CountUpdate += OnUpdate; });
        view.OnUnloaded(_ => { BackgroundCounterModel.Instance.CountUpdate -= OnUpdate; });
    }
}

public class BackgroundCounterBindingView : BaseView
{
    protected BackgroundCounterModel Model = new BackgroundCounterModel();

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

        this.BindCountUpdate();
    }

    protected void BuildContent(out Control content)
    {
        VStack([
            TextBlock($"[未绑定] Count: {BackgroundCounterModel.Instance.Count}"),
            TextBlock(()=>$"[绑定] Count: {BackgroundCounterModel.Instance.Count}"),
            TextButton("增加").OnClick(_=> { BackgroundCounterModel.Instance.Count ++;}),
        ])
        .Margin(20)
        .Return(out content);
    }
}
