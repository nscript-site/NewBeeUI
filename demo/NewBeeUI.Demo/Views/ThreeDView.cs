namespace NewBeeUI.Demo.Views;

public class ThreeDView : BaseView
{
    protected override void Build(out Control content)
    {
        BuildContent(out Control body);

        VGrid("42,1, *", [
            HGrid("40,*,40",[
                IconButton(ArrowLeftIcon.Instance).OnClick(_ =>this.RemoveFromOverlay()),
                TextBlock(this.Name).Align(0,0)
                ]),
            HLine(1,1).Margin(0),
            body,
        ])
        .Background(R("SukiStrongBackground"))
        .Return(out content);
    }

    protected void BuildContent(out Control content)
    {
        VStack([
            new Glb3DView(),
            DemoViewCodeView(),
        ]).Margin(20)
        .Align(0, -1)
        .Return(out content);
    }
}