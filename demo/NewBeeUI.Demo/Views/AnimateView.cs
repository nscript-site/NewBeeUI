namespace NewBeeUI.Demo.Views;

public class AnimateView : BaseView
{
    protected override object Build()
    {
        return VStack([
                TextBlock("Your Content"),
            ])
            .Align(0, 0)
            .Opacity(0.5)
            .WhenLoaded(x => {
                x.Opacity(0.5, 0, 1);
                x.Move(0.5, 0, 0, 100, 100);
                x.Rotate(0.5, 0, 90);
            });
    }
}
