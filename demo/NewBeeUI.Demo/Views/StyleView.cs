using Avalonia.Styling;

namespace NewBeeUI.Demo.Views;

public class StyleView : BaseView
{
    protected override object Build()
    {
        var stack = VStack([
            TextBlock("This is a text block with default style"),
            TextButton("Button1").Classes("btn_cls1").Name("btn1")
                .OnClick(() => Console.WriteLine("Button1 clicked!")),
            TextButton("Button2").Classes("btn_cls2").Name("btn2")
                .OnClick(() => Console.WriteLine("Button2 clicked!")),
                TextButton("Button3").Classes("btn_cls3").Name("btn3")
                .OnClick(() => Console.WriteLine("Button3 clicked!"))
        ])
        .Spacing(10);

        return stack;
    }

    protected override StyleGroup? BuildStyles()
    {
        //return base.BuildStyles();

        return
        [
            new Style<TextBlock>().FontSize(14).Foreground(Brushes.Red),
            new Style<Button>(x => x.Class("btn_cls1")).Background(Brushes.Green).Foreground(Brushes.Black),
            new Style<Button>(x => x.Class("btn_cls2")).Background(Brushes.Blue).Foreground(Brushes.Black),
            //new Style<Button>().Selector(x=>x.Class("btn_cls3")).BorderThickness(2d)
            //    .BorderBrush(Brushes.Red)
            //new Style<Button>().Selector(Select).Background(Brushes.Blue).Foreground(Brushes.Black).Padding(10),
        ];
    }

    protected Selector Select(Selector selector)
    {
        return selector;
    }
}
