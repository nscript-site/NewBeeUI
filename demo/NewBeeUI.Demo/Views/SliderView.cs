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
        return new Slider().Width(200);
        //return HStack([
        //    new Slider().Width(200).Align(0,-1),
        //    VStack([
        //    HStack([
        //        new Slider().Width(200)
        //        ]),
        //    new Slider().Width(200),
        //    new Slider().Width(200),
        //    ])
        //    ]);
        //return VStack([
        //    HStack([
        //        new Slider().Width(200)
        //        ]),
        //    new Slider().Width(200),
        //    new Slider().Width(200),
        //    ]); 
    }
}
