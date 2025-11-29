using System;
using System.Windows.Input;
using MessengerClient.Models;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly StateManager _stateManager;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        public event Action? LoginSuccessful;

        public LoginViewModel(StateManager stateManager)
        {
            _stateManager = stateManager;
            LoginCommand = new RelayCommand(Login);
        }

        public void Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter username and password";
                return;
            }

            // Временная логика - потом заменим на реальную авторизацию
            _stateManager.CurrentUser = new User
            {
                Id = "1",
                Username = Username,
                Email = $"{Username}@example.com"
            };

            ErrorMessage = string.Empty;
            LoginSuccessful?.Invoke();
        }
    }
}