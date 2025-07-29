using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Properties;

internal class GlobalStyles
{
    public static Style[] BuildStyles() =>
    [
        new Style<Button>(x=>x.Class(BaseView.Classed_Icon_Button)).Padding(5,-10).BorderThickness(0).CornerRadius(0),
 
        new Style<Border>(x=>x.Class(IconView.Classed_IconView_Border)).Background(new DynamicResourceExtension("SukiBorderBrush")),
    ];
}
