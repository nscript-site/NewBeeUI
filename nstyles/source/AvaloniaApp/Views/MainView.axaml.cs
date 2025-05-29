using Avalonia.Controls;

namespace AvaloniaApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Button_BaseTemplateSelector_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = new TemplateSelectorWindow();
        window.Show();
    }

    private void Button_NWindow_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = new NWindowDemoWindow();
        window.Show();
    }

    private void StackPanel_PointerEntered(object? sender, Avalonia.Input.PointerEventArgs e)
    {
    }

    private void StackPanel_PointerExited_1(object? sender, Avalonia.Input.PointerEventArgs e)
    {
    }
}
