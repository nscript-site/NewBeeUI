using NStyles.SvgIconGenerator;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace NStyles.ElementPlusIcons.Generator;

internal class Program:IconSourceGenerator
{
    static String NameSpace = "NStyles.ElementPlusIcons";

    public static string GetSourceDirectory([CallerFilePath] string filePath = "")
    => Path.GetDirectoryName(filePath)!;

    static void Main(string[] args)
    {
        var path = GetSourceDirectory();

        GenerateIcons(Path.Combine(path, "svg"), NameSpace);
    }
}