using Avalonia.Threading;

namespace NewBeeUI;

public class MobBottomTab : BaseView
{
    public string Title { get; set; } = string.Empty;

    public PathIcon? Icon { get; set; }

    public Action<MobBottomTab>? OnClick { get; set; }

    private bool isSelected = false;
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            if (isSelected != value)
            {
                isSelected = value;
                UpdateColor();
            }
        }

    }

    PathIcon icon = default!;
    TextBlock title = default!;

    protected override object Build()
    {
        if(Icon!= null)
        {
            if (Icon.Parent != null)
            {
                (Icon.Parent as StackPanel)?.Children.Remove(Icon);
            }
        }

        var stack = VStack([
            (Icon ?? new PathIcon().Data(Icons.Star)).Align(0).IsHitTestVisible(false).Ref(out icon),
            TextBlock(Title).Align(0).FontSize(12).IsHitTestVisible(false).Ref(out title),
        ]).Background(Brushes.Transparent).Spacing(2);

        UpdateColor();

        stack.WhenClick((_) => FireOnClick());

        return stack;
    }

    private void UpdateColor()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (IsSelected)
            {
                title.Foreground(R("SukiPrimaryColor"));
                icon.Foreground(R("SukiPrimaryColor"));
            }
            else
            {
                title.Foreground(R("SukiText"));
                icon.Foreground(R("SukiText"));
            }
        });
    }

    private void FireOnClick()
    {
        IsSelected = true;
        OnClick?.Invoke(this);
    }

    public static MobBottomTab CreateFrom(RoutedViewBuilder builder)
    {
        return new MobBottomTab
        {
            Title = builder.Name,
            Icon = builder.Icon
        };
    }

    public static MobBottomTab[] CreateFrom(IList<RoutedViewBuilder> builders)
    {
        return builders.Select(CreateFrom).ToArray();
    }
}
