using System;
using Avalonia.Controls;
using MessengerClient.Models;
using MessengerClient.Views;

namespace MessengerClient.Services
{
    public class AppController
    {
        private readonly Window _mainWindow;
        private User? _currentUser;
        
        public User? CurrentUser 
        { 
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnUserChanged?.Invoke(value);
            }
        }
        
        public event Action<User?>? OnUserChanged;
        
        public AppController(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }
        
        public void ShowLogin()
        {
            var loginView = new LoginView(this);
            _mainWindow.Content = loginView;
        }
        
        public void ShowChatList()
        {
            var chatListView = new ChatListView(this);
            _mainWindow.Content = chatListView;
        }
        
        public void ShowChat(Chat chat)
        {
            var chatView = new ChatView(this, chat);
            _mainWindow.Content = chatView;
        }
        
        public void ShowProfile()
        {
            var profileView = new ProfileView(this);
            _mainWindow.Content = profileView;
        }
        
        public void Logout()
        {
            CurrentUser = null;
            ShowLogin();
        }
    }
}