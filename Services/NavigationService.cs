using System;
using Avalonia.Controls;
using Avalonia.Layout;
using MessengerClient.Models;
using MessengerClient.ViewModels;

namespace MessengerClient.Services
{
    public class NavigationService
    {
        private ContentControl _currentContent;
        private User _testUser;

        public NavigationService(ContentControl content)
        {
            _currentContent = content;
            _testUser = new User { Id = "1", Username = "TestUser" };
        }

        public void NavigateToLogin()
        {
            var loginVM = new LoginViewModel();
            loginVM.LoginSuccessful += NavigateToChatList;
            
            var loginView = new Views.LoginView();
            loginView.DataContext = loginVM;
            
            _currentContent.Content = loginView;
        }

        public void NavigateToChatList()
        {
            var chatListVM = new ChatListViewModel(_testUser, this);
            var chatListView = new Views.ChatListView();
            chatListView.DataContext = chatListVM;
            
            _currentContent.Content = chatListView;
        }

        public void NavigateToChat(Chat chat)
        {
            var chatVM = new ChatViewModel(chat, _testUser, this);
            var chatView = new Views.ChatView();
            chatView.DataContext = chatVM;
            
            _currentContent.Content = chatView;
        }
    }
}