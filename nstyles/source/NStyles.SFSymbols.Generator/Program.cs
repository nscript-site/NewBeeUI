using NStyles.SvgIconGenerator;
using System.Runtime.CompilerServices;

namespace NStyles.SFSymbols.Generator;

internal class Program : IconSourceGenerator
{
    static String NameSpace = "NStyles.SFSymbols";

    public static string GetSourceDirectory([CallerFilePath] string filePath = "")
=> Path.GetDirectoryName(filePath)!;

    static void Main(string[] args)
    {
        var path = GetSourceDirectory();
        GenerateIcons(Path.Combine(path, "sf-symbols-svg"), NameSpace);
    }
}
