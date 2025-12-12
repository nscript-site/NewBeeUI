using ConsoleAppFramework;
using SkiaSharp;

namespace NewBeeUI.Tools.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var app = ConsoleAppFramework.ConsoleApp.Create();
        app.Add("gicon", GenerateiOSAppIcon);
        app.Run(args);
    }

    /// <summary>
    /// Generate iOS App Icon set from a single input image.
    /// </summary>
    /// <param name="input">File path to the input image.</param>
    static void GenerateiOSAppIcon(string input)
    {
        // 用 SkiaSharp 从 input 读取图像文件
        using var inputStream = File.OpenRead(input);
        using var bitmap = SkiaSharp.SKBitmap.Decode(inputStream);
        if (bitmap == null)
        {
            Console.WriteLine("Failed to load image. Please check the input file path and format.");
            return;
        }
        Console.WriteLine($"Loaded image: {input} ({bitmap.Width}x{bitmap.Height})");

        var sizes = new int[] { 20, 29, 40, 58, 60, 76, 80, 87, 120, 152, 167, 180, 1024 };

        DirectoryInfo dirOutput = new DirectoryInfo(DateTime.Now.ToString("yyyyMMdd_HHmmss_iOSAppIcon"));
        dirOutput.Create();

        foreach (var item in sizes)
        {
            GenerateBitmap(dirOutput, bitmap, item);
        }

        Console.WriteLine("iOS App Icon generation completed.");
    }

    static void GenerateBitmap(DirectoryInfo dirOutput, SKBitmap bitmap, int size)
    {
        var resized = bitmap.Resize(new SKImageInfo(size, size), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None));
        if (resized == null)
        {
            Console.WriteLine($"Failed to resize image to {size}x{size}");
            return;
        }
        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        var outputPath = $"Icon{size}.png";
        outputPath = Path.Combine(dirOutput.FullName, outputPath);
        using var outputStream = File.OpenWrite(outputPath);
        data.SaveTo(outputStream);
        Console.WriteLine($"Saved resized image: {outputPath}");
    }
}
