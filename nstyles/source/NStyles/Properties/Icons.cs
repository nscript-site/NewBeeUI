using Avalonia.Media;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NStyles;

/// <summary>
/// AppIcons provided by:
/// :: Material Icons under Apache V2 - https://github.com/google/material-design-icons/blob/master/LICENSE
/// </summary>
public static class Icons
{
    public static StreamGeometry Search { get; } = GetIcon(nameof(Search), IconsResources.Search);

    public static StreamGeometry WindowMinimize { get; } = GetIcon(nameof(WindowMinimize), IconsResources.WindowMinimize);

    public static StreamGeometry FileOpen { get; } = GetIcon(nameof(FileOpen), IconsResources.FileOpen);

    public static StreamGeometry WindowRestore { get; } = GetIcon(nameof(WindowRestore), IconsResources.WindowRestore);

    public static StreamGeometry WindowMaximize { get; } = GetIcon(nameof(WindowMaximize), IconsResources.WindowMaximize);

    public static StreamGeometry WindowClose { get; } = GetIcon(nameof(WindowClose), IconsResources.WindowClose);

    public static StreamGeometry Check { get; } = GetIcon(nameof(Check), IconsResources.Check);

    public static StreamGeometry Cross { get; } = GetIcon(nameof(Cross), IconsResources.Cross);

    public static StreamGeometry Calendar { get; } = GetIcon(nameof(Calendar), IconsResources.Calendar);
    public static StreamGeometry Plus { get; } = GetIcon(nameof(Plus), IconsResources.Plus);
    public static StreamGeometry Minus { get; } = GetIcon(nameof(Minus), IconsResources.Minus);
    public static StreamGeometry Error { get; } = GetIcon(nameof(Error), IconsResources.Error);
    public static StreamGeometry Login { get; } = GetIcon(nameof(Login), IconsResources.Login);
    public static StreamGeometry ChevronUp { get; } = GetIcon(nameof(ChevronUp), IconsResources.ChevronUp);
    public static StreamGeometry ChevronDown { get; } = GetIcon(nameof(ChevronDown), IconsResources.ChevronDown);
    public static StreamGeometry ChevronLeft { get; } = GetIcon(nameof(ChevronLeft), IconsResources.ChevronLeft);
    public static StreamGeometry ChevronRight { get; } = GetIcon(nameof(ChevronRight), IconsResources.ChevronRight);
    public static StreamGeometry CircleCheck { get; } = GetIcon(nameof(CircleCheck), IconsResources.CircleCheck);
    public static StreamGeometry CircleWarning { get; } = GetIcon(nameof(CircleWarning), IconsResources.CircleWarning);
    public static StreamGeometry CircleInformation { get; } = GetIcon(nameof(CircleInformation), IconsResources.CircleInformation);
    public static StreamGeometry CircleClose { get; } = GetIcon(nameof(CircleClose), IconsResources.CircleClose);
    public static StreamGeometry CircleOutline { get; } = GetIcon(nameof(CircleOutline), IconsResources.CircleOutline);
    public static StreamGeometry CircleOutlineClose { get; } = GetIcon(nameof(CircleOutlineClose), IconsResources.CircleOutlineClose);
    public static StreamGeometry CircleOutlinePlus { get; } = GetIcon(nameof(CircleOutlinePlus), IconsResources.CircleOutlinePlus);
    public static StreamGeometry CircleOutlineMinus { get; } = GetIcon(nameof(CircleOutlineMinus), IconsResources.CircleOutlineMinus);
    public static StreamGeometry CircleOutlineCheck { get; } = GetIcon(nameof(CircleOutlineCheck), IconsResources.CircleOutlineCheck);
    public static StreamGeometry KeyboardCaps { get; } = GetIcon(nameof(KeyboardCaps), IconsResources.KeyboardCaps);
    public static StreamGeometry BackspaceOutline { get; } = GetIcon(nameof(BackspaceOutline), IconsResources.BackspaceOutline);
    public static StreamGeometry ArrowLeft { get; } = GetIcon(nameof(ArrowLeft), IconsResources.ArrowLeft);
    public static StreamGeometry ArrowRight { get; } = GetIcon(nameof(ArrowRight), IconsResources.ArrowRight);
    public static StreamGeometry Menu { get; } = GetIcon(nameof(Menu), IconsResources.Menu);
    public static StreamGeometry Star { get; } = GetIcon(nameof(Star), IconsResources.Star);
    public static StreamGeometry InformationOutline { get; } = GetIcon(nameof(InformationOutline), IconsResources.InformationOutline);
    public static StreamGeometry AlertOutline { get; } = GetIcon(nameof(AlertOutline), IconsResources.AlertOutline);
    public static StreamGeometry RotateRight { get; } = GetIcon(nameof(RotateRight), IconsResources.RotateRight);

    public static StreamGeometry GetIcon(string name, string svg)
    {
        if (IconsResources._iconCache.TryGetValue(name, out var icon))
        {
            return icon;
        }

        icon = StreamGeometry.Parse(svg);
        IconsResources._iconCache.AddOrUpdate(name, icon, (key, oldValue) => icon);
        return icon;
    }
}

public class SearchIcon
{
    private static readonly Lazy<SearchIcon> _instance = new(() => new SearchIcon());
    public static StreamGeometry Instance => _instance.Value.g;

    private StreamGeometry g;
    private SearchIcon()
    {
        g = StreamGeometry.Parse("M11.5,2.75 C16.3324916,2.75 20.25,6.66750844 20.25,11.5 C20.25,13.6461673 19.4773285,15.6118676 18.1949905,17.1340957 L25.0303301,23.9696699 C25.3232233,24.2625631 25.3232233,24.7374369 25.0303301,25.0303301 C24.7640635,25.2965966 24.3473998,25.3208027 24.0537883,25.1029482 L23.9696699,25.0303301 L17.1340957,18.1949905 C15.6118676,19.4773285 13.6461673,20.25 11.5,20.25 C6.66750844,20.25 2.75,16.3324916 2.75,11.5 C2.75,6.66750844 6.66750844,2.75 11.5,2.75 Z M11.5,4.25 C7.49593556,4.25 4.25,7.49593556 4.25,11.5 C4.25,15.5040644 7.49593556,18.75 11.5,18.75 C15.5040644,18.75 18.75,15.5040644 18.75,11.5 C18.75,7.49593556 15.5040644,4.25 11.5,4.25 Z");
    }
}

internal static class IconsResources
{
    internal static ConcurrentDictionary<string, StreamGeometry> _iconCache = new();

    internal const string Search = "M11.5,2.75 C16.3324916,2.75 20.25,6.66750844 20.25,11.5 C20.25,13.6461673 19.4773285,15.6118676 18.1949905,17.1340957 L25.0303301,23.9696699 C25.3232233,24.2625631 25.3232233,24.7374369 25.0303301,25.0303301 C24.7640635,25.2965966 24.3473998,25.3208027 24.0537883,25.1029482 L23.9696699,25.0303301 L17.1340957,18.1949905 C15.6118676,19.4773285 13.6461673,20.25 11.5,20.25 C6.66750844,20.25 2.75,16.3324916 2.75,11.5 C2.75,6.66750844 6.66750844,2.75 11.5,2.75 Z M11.5,4.25 C7.49593556,4.25 4.25,7.49593556 4.25,11.5 C4.25,15.5040644 7.49593556,18.75 11.5,18.75 C15.5040644,18.75 18.75,15.5040644 18.75,11.5 C18.75,7.49593556 15.5040644,4.25 11.5,4.25 Z";

    internal const string WindowMinimize = "M20,14H4V10H20";

    internal const string FileOpen = "M17.0606622,9 C17.8933043,9 18.7000032,9.27703406 19.3552116,9.78392956 L19.5300545,9.92783739 L22.116207,12.1907209 C22.306094,12.356872 22.5408581,12.4608817 22.7890575,12.4909364 L22.9393378,12.5 L40.25,12.5 C42.2542592,12.5 43.8912737,14.0723611 43.994802,16.0508414 L44,16.25 L44,35.25 C44,37.2542592 42.4276389,38.8912737 40.4491586,38.994802 L40.25,39 L7.75,39 C5.74574083,39 4.10872626,37.4276389 4.00519801,35.4491586 L4,35.25 L4,12.75 C4,10.7457408 5.57236105,9.10872626 7.55084143,9.00519801 L7.75,9 L17.0606622,9 Z M22.8474156,14.9988741 L20.7205012,17.6147223 C20.0558881,18.4327077 19.0802671,18.9305178 18.0350306,18.993257 L17.8100737,19 L6.5,18.999 L6.5,35.25 C6.5,35.8972087 6.99187466,36.4295339 7.62219476,36.4935464 L7.75,36.5 L40.25,36.5 C40.8972087,36.5 41.4295339,36.0081253 41.4935464,35.3778052 L41.5,35.25 L41.5,16.25 C41.5,15.6027913 41.0081253,15.0704661 40.3778052,15.0064536 L40.25,15 L22.8474156,14.9988741 Z M17.0606622,11.5 L7.75,11.5 C7.10279131,11.5 6.5704661,11.9918747 6.50645361,12.6221948 L6.5,12.75 L6.5,16.499 L17.8100737,16.5 C18.1394331,16.5 18.4534488,16.3701335 18.6858203,16.1419575 L18.7802162,16.0382408 L20.415,14.025 L17.883793,11.8092791 C17.693906,11.643128 17.4591419,11.5391183 17.2109425,11.5090636 L17.0606622,11.5 Z";

    internal const string WindowRestore = "M4,8H8V4H20V16H16V20H4V8M16,8V14H18V6H10V8H16M6,12V18H14V12H6Z";

    internal const string WindowMaximize = "M4,4H20V20H4V4M6,8V18H18V8H6Z";

    internal const string WindowClose = "M13.46,12L19,17.54V19H17.54L12,13.46L6.46,19H5V17.54L10.54,12L5,6.46V5H6.46L12,10.54L17.54,5H19V6.46L13.46,12Z";

    internal const string Check = "M21,7L9,19L3.5,13.5L4.91,12.09L9,16.17L19.59,5.59L21,7Z";

    internal const string Cross = "M19,6.41L17.59,5L12,10.59L6.41,5L5,6.41L10.59,12L5,17.59L6.41,19L12,13.41L17.59,19L19,17.59L13.41,12L19,6.41Z";

    internal const string Calendar = "M19,19H5V8H19M16,1V3H8V1H6V3H5C3.89,3 3,3.89 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5C21,3.89 20.1,3 19,3H18V1M17,12H12V17H17V12Z";

    internal const string Plus = "M19,13H13V19H11V13H5V11H11V5H13V11H19V13Z";

    internal const string Minus = "M19,13H5V11H19V13Z";

    internal const string Error = "M13 14H11V9H13M13 18H11V16H13M1 21H23L12 2L1 21Z";

    internal const string Login = "M10,17V14H3V10H10V7L15,12L10,17M10,2H19A2,2 0 0,1 21,4V20A2,2 0 0,1 19,22H10A2,2 0 0,1 8,20V18H10V20H19V4H10V6H8V4A2,2 0 0,1 10,2Z";

    internal const string ChevronUp = "M7.41,15.41L12,10.83L16.59,15.41L18,14L12,8L6,14L7.41,15.41Z";

    internal const string ChevronDown = "M7.41,8.58L12,13.17L16.59,8.58L18,10L12,16L6,10L7.41,8.58Z";

    internal const string ChevronLeft = "M15.41,16.58L10.83,12L15.41,7.41L14,6L8,12L14,18L15.41,16.58Z";

    internal const string ChevronRight = "M8.59,16.58L13.17,12L8.59,7.41L10,6L16,12L10,18L8.59,16.58Z";

    internal const string CircleCheck = "M12 2C6.5 2 2 6.5 2 12S6.5 22 12 22 22 17.5 22 12 17.5 2 12 2M10 17L5 12L6.41 10.59L10 14.17L17.59 6.58L19 8L10 17Z";

    internal const string CircleWarning = "M13,13H11V7H13M13,17H11V15H13M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";

    internal const string CircleInformation = "M13,9H11V7H13M13,17H11V11H13M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";

    internal const string CircleClose = "M12,2C17.53,2 22,6.47 22,12C22,17.53 17.53,22 12,22C6.47,22 2,17.53 2,12C2,6.47 6.47,2 12,2M15.59,7L12,10.59L8.41,7L7,8.41L10.59,12L7,15.59L8.41,17L12,13.41L15.59,17L17,15.59L13.41,12L17,8.41L15.59,7Z";

    internal const string CircleOutline = "M12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4A8,8 0 0,1 20,12A8,8 0 0,1 12,20M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z";

    internal const string CircleOutlineClose = "M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2C6.47,2 2,6.47 2,12C2,17.53 6.47,22 12,22C17.53,22 22,17.53 22,12C22,6.47 17.53,2 12,2M14.59,8L12,10.59L9.41,8L8,9.41L10.59,12L8,14.59L9.41,16L12,13.41L14.59,16L16,14.59L13.41,12L16,9.41L14.59,8Z";

    internal const string CircleOutlinePlus = "M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M13,7H11V11H7V13H11V17H13V13H17V11H13V7Z";

    internal const string CircleOutlineMinus = "M12,20C7.59,20 4,16.41 4,12C4,7.59 7.59,4 12,4C16.41,4 20,7.59 20,12C20,16.41 16.41,20 12,20M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M7,13H17V11H7";

    internal const string CircleOutlineCheck = "M12 2C6.5 2 2 6.5 2 12S6.5 22 12 22 22 17.5 22 12 17.5 2 12 2M12 20C7.59 20 4 16.41 4 12S7.59 4 12 4 20 7.59 20 12 16.41 20 12 20M16.59 7.58L10 14.17L7.41 11.59L6 13L10 17L18 9L16.59 7.58Z";

    internal const string KeyboardCaps = "M6,18H18V16H6M12,8.41L16.59,13L18,11.58L12,5.58L6,11.58L7.41,13L12,8.41Z";

    internal const string BackspaceOutline = "M19,15.59L17.59,17L14,13.41L10.41,17L9,15.59L12.59,12L9,8.41L10.41,7L14,10.59L17.59,7L19,8.41L15.41,12L19,15.59M22,3A2,2 0 0,1 24,5V19A2,2 0 0,1 22,21H7C6.31,21 5.77,20.64 5.41,20.11L0,12L5.41,3.88C5.77,3.35 6.31,3 7,3H22M22,5H7L2.28,12L7,19H22V5Z";

    internal const string ArrowLeft = "M20,11V13H8L13.5,18.5L12.08,19.92L4.16,12L12.08,4.08L13.5,5.5L8,11H20Z";

    internal const string ArrowRight = "M4,11V13H16L10.5,18.5L11.92,19.92L19.84,12L11.92,4.08L10.5,5.5L16,11H4Z";

    internal const string Menu = "M3,6H21V8H3V6M3,11H21V13H3V11M3,16H21V18H3V16Z";

    internal const string Star = "M12,17.27L18.18,21L16.54,13.97L22,9.24L14.81,8.62L12,2L9.19,8.62L2,9.24L7.45,13.97L5.82,21L12,17.27Z";

    internal const string InformationOutline = "M11 9H13V7H11V9M11 17H13V11H11V17Z";

    internal const string AlertOutline = "M 11,4L 13,4L 13,15L 11,15L 11,4 Z M 13,18L 13,20L 11,20L 11,18L 13,18 Z";

    internal const string RotateRight = "M16.89,15.5L18.31,16.89C19.21,15.73 19.76,14.39 19.93,13H17.91C17.77,13.87 17.43,14.72 16.89,15.5M13,17.9V19.92C14.39,19.75 15.74,19.21 16.9,18.31L15.46,16.87C14.71,17.41 13.87,17.76 13,17.9M19.93,11C19.76,9.61 19.21,8.27 18.31,7.11L16.89,8.53C17.43,9.28 17.77,10.13 17.91,11M15.55,5.55L11,1V4.07C7.06,4.56 4,7.92 4,12C4,16.08 7.05,19.44 11,19.93V17.91C8.16,17.43 6,14.97 6,12C6,9.03 8.16,6.57 11,6.09V10L15.55,5.55Z";
}
