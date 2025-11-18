using Avalonia;
using Avalonia.iOS;

using Foundation;
using UIKit;

namespace NewBeeUI.Demo.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
[Register("AppDelegate")]
public partial class AppDelegate : AvaloniaAppDelegate<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder).AfterSetup(_ =>
        {
            // 设置背景色，不然会是黑色的，闪一下子
            if (this.Window != null)
                this.Window.BackgroundColor = UIColor.White;
        }).WithInterFont();
    }
}
