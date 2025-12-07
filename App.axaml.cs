using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MessengerClient.Services;
using MessengerClient.ViewModels;
using System;
using System.Net.Http;

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
                // Создаем сервисы
                var stateManager = new StateManager();
                var httpClient = new HttpClient();
                var appController = new AppController(httpClient, stateManager);
                
                // Создаем ViewModel главного окна
                var mainWindowViewModel = new MainWindowViewModel();
                
                // Создаем главное окно
                var mainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel
                };
                
                // Создаем NavigationService после создания окна
                var navigationService = new NavigationService(
                    stateManager, 
                    appController, 
                    mainWindowViewModel,
                    mainWindow); // передаем mainWindow как parent
                
                // Устанавливаем начальный View
                mainWindowViewModel.CurrentViewModel = new Views.LoginView
                {
                    DataContext = new LoginViewModel(navigationService)
                };
                
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}