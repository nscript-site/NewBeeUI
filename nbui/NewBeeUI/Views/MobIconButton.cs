using Avalonia.Threading;

namespace NewBeeUI;

public class MobIconButton : BaseView
{
    public string Title { get; set; } = string.Empty;

    public new PathIcon? Icon { get; set; }

    public Action<MobIconButton>? OnClick { get; set; }

    public bool AutoSelectedWhenClick { get; set; } = true;

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
        stack.WhenPointer(act => {
            UpdateColor();
        });

        return stack;
    }

    private void UpdateColor()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (IsSelected || this.IsPointerOver)
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
        if(AutoSelectedWhenClick == true)
            IsSelected = true;

        OnClick?.Invoke(this);
    }

    public static MobIconButton CreateFrom(RoutedViewBuilder builder)
    {
        return new MobIconButton
        {
            Title = builder.Name,
            Icon = builder.Icon
        };
    }

    public static MobIconButton[] CreateFrom(IList<RoutedViewBuilder> builders)
    {
        return builders.Select(CreateFrom).ToArray();
    }
}
