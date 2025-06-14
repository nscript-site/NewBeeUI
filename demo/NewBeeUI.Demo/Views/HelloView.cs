using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class HelloView : BaseView
{
    int count = 0;

    protected override object Build()
    {
        return VStack([
                TextBlock().Align(0).Text(() => $"Click {count} times"),
                TextButton("Hello").OnClick(_=>{
                    count++;
                    this.UpdateState();
                })
            ]).Margin(20);
    }
}
