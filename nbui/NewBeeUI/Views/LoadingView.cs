using NStyles.Controls;

namespace NewBeeUI;

public class LoadingView : BaseView
{
    public TextBlock? TitleContent { get; set; }

    protected override void Build(out Control content)
    {
        if (TitleContent == null)
            new Loading().Return(out content);
        else
            VStack([
                new Loading(),
                TitleContent
                ]).Align(0,0).Return(out content);
    }
}
