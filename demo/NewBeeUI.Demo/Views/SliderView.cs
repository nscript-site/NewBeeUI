using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class SliderView : BaseView
{
    protected override object Build()
    {
        return VStack([
            HStack([
                new Slider().Width(200)
                ]),
                new ProgressBar().Width(200).Value(50)
            ]);
    }
}
