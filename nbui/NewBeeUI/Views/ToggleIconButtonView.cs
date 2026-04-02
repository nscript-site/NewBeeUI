namespace NewBeeUI;

public class ToggleIconButtonView : BaseView
{
    public StreamGeometry IconOn { get; init; } = default!;

    public StreamGeometry IconOff { get; init; } = default!;

    public string? ToolTipOn { get; set; }

    public string? ToolTipOff { get; set; }

    public bool IsOn { get; set; } = true;

    internal double? IconSize;

    public Action<ToggleIconButtonView>? OnSwitch { get; set; } 

    protected override void Build(out Control content)
    {
        Panel([
            IconButton(IconOn,ToolTipOn, iconSize: IconSize)
                .IsVisible(()=>IsOn)
                .OnClick(_ => { 
                    IsOn = false;
                    OnSwitch?.Invoke(this);
                    this.UpdateState();
                }),
            IconButton(IconOff,ToolTipOff, iconSize: IconSize)
                .IsVisible(()=>!IsOn)
                .OnClick(_ => { 
                    IsOn = true;
                    OnSwitch?.Invoke(this);
                    this.UpdateState();
                }),
        ]).Return(out content);
    }
}
