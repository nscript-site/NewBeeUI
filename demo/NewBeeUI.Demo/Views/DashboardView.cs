namespace NewBeeUI.Demo.Views;

public class DashboardView : BaseView
{
    protected override object Build()
    {
        return 
            VStack([
                TextBlock("Dashboard View").Align(0, 0),
                TextBlock("This is a placeholder for the dashboard view.").Align(0, 0),
                TextBlock("You can add more components here as needed.").Align(0, 0),
            ])
            .Align(0,0)
            .Opacity(0.5);
    }
}
