using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Styling;

namespace NewBeeUI.Demo.Properties;

internal class GlobalStyles
{
    public static Style[] BuildStyles() =>
    [
        new Style<Button>(x=>x.Class(BaseView.Classed_Icon_Button)).Padding(5,-10).BorderThickness(0).CornerRadius(0),
 
        new Style<Border>(x=>x.Class(IconView.Classed_IconView_Border)).Background(new DynamicResourceExtension("SukiBorderBrush")),

        new Style<StackPanel>(x=>x.Class(BaseView.BaseView_Classes_VStack)).HorizontalAlignment(HorizontalAlignment.Stretch).VerticalAlignment(VerticalAlignment.Top).Spacing(5)
    ];
}
