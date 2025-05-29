using CommunityToolkit.Mvvm.ComponentModel;
using NStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApp.ViewModels;

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class PersonTemplateSelector : BaseTemplateSelector
{
    protected override string? GetKey(object? data)
    {
        if (data == null) return "Person_PlaceHolder";
        var m = data as Person;
        if (m == null) return "Person_PlaceHolder";
        else return "Person_Normal";
    }
}

public partial class TemplateSelectorViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<Person?> _users = new List<Person?>();

    [ObservableProperty]
    private Person? _user = new Person() { Name = "王五" };

    [RelayCommand]
    private void foo()
    {
        // Do something
    }
}
