using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class PopupWindowView : BaseView, IWindowView
{
    public WindowInfo WindowInfo { get; } = new WindowInfo()
    {
        WindowTitle = "弹出窗口",
        CanResize = false,
        CanMinimize = false,
        CanClose = false,
        WindowMinWidth = 300,
        WindowMinHeight = 200,
        WindowWidth = 400,
        WindowHeight = 300,
    };

    protected override object Build()
    {
        return VStack(null, 0)
            .Children([

                TextButton("在母窗体弹出").OnClick(async _=>{
                    await new PopupWindowView().ShowDialogAsync("在母窗体弹出");
                }),

                TextButton("在子窗体弹出").OnClick(async _=>{
                    await new PopupWindowView().ShowDialogAsync(this, "在子窗体弹出");
                }),

                TextButton("在子窗体弹出2").OnClick( _=>{
                    new PopupWindowView().ShowDialog(this, "在子窗体弹出2");
                }),

                TextButton("关闭窗口").OnClick(_=>{
                    this.CloseWindow();
                }),
            ]);
    }
}
