using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class SettingView : BaseView, IWindowView
{
    public WindowInfo WindowInfo { get; } = new WindowInfo()
    {
        WindowTitle = "设置",
        CanResize = false,
        CanMinimize = false,
        CanClose = true,
        WindowMinWidth = 300,
        WindowMinHeight = 200,
        WindowWidth = 400,
        WindowHeight = 300,
    };

    protected override object Build()
    {
        return VStack(0, 0).Children([
            TextButton("切换基础主题（白天/黑夜）").FlatStyle().OnClick(_ => {
                NTheme.GetInstance()?.SwitchBaseTheme();
            }),
            TextButton("切换颜色主题").FlatStyle().OnClick(_ => {
                NTheme.GetInstance()?.SwitchColorTheme();
            })
        ]);
    }
}
