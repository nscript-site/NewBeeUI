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
                VStack(0, 0).Children([TextButton("返回").OnClick(_=>{ Router?.GoBack(); }),
                    TextBlock("Hello World!").Align(0, 0),
                    new ToggleSwitch(),
                    IconButton(SearchIcon.Instance),
                    IconButton(Icons.ScaleToOriginal),
                    IconButton("InnerText", SearchIcon.Instance),
                    IconButton("InnerText", SearchIcon.Instance, iconSize:12),
                    IconButton(NStyles.MeterialIcons.SearchWebIcon.Instance),
                    TextButton("显示加载").WhenClick(_ => MockLoading()),
                ]),
                HStack(0,0).Row(1).Children([
                    IconButton(SearchIcon.Instance, "ToolTip", ToolTipPosition.Top ), SelectableIconButton(SearchIcon.Instance, "ToolTip", "ToolTip2", ToolTipPosition.Top),
                    SelectableIconButton(CogBoxIcon.Instance).OnClick(v=>{ v.Selected = !v.Selected; v.UpdateState(); }),
                    ])
            ]);
    }

    protected void MockLoading()
    {
        this.RunWithDelayedLoading(() =>
        {
            Thread.Sleep(3000);
        }, runAtBackground: true, onCreate: l => { l.Margin(200, 0, 0, 0); });
    }
}
