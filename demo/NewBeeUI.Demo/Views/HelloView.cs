
namespace NewBeeUI.Demo.Views;

public class HelloView : BaseView
{
    int count = 0;

    protected override void Build(out Control content)
    {
        VStack([
                TextBlock().Align(0).Text(() => $"Click {count} times"),
                TextButton("Hello").WhenClick(_=>{
                    count++;
                    this.UpdateState();
                })
        ]).Margin(20).Ref(out content);
    }
}
