using System.Diagnostics;

namespace NewBeeUI.Demo;

public static class GlobalClassExtentions
{
    public static TextBlock TitleStyle(this TextBlock tb, double size = 15)
    {
        tb.FontSize(size).FontWeight(FontWeight.Bold);
        return tb;
    }

    public static TextBlock NormalText(this TextBlock tb, float em = 0, bool lowText = false)
    {
        float size = 12 + em;
        tb.FontSize(size);
        if (lowText)
        {
            tb.Foreground(BaseView.R("SukiLowTextForeground"));
        }
        return tb;
    }
}
