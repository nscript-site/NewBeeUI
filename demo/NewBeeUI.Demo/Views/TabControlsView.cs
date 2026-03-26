using System;
using System.Collections.Generic;
using System.Text;

namespace NewBeeUI.Demo.Views;

public class TabControlsView : BaseView
{
    protected override void Build(out Control content)
    {
        var tabs = new TabItem[]
        {
            HTextTabItem("Tab1", Panel(TextBlock("Content1")), textAlign:1),
            HTextTabItem("Tab2", Panel(TextBlock("Content2")), textAlign:1)
        };

        HTabs(tabs).Return(out content);
    }
}
