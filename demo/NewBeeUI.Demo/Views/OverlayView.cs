namespace NewBeeUI.Demo.Views;

public class OverlayView : BaseView
{
    protected override void Build(out Control content)
    {
        if (App.IsMobileApp == false && App.MockMobileOnDesktop == false)
        {
            VStack(BuildDesktop().ConcatWith(BuildMobile()))
                .Return(out content);
        }
        else
        {
            VStack(BuildMobile())
                .Return(out content);
        }
    }

    protected Control[] BuildMobile()
    {
        return
        [
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
            }),
            TextButton("Modal").OnClick(_ =>
            {
                var modal = new ModalView(){ Name = "Modal View" };
                modal.ShowInOverlay(this,true,0);
            }),
            DemoViewCodeView(),
        ];
    }

    protected Control[] BuildDesktop()
    {
        return
        [
            TextButton("弹出窗口1").OnClick(async _=>{
                await new PopupWindowView().ShowDialogAsync(null);
            }),
            TextButton("弹出窗口2").OnClick(async _=>{
                await new PopupWindowView().ShowDialogAsync(null, "自定义窗口标题"); 
            })
        ];
    }
}
