using Avalonia.Controls.Templates;

namespace NewBeeUI;

public class MemuIconHeader : IDataTemplate
{
    private StreamGeometry? _geometry;

    private IconView? _iconView;

    public MemuIconHeader(StreamGeometry geometry)
    {
        this._geometry = geometry;
    }

    public MemuIconHeader(IconView iconView)
    {
        this._iconView = iconView;
    }

    public Control Build(object? data)
    {
        if (_iconView != null)
            return _iconView;
        else if (_geometry != null)
            return new PathIcon().Data(_geometry).Size(24, 24);
        else
            return new PathIcon().Data(NStyles.MeterialIcons.MenuIcon.Instance).Size(24, 24);
    }

    public bool Match(object? data)
    {
        return true;
    }
}
