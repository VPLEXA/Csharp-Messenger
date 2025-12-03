using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MessengerClient.Services;

namespace MessengerClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new Window
                {
                    Title = "Messenger",
                    Width = 400,
                    Height = 600,
                    MinWidth = 400,
                    MinHeight = 500
                };
                
                var controller = new AppController(mainWindow);
                controller.ShowLogin();
                
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}