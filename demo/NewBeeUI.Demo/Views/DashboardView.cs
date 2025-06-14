using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class DashboardView : BaseView
{
    protected override object Build()
    {
        return VStack(0, 0).Children([
            TextBlock("Dashboard View").Align(0, 0),
            TextBlock("This is a placeholder for the dashboard view.").Align(0, 0),
            TextBlock("You can add more components here as needed.").Align(0, 0),
        ]);
    }
}
