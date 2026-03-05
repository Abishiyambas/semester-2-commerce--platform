using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SHOP.ViewModels;

namespace SHOP;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        base.OnFrameworkInitializationCompleted();

        // Set up the MainWindow
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {

            desktop.MainWindow = new MainView
            {
                DataContext = new MainViewModel() // This sets out DataContext in the backend instead of in the XAML
            };

            // You no longer need to load the logo here
            // All logo loading is handled in MainWindow
        }
    }
}