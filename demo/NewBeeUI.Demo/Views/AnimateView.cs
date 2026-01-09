
namespace NewBeeUI.Demo.Views;

public class AnimateView : BaseView
{
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
             TextBlock("Your Content"),
        ])
        .Align(0, 0)
        .Opacity(0.5)
        .WhenLoaded(x => {
            x.Opacity(0.5, 0, 1);
            x.Move(0.5, 0, 0, 100, 100);
            x.Rotate(0.5, 0, 90);
        }).Return(out content);
    }
}
