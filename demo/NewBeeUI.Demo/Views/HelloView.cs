
namespace NewBeeUI.Demo.Views;

public class HelloView : BaseView
{
    int count = 0;

    protected override void Build(out Control content)
    {
        VStack([
                TextBlock(() => $"Click {count} times").Align(0),
                TextButton("Hello").WhenClick(_=>{
                    count++;
                    this.UpdateState();
                })
        ]).Margin(20).Ref(out content);
    }
}
