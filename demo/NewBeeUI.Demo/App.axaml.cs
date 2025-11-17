using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using NewBeeUI.Demo.Properties;
using NewBeeUI.Demo.Views;

namespace NewBeeUI.Demo;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        this.Styles.AddRange(GlobalStyles.BuildStyles());
    }

    public static bool MockMobileOnDesktop { get; set; } = false;

    public static bool IsMobileApp { get; private set; }

    public static bool IsMobileLayout { get => IsMobileApp || MockMobileOnDesktop; }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
#if DEBUG
            this.AttachDevTools();
#endif

            if(MockMobileOnDesktop)
            {
                new MobileMainView().ShowDialog();
            }
            else
            {
                new MainView().ShowDialog();
            }
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            IsMobileApp = true;
            singleViewPlatform.MainView = new HostView(new MobileMainView());
        }

        base.OnFrameworkInitializationCompleted();
    }
}
