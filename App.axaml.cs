using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MessengerClient.Views;
using MessengerClient.Services;
using MessengerClient.Models;

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
                // Сразу показываем ChatListView для тестирования
                var mainWindow = new MainWindow();
                var navigationService = new NavigationService(mainWindow);
                
                // Создаем тестового пользователя
                var testUser = new User { Id = "1", Username = "TestUser" };
                
                // Создаем ViewModel и View
                var chatListVM = new ViewModels.ChatListViewModel(testUser, navigationService);
                var chatListView = new ChatListView();
                chatListView.DataContext = chatListVM;
                
                mainWindow.Content = chatListView;
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}