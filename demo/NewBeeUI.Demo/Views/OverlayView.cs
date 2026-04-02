using System;

namespace NewBeeUI.Demo.Views;

public class OverlayView : BaseView
{
    protected override void Build(out Control content)
    {
        if (App.IsMobileApp == false && App.MockMobileOnDesktop == false)
        {
            VStack(BuildDesktop().ConcatWith(BuildMobile())).Spacing(32)
                .Return(out content);
        }
        else
        {
            VStack(BuildMobile()).Spacing(32)
                .Return(out content);
        }
    }

    protected Control[] BuildMobile()
    {
        return
        [
            GroupBox("Toast",
                HStack([
                    TextButton("Toast示例1").OnClick(_=>{
                        this.ShowToast("这是一条 Toast 消息");
                    }),
                    TextButton("Toast示例2").OnClick(_=>{
                        this.ShowToast(
                            HStack([
                                TextBlock("这是一条复杂的 Toast 消息"),
                                Icon(WardrobeIcon.Instance),
                                TextBlock("!!")
                                ])
                        );
                    }),
                ])
            ),

            GroupBox("消息弹窗",
                HStack([
                    TextButton("消息弹窗示例").OnClick(_=>{
                        MessageBoxView.Show(this, "这是一条消息XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                    }),
                ])
            ),

            GroupBox("消息确认",
                HStack([
                    TextButton("消息确认示例").OnClick(async _=>{
                        await MessageBoxView.ShowOkCancel(this, "确定 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX 吗",
                            onClose: val => {
                                MessageBoxView.Show(this, val ? "你点击了确定" : "你点击了取消");
                            });
                    }),
                    TextButton("消息确认示例（带图标）").OnClick(async _=>{
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
                ])
            ),

            GroupBox("模态窗口",
                HStack([
                    TextButton("模态窗口示例").OnClick(_ =>
                    {
                        var modal = new ModalView(){ Name = "模态窗口示例" };
                        modal.ShowInOverlay(this,true,0);
                    }),
                ])
            ),

            GroupBox("显示加载",
                HStack([
                    TextButton("显示加载示例1").WhenClick(_ => {
                        this.RunWithDelayedLoading(() =>
                        {
                            // Do some work
                            Thread.Sleep(3000);
                        }, runAtBackground: true);
                    }),
                    TextButton("显示加载示例2").WhenClick(_ => {
                        this.RunWithDelayedLoading(() =>
                        {
                            // Do some work
                            Thread.Sleep(3000);
                        },  runAtBackground: true, centeredInApp: true, text: TextBlock("加载中"));
                    }),
                ])
            ),

            GroupBox("Overlay",
                HStack([
                    TextButton("添加").OnClick(_ => {
                        num ++;
                        var count = this.OverlayHosts()?.Count??0;
                        this.AddOverlay(TextBlock($"[{num}] Overlay 内容").Background(R_SukiCardBackground).Margin(0,0,20,90 + count * 30).Align(1, 1));
                    }),
                    TextButton("移除").OnClick(_ => {
                        var count = this.OverlayHosts()?.Count??0;
                        if(count <= 0) return;
                        this.RemoveOverlayAt(count-1);
                    }),
                    TextButton("清除").OnClick(_ => {
                        this.ClearOverlays();
                    }),
                ])
            ),

            DemoViewCodeView(),
        ];
    }

    int num = 0;

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
