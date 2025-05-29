using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged.SourceGenerator;

namespace AvaloniaApp;

public partial class TemplateSelectorWindow : Window
{
    public TemplateSelectorWindow()
    {
        InitializeComponent();
        var vm = new ViewModels.TemplateSelectorViewModel();
        vm.Users.Add(null);
        vm.Users.Add(new ViewModels.Person() { Name = "张三", Age = 30 });
        vm.Users.Add(new ViewModels.Person() { Name = "李四", Age = 30 });
        vm.User = new ViewModels.Person() { Name = "王五", Age = 30 };
        this.DataContext = vm;
    }

    [Notify]
    private string _myTitle = "就是这么任性";
}