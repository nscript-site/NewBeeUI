
using NStyles.MeterialIcons;

namespace NewBeeUI;

public class SourceCodeView : BaseView, IWindowView
{
    public WindowInfo WindowInfo { get; } = new WindowInfo()
    {
        WindowTitle = $"浏览源代码",
        CanResize = true,
        CanMinimize = true,
        CanClose = true,
        WindowMinWidth = 300,
        WindowMinHeight = 200,
        WindowWidth = 1000,
        WindowHeight = 800,
    };

    public string Codes { get; set; } = String.Empty;
    public string FileName { get; set; } = String.Empty;

    protected override void Build(out Control content)
    {
        VGrid("Auto, *", [
            HGrid("Auto, *,Auto",[
                Icon(FileCodeOutlineIcon.Instance, size: 16).Foreground(R("SukiPrimaryColor")).Margin(10,0,4,0).Align(0,0),
                TextBlock($"{FileName}").Align(-1,0).Margin(0,0,0,-2),
                IconButton(ContentCopyIcon.Instance, "复制源代码", iconSize:16).WhenClick(_=>{ CopyToClipboard(Codes); }).Align(1,1),
            ]).Margin(0,0,0,0).Height(32),
            new TextBox().Text(Codes).Align(null,null).IsReadOnly(true).TextWrapping(TextWrapping.Wrap),
        ]).Margin(10,10).Return(out content);
    }
}
