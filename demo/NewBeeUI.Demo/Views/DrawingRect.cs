namespace NewBeeUI.Demo.Views;

public class DrawingRect : Control
{
    public string Title { get; set; } = "DrawingRect";

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        int margin = 10;

        // 定义矩形区域
        var rect = new Rect(margin, margin, Width - margin * 2, Height - margin * 2);

        // 定义画刷和画笔
        var fill = Brushes.LightBlue;
        var pen = new Pen(Brushes.DarkBlue, 2);

        // 绘制填充矩形
        context.FillRectangle(fill, rect);

        // 绘制边框
        context.DrawRectangle(pen, rect);

        context.DrawLine(new Pen(Brushes.Red, 1), new Point(margin, margin), new Point(Width - margin, Height - margin));
        context.DrawLine(new Pen(Brushes.Red, 1), new Point(Width - margin, margin), new Point(margin, Height - margin));
    }
}
