using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class ButtonsView : BaseView
{
    public ViewRouter? Router { get; set; }

    protected override object Build()
    {
        return Grid(rows: "*,60").Children([
                VStack(0, 0).Children([TextButton("返回").OnClick(_=>{
                        Router?.GoBack(); }),
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
