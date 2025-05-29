using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NStyles;
using NStyles.Controls;

namespace AvaloniaApp.Views;

public partial class NWindowDemoWindow : NWindow
{
    public NWindowDemoWindow()
    {
        InitializeComponent();

        Theme.ColorTheme(NStyles.Models.SukiColor.Orange);
    }

    private NTheme Theme
    {
        get
        {
            var _theme = NTheme.GetInstance();
            return _theme;
        }
    }

    private void Button_ToggleBaseTheme_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Theme.SwitchBaseTheme();
    }

    private void Button_ToggleColorTheme_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Theme.SwitchColorTheme();
    }

    private void Grid_PointerEntered(object? sender, Avalonia.Input.PointerEventArgs e)
    {
    }

    private void Grid_PointerExited_1(object? sender, Avalonia.Input.PointerEventArgs e)
    {
    }

    private void Grid_PointerMoved_2(object? sender, Avalonia.Input.PointerEventArgs e)
    {
    }

    private void Button_Click_Loading(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        loading.IsVisible = !loading.IsVisible;
    }
}