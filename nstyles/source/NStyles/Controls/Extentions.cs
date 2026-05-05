using Avalonia.Controls;
using Avalonia.VisualTree;

namespace NStyles;

public static class Extentions
{
    public static TextBox ListenIME(this TextBox textBox, Predicate<TextBox> predicate = null)
    {
        //if(predicate != null && predicate(textBox) == false)
        //{
        //    return textBox; // 如果不满足条件，则直接返回
        //}

        //bool inited = false;
        //TextBlock? waterMark = null;

        //textBox.KeyDown += (s, e) =>
        //{
        //    if (inited == false)
        //    {
        //        inited = true;
        //        // 当按下键盘时，隐藏水印
        //        foreach (var descendant in textBox.GetVisualDescendants())
        //        {
        //            if (descendant is TextBlock tb && tb.Name == "watermark")
        //            {
        //                waterMark = tb;
        //            }
        //        }
        //    }
        //    if(waterMark != null) waterMark.IsVisible = false;
        //};

        //textBox.LostFocus += (s, e) =>
        //{
        //    if (waterMark != null)
        //    {
        //        waterMark.IsVisible = String.IsNullOrEmpty(textBox.Text);
        //    }
        //};

        return textBox;
    }
}
