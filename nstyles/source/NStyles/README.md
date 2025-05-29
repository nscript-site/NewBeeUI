# navalonia

提供 Avalonia 的一些辅助类和扩展功能，及一套裁剪自 [SukiUI](https://github.com/kikipoulet/SukiUI) 的主题。

## BaseTemplateSelector

简化了模板选择器的实现。详细使用：

```csharp
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
}
```

```xaml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="450"
        x:Class="AvaloniaApp.TemplateSelectorWindow"
        xmlns:vm="clr-namespace:AvaloniaApp.ViewModels"
        x:DataType="vm:TemplateSelectorViewModel" Width="600" Height="400"
        Title="TemplateSelectorWindow">
  <ItemsControl VerticalAlignment="Center" ItemsSource="{Binding Users}">
    <ItemsControl.ItemsPanel>
      <ItemsPanelTemplate>
        <WrapPanel HorizontalAlignment="Center" ScrollViewer.VerticalScrollBarVisibility="Auto">
        </WrapPanel>
      </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
    <ItemsControl.ItemTemplate>
      <vm:PersonTemplateSelector>
        <DataTemplate x:Key="Person_PlaceHolder" x:DataType="vm:Person">
          <Border Background="LightGray" Padding="10" Margin="5" Width="100" Height="100" 
                  HorizontalAlignment="Center" VerticalAlignment="Center">
            <TextBlock Width="20" Height="20" LineHeight="20" FontSize="32" Text="+"></TextBlock>
          </Border>
        </DataTemplate>
        <DataTemplate x:Key="Person_Normal" x:DataType="vm:Person">
          <Border Background="LightGray" Padding="10" Margin="5" Width="100" Height="100">
            <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
              <TextBlock Text="{Binding Name}"></TextBlock>
            </StackPanel>
          </Border>
        </DataTemplate>
      </vm:PersonTemplateSelector>
    </ItemsControl.ItemTemplate>
  </ItemsControl>
</Window>
```

```csharp
public partial class TemplateSelectorWindow : Window
{
    public TemplateSelectorWindow()
    {
        InitializeComponent();
        var vm = new ViewModels.TemplateSelectorViewModel();
        vm.Users.Add(null);
        vm.Users.Add(new ViewModels.Person() { Name = "张三", Age = 30 });
        vm.Users.Add(new ViewModels.Person() { Name = "李四", Age = 30 });
        this.DataContext = vm;
    }
}
```

## NWindow

NWindow 精简了 Skui 的 SkuiWindow 实现，方便自定义带样式的窗口。使用时先添加主题：

```xaml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="AvaloniaApp.App"
             xmlns:ns="nstyles"
             RequestedThemeVariant="Default">
    <Application.Styles>
        <FluentTheme />
        <!-- 添加 NScript 主题 -->
        <ns:NTheme />
    </Application.Styles>
</Application>
```

