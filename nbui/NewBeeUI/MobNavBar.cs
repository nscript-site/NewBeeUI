using System.Text;

namespace NewBeeUI;

public class MobNavBar : BaseView
{
    public MobIconButton[]? Items { get; set; }

    public string? CustomGridRows { get; set; } = null;

    public Action<int,MobIconButton>? OnSelect { get; set; }

    public bool HasOverlay { get; init; } = false;

    private Panel? _overlayLayer = null;

    public Panel? OverlayLayer { get => _overlayLayer; }

    private Control? _mainLayer = null;
    public Control? MainLayer { get => _mainLayer; }

    protected override object Build()
    {
        if (Items == null || Items.Length == 0) return TextBlock("Empty Nav Bar");

        var rows = CustomGridRows;
        if(rows == null)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Items.Length; i++)
            {
                sb.Append("*");
                if (i != Items.Length - 1)
                {
                    sb.Append(",");
                }
            }
            rows = sb.ToString();
        }

        for(int i = 0; i < Items.Length; i++)
        {
            var item = Items[i];
            if(OnSelect != null)
            {
                var index = i; // Capture index for closure
                item.OnClick = (tab) =>
                {
                    OnSelect(index, tab);
                };
            }
        }

        var grid = HGrid(rows, Items);
        var border = new Border().Child(grid)
                .Background(R("SukiCardBackground"))
                .Padding(10, 10, 10, 10);
        _mainLayer = border;

        if (HasOverlay == false)
            return border;
        else
        {
            _overlayLayer = new Panel();
            return Panel([border, _overlayLayer]);
        }
    }

    public void SelectIndex(int index)
    {
        if (Items == null || index < 0 || index >= Items.Length) return;
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].IsSelected = (i == index);
            OnSelect?.Invoke(index, Items[i]);
        }
    }

    public int GetSelectedIndex()
    {
        if (Items == null) return -1;
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].IsSelected) return i;
        }
        return -1;
    }
}
