using NStyles.SvgIconGenerator;
using System.Runtime.CompilerServices;

namespace NStyles.FontAwesome7.Generator;

internal class Program : IconSourceGenerator
{
    static String NameSpace = "NStyles.FontAwesome7";

    public static string GetSourceDirectory([CallerFilePath] string filePath = "")
    => Path.GetDirectoryName(filePath)!;

    static void Main(string[] args)
    {
        var path = GetSourceDirectory();
        GenerateIcons(Path.Combine(path, "brands"), $"{NameSpace}.BrandIcons", "Brands.cs");
        GenerateIcons(Path.Combine(path, "regular"), $"{NameSpace}.RegularIcons", "Regular.cs");
        GenerateIcons(Path.Combine(path, "solid"), $"{NameSpace}.SolidIcons", "Solid.cs");
    }
}