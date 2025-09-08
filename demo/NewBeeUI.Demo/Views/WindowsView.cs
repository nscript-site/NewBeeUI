using NStyles.MeterialIcons;

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
                TextButton("MessageBoxView1").OnClick(_=>{
                    MessageBoxView.Show(this, "这是一条消息XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                }),
                TextButton("ConfirmMessage").OnClick(_=>{
                    MessageBoxView.ShowOkCancel(this, "确定 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX 吗", 
                        onClose: val => {
                            MessageBoxView.Show(this, val ? "你点击了确定" : "你点击了取消");
                        });
                }),
                 TextButton("ConfirmMessageWithIcons").OnClick(_=>{
                    MessageBoxView.ShowOkCancel(this, "确定 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX 吗",
                        iconOkButton: CheckIcon.Instance,
                        iconCancelButton: CloseIcon.Instance,
                        okButtonClasses: "Primary", 
                        cancelButtonClasses: "Danger",
                        iconContent: Icon(InformationIcon.Instance, 20).Foreground(Brushes.Red),
                        onClose: val => {
                            MessageBoxView.Show(this, val ? "你点击了确定" : "你点击了取消");
                        });
                }),
            ]);
    }
}
