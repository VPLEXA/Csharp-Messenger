using System;
using Avalonia.Controls;
using MessengerClient.Models;
using MessengerClient.ViewModels;
using MessengerClient.Views;

namespace MessengerClient.Services
{
    public class NavigationService
    {
        private readonly StateManager _state;
        private readonly AppController _appController;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private Window? _mainWindow;

        // Упрощенный конструктор без mainWindowViewModel
        public NavigationService(
            StateManager state, 
            AppController appController,
            Window mainWindow)
        {
            _state = state;
            _appController = appController;
            _mainWindow = mainWindow;
            
            // Получаем MainWindowViewModel из DataContext окна
            _mainWindowViewModel = mainWindow.DataContext as MainWindowViewModel 
                ?? new MainWindowViewModel();
        }

        public NavigationService(
            StateManager state, 
            AppController appController, 
            MainWindowViewModel mainWindowViewModel,
            Window? mainWindow = null) : this(state, appController, mainWindow ?? throw new ArgumentNullException(nameof(mainWindow)))
        {
        }

        public object? CurrentViewModel
        {
            get => _mainWindowViewModel.CurrentViewModel;
            set
            {
                _mainWindowViewModel.CurrentViewModel = value;
                OnCurrentViewModelChanged?.Invoke(value);
            }
        }

        public event Action<object?>? OnCurrentViewModelChanged;

        public void NavigateToProfile()
        {
            CurrentViewModel = new ProfileView
            {
                DataContext = new ProfileViewModel(_state, _appController, this, _mainWindow)
            };
        }

        public void NavigateToChangePassword()
        {
            CurrentViewModel = new Views.ChangePasswordView
            {
                DataContext = new ViewModels.ChangePasswordViewModel(_state, _appController, this, _mainWindow)
            };
        }

        public void NavigateToForgotPassword()
        {
            CurrentViewModel = new Views.ForgotPasswordView
            {
                DataContext = new ViewModels.ForgotPasswordViewModel(this, _mainWindow)
            };
        }

        public void NavigateToChat(Chat chat)
        {
            if (_state.CurrentUser == null)
                throw new Exception("CurrentUser is null");

            CurrentViewModel = new Views.ChatView
            {
                DataContext = new ChatViewModel(chat, _state.CurrentUser, this, _mainWindow)
            };
        }

        public void NavigateToChatList()
        {
            if (_state.CurrentUser == null)
                throw new Exception("CurrentUser is null");

            CurrentViewModel = new Views.ChatListView
            {
                DataContext = new ChatListViewModel(_state.CurrentUser, this)
            };
        }

        public void NavigateToLogin()
        {
            CurrentViewModel = new Views.LoginView
            {
                DataContext = new LoginViewModel(this)
            };
        }

        public void GoBack()
        {
            // Простая навигация назад - возвращаемся к списку чатов
            NavigateToChatList();
        }

        public StateManager GetStateManager() => _state;
    }
}