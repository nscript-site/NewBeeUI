namespace NewBeeUI;

public class ncss
{
    //TODO
}

public struct animate
{
    //TODO
}
public struct brush
{
    private IBrush val;

    public brush(IBrush brush)
    {
        val = brush;
    }

    public static implicit operator brush(Brush value)
    {
        return new brush(value);
    }

    public static implicit operator brush(string value)
    {
        throw new NotImplementedException();
    }

    public static implicit operator brush(uint color)
    {
        throw new NotImplementedException();
    }
}

public struct nbase
{
    public static implicit operator nbase(ncss value)
    {
        throw new NotImplementedException();
    }

    public static implicit operator nbase((ncss,ncss) value)
    {
        throw new NotImplementedException();
    }
}

public struct nanimate
{
    public static implicit operator nanimate(animate value)
    {
        throw new NotImplementedException();
    }

    public static implicit operator nanimate((animate, animate) value)
    {
        throw new NotImplementedException();
    }

    public static implicit operator nanimate((animate, animate,animate) value)
    {
        throw new NotImplementedException();
    }

    public static implicit operator nanimate(animate[] value)
    {
        throw new NotImplementedException();
    }
}

public class Test
{
    #region 使用示范

    public ncss[] styles =>
    [
        ncss(               // on pointover
            animate: (scale(1,1,3), opacity(1,0,1)),
            background: 0xFF000000
        ).Ref(out ncss on_pointover),
        ncss(               // base
            width: 4,
            height: 4,
            margin: [10,20],
            pointover: on_pointover
        ).Ref(out ncss _base),
        ncss(               // for all elements
            @base: _base,
            condition: x => true,
            background: "red"
        ),                  
        ncss(               // for some elements
            @base: _base,
            condition: x => x.Name == "yyy" && x is Button,
            background: new brush(Brushes.Black)
        ),
        ncss(               // find elements by condition, then select sibling
            @base: _base,
            condition: x => x is TextBlock,
            select: x => x.sibling(),       // TextBlock 的 sibling
            background: new brush(Brushes.Black)
        ),
    ];

    #endregion

    public static ncss ncss(ncss? @base = null, int? width = null, int? height = null,
        brush? background = null, nanimate? animate = null, int[]? margin = null, 
        Func<Control, StyledElement?>? select = null,
        ncss? pointover = null,
        Predicate<StyledElement>? condition = null)
    {
        throw new NotImplementedException();
    }

    public static animate scale(double duration, double from, double to)
    {
        throw new NotImplementedException();
    }

    public static animate opacity(double duration, double from, double to)
    {
        throw new NotImplementedException();
    }
}

public static class StyledElementExtention
{
    public static StyledElement? sibling(this StyledElement element)
    {
        throw new NotImplementedException();
    }
}
