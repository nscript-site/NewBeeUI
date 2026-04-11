using Avalonia.Data;

namespace NewBeeUI.Demo.Views.Components;

public class DemoColorView : BaseView
{
    public BindingBase? Color { get; set; }

    protected override void Build(out Control content)
    {
        var border = Border();
        if (Color != null) border.Background(Color);

        VStack([
            HGrid("Auto,*",[
                    TextBlock(Name??String.Empty).Align(-1,-1),
                    IconButton(ContentCopyIcon.Instance, "复制", iconSize:12).Height(24).WhenClick(_=>{ CopyToClipboard($"R_{Name}"); }).Align(1,-1)
                ]),
                border.Height(36).BorderThickness(1).BorderBrush(R_SukiBorderBrush)
            ]).Spacing(2)
            .Return(out content);
    }
}
