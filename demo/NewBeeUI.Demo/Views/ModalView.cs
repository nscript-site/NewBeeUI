
namespace NewBeeUI.Demo.Views;

public class ModalView: BaseView
{
    protected override void Build(out Control content)
    {
        BuildContent(out Control body);

        VGrid("42,1, *", [
            HGrid("40,*,40",[
                IconButton(ArrowLeftIcon.Instance).OnClick(_ => {
                    // Close with animation
                    this.Move(0.5, 0,0, this.WindowsSize().Width,0,onComplete:()=>{
                        InvokeByUIThread( ()=>{
                            this.RemoveFromOverlay();
                        });
                    });
                }),
                TextBlock(this.Name).Align(0,0)
                ]),
            HLine(1,1).Margin(0),
            body,
        ])
        .Background(R("SukiStrongBackground"))
        .Return(out content);

        SetAnimation(content);
    }

    // Set slide-in animation from right to left
    protected void SetAnimation(Control control)
    {
        var size = this.WindowsSize();
        control.Translate(size.Width, 0)
        .WhenLoaded(x =>
        {
            x.Move(0.5, size.Width, 0, 0, 0);
        });
    }

    protected void BuildContent(out Control content)
    {
        VStack([
             TextBlock("Your Content"),
             DemoViewCodeView(),
        ])
        .Align(0, 0)
        .Return(out content);
    }
}
