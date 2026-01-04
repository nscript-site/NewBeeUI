using System;
using System.Collections.Generic;
using System.Text;

namespace NewBeeUI.Demo.Views;

public class LayoutView : BaseView
{
    protected override void Build(out Control content)
    {
        VStack([
            TextBlock("LayoutView"),
            DemoViewCodeView(),
        ]).Return(out content);
    }
}
