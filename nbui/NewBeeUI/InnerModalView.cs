using Avalonia.Controls.Shapes;

namespace NewBeeUI;

internal class InnerModalView : BaseView
{
    protected override object Build()
    {
        var border = new Rectangle().Fill(R("SukiCardBackground"));
        return border;
    }
}
