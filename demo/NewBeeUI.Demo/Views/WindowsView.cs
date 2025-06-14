namespace NewBeeUI.Demo.Views;

public class WindowsView : BaseView
{
    protected override object Build()
    {
        return VStack(null, 0).Spacing(10)
            .Children([
                TextButton("弹出窗口1").OnClick(async _=>{
                    await new PopupWindowView().ShowDialogAsync(null);
                }),
                TextButton("弹出窗口2").OnClick(async _=>{
                    await new PopupWindowView().ShowDialogAsync(null, "自定义窗口标题");
                }),
            ]);
    }
}
