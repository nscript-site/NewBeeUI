namespace NewBeeUI.Demo.Views;

public class OverlayView : BaseView
{
    protected override object Build()
    {
        var list = new List<Control>();

        if(App.IsMobileApp == false && App.MockMobileOnDesktop == false)
        {
            Control[] desktopOnlyList = [
                TextButton("弹出窗口1").OnClick(async _=>{
                            await new PopupWindowView().ShowDialogAsync(null);
                        }),
                TextButton("弹出窗口2").OnClick(async _=>{
                    await new PopupWindowView().ShowDialogAsync(null, "自定义窗口标题");
                }),
            ];
            list.AddRange(desktopOnlyList);
        }

        Control[] commonList = [
            TextButton("MessageBoxView1").OnClick(_=>{
                MessageBoxView.Show(this, "这是一条消息XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            }),
            TextButton("ConfirmMessage").OnClick(async _=>{
                await MessageBoxView.ShowOkCancel(this, "确定 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX 吗",
                    onClose: val => {
                        MessageBoxView.Show(this, val ? "你点击了确定" : "你点击了取消");
                    });
            }),
                TextButton("ConfirmMessageWithIcons").OnClick(async _=>{
                await MessageBoxView.ShowOkCancel(this, "确定 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX 吗",
                    iconOkButton: CheckIcon.Instance,
                    iconCancelButton: CloseIcon.Instance,
                    okButtonClasses: "Primary",
                    cancelButtonClasses: "Danger",
                    iconContent: Icon(InformationIcon.Instance, 20).Foreground(Brushes.Red),
                    onClose: val => {
                        MessageBoxView.Show(this, val ? "你点击了确定" : "你点击了取消");
                    });
            }),
            TextButton("添加 Overlay").OnClick(_ => {
                var hosts = this.OverlayHosts();
                if(hosts != null && hosts.Count == 0)
                {
                    hosts.Add(TextBlock("添加 Overlay 的内容").Margin(100).Align(0, -1));
                }
            }),
            TextButton("移除 Overlay").OnClick(_ => {
                var hosts = this.OverlayHosts();
                hosts?.Clear();
            })
        ];

        list.AddRange(commonList);

        return VStack(0, 0).Spacing(10)
            .Children(list.ToArray());
    }
}
