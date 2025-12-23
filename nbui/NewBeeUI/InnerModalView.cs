using Avalonia.Controls.Shapes;

namespace NewBeeUI;

internal class InnerModalView : BaseView
{
    protected override object Build()
    {
        var border = new Rectangle().Fill(R("SukiCardBackground")).Opacity(0.7);
        return border;
    }
}
