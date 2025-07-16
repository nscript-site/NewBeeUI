using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBeeUI.Demo.Views;

public class FlyoutView : BaseView
{
    protected override object Build()
    {
        return VStack([
            TextButton("Open Flyout")
                .Flyout(new TextBlock().Text("XXXX")),
            IconButton(DownloadBoxIcon.Instance)
                .Flyout(
                        VStack([
                            new TextBlock().Text("AAAA"),
                            new TextBlock().Text("BBBB")
                        ]).Margin(20),
                    f => f
                    .Placement(PlacementMode.Bottom).VerticalOffset(10)
                ),
            ])
            .Align(0,0);
    }
}
