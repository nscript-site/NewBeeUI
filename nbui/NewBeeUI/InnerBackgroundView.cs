using Avalonia.Controls.Shapes;

namespace NewBeeUI;

internal class InnerBackgroundView : BaseView
{
    public BaseView? RelatedView;

    internal bool IsModal = true;

    internal Action? OnRemove;

    protected override object Build()
    {
        var border = new Rectangle().Fill(R("SukiCardBackground"));
        return IsModal ? border : new Border();
    }
}
