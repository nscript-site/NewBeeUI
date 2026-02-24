using Avalonia.Media.Imaging;

namespace NewBeeUI.Demo.Views;

public class GenVideoView : BaseView
{
    protected override void Build(out Control content)
    {
        BuildContent(out Control body);

        VGrid("42,1,*,Auto", [
            HGrid("40,*,40",[
                IconButton(ArrowLeftIcon.Instance).OnClick(_ => {
                this.RemoveFromOverlay();
                }),
                TextBlock(this.Name).Align(0,0)
            ]),
            HLine(1,1).Margin(0),
            body,
            DemoViewCodeView(),
        ]).Background(R("SukiStrongBackground")).Return(out content);
    }

    protected void BuildContent(out Control content)
    {
        VStack([
            TextButton("渲染图像")
                .WhenClick(_=>{ GenImage(); }),
            TextButton("渲染动画")
                .WhenClick(_=>{ GenAnimation(); })
        ]).Align(0,0).Return(out content);
    }

    protected void GenImage()
    {
        Render(HGrid("60,*",[null,TextBlock("你好").Margin(20)])
            .Background(Brushes.White));
        OpenOutputDir();
    }

    protected void GenAnimation()
    {
        long gid = DateTime.Now.ToFileTimeUtc();
        for (int i = 0; i < 10; i++)
        {
            Render(
                HGrid("60,*", [null, TextBlock("你好").Margin(20 + i * 3)])
                    .Background(Brushes.White), gid, i+1
            );
        }
        OpenOutputDir();
    }

    public static void Render(Control view, long? groupId = null, int? num = null)
    {
        int width = 500;
        int height = 800;
        double dpi = 96;

        // 创建渲染目标位图
        using var renderTarget = new RenderTargetBitmap(
            new PixelSize(width, height),
            new Vector(dpi, dpi)
        );

        view.Measure(new Size(width, height));
        view.Arrange(new Rect(0, 0, width, height));
        view.UpdateLayout();

        var gid = groupId ?? DateTime.Now.ToFileTimeUtc();

        // 渲染控件到位图
        renderTarget.Render(view);
        SaveBitmap(renderTarget, gid, num);
    }

    static void SaveBitmap(RenderTargetBitmap bitmap, long groupId, int? num = null)
    {
        DirectoryInfo dirOutput = new DirectoryInfo("output");
        if(dirOutput.Exists == false)
        {
            dirOutput.Create();
        }

        var fileName = num.HasValue ? $"output_{groupId}_{num.Value}.png" : $"output_{groupId}.png";

        bitmap.Save($"output/{fileName}");
    }

    static void OpenOutputDir()
    {
        DirectoryInfo dirOutput = new DirectoryInfo("output");
        if (dirOutput.Exists)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = dirOutput.FullName,
                UseShellExecute = true,
                Verb = "open"
            });
        }
    }
}
