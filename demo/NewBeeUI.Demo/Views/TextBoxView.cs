using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

internal class TextBoxView : BaseView
{
    protected override object Build()
    {
        var vstack = VStack([
            new TextBox().PlaceholderText("请输入内容").ListenIME(),
                        new TextBox().PlaceholderText("请输入内容2").ListenIME()
            ]);
        return vstack;
    }
}
