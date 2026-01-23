namespace NewBeeUI.Demo.Views.Components;

public class DemoColorView : BaseView
{
    public IBrush? Color { get; set; }

    protected override void Build(out Control content)
    {
        VStack([
                Border()
                .Background(Color?? Brushes.Transparent)
                .Size(36),
                TextBlock(Name??String.Empty)
            ])
            .Return(out content);
    }
}
