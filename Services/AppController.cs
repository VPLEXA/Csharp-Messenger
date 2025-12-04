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
            _mainWindow.Content = new LoginView();
        }
        
        public void ShowRegister()
        {
            _mainWindow.Content = new RegisterView();
        }
        
        public void ShowChatList()
        {
            _mainWindow.Content = new ChatListView();
        }
        
        public void ShowChat(Chat chat)
        {
            var chatView = new ChatView();
            _mainWindow.Content = chatView;
        }
        
        public void ShowProfile()
        {
            _mainWindow.Content = new ProfileView();
        }
        
        public void ShowSettings()
        {
            _mainWindow.Content = new SettingsView();
        }
        
        public void Logout()
        {
            CurrentUser = null;
            ShowLogin();
        }
    }
}