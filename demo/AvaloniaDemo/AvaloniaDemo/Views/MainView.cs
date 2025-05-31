using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaDemo.Views;

public class MainView : BaseView
{
    protected override object Build()
    {
        return Grid(rows: "*,60").Children([
                VStack(0, 0).Children([
                    TextBlock("Hello World!").Align(0, 0),
                    new ToggleSwitch(),
                    IconButton(SearchIcon.Instance),
                    IconButton(NStyles.MeterialIcons.SearchWebIcon.Instance),
                ]),
                HStack(0,0).Row(1).Children([
                    IconButton(SearchIcon.Instance, "ToolTip", ToolTipPosition.Top ), SelectableIconButton(SearchIcon.Instance, "ToolTip", "ToolTip2", ToolTipPosition.Top),
                    ])
            ]);
    }
}
