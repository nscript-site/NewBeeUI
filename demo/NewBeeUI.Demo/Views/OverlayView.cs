namespace NewBeeUI.Demo.Views;

public class OverlayView : BaseView
{
    protected override object Build()
    {
        return VStack(0, 0).Children([
            TextButton("添加 Overlay").OnClick(_ => {
                var hosts = this.OverlayHosts();
                if(hosts != null && hosts.Count == 0)
                {
                    hosts.Add(TextBlock("添加 Overlay").Margin(30).Align(1, 1));
                }
            }),
            TextButton("移除 Overlay").OnClick(_ => {
                var hosts = this.OverlayHosts();
                hosts?.Clear();
            })
        ]);
    }
}
