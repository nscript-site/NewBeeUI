namespace NewBeeUI;

using C = (string Name, string IP);

public class ToggleIconButtonView : BaseView
{
    public PathIcon OnIcon { get; init; } = default!;

    public PathIcon OffIcon { get; init; } = default!;

    public string OnToolTip { get; set; } = String.Empty;

    public string OffToolTip { get; set; } = String.Empty;

    protected override void Build(out Control content)
    {
        C a = new C("name", "ip");

        base.Build(out content);
    }
}
