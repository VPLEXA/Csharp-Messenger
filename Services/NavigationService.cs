using System;
using Avalonia.Controls;
using MessengerClient.ViewModels;

namespace MessengerClient.Services
{
    public class NavigationService
    {
        private ContentControl _currentContent;

        public NavigationService(ContentControl content)
        {
            _currentContent = content;
        }

        public void NavigateToLogin()
        {
            var loginVM = new LoginViewModel();
            loginVM.LoginSuccessful += () => NavigateToChatList();
            
            var loginView = new Views.LoginView();
            loginView.DataContext = loginVM;
            
            _currentContent.Content = loginView;
        }

        public void NavigateToChatList()
        {
            // Заглушка - завтра заменим на реальный ChatListView
            _currentContent.Content = new TextBlock 
            { 
                Text = "SUCCESS! Chat List Coming Tomorrow!", 
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                FontSize = 16
            };
        }
    }
}