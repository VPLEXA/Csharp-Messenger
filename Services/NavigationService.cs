using System;
using Avalonia.Controls;
using MessengerClient.ViewModels;

namespace MessengerClient.Services
{
    public class NavigationService
    {
        private ContentControl _currentContent;
        private MainWindowViewModel _mainViewModel;

        public NavigationService(ContentControl content, MainWindowViewModel mainViewModel)
        {
            _currentContent = content;
            _mainViewModel = mainViewModel;
        }

        public void NavigateToLogin()
        {
            var loginVM = new LoginViewModel(_mainViewModel.StateManager);
            loginVM.LoginSuccessful += () => NavigateToChatList();
            _currentContent.Content = new Views.LoginView { DataContext = loginVM };
        }

        public void NavigateToChatList()
        {
            // Заглушка - завтра заменим на реальный ChatListView
            _currentContent.Content = new TextBlock 
            { 
                Text = "Chat List - Coming Tomorrow!", 
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
        }
    }
}