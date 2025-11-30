using System;
using System.Windows.Input;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username = "test_user";
        private string _password = "password123";
        private string _errorMessage = "";

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

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            Console.WriteLine("LOGIN CLICKED!");
            
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Please enter username and password";
                return;
            }

            Console.WriteLine($"Logging in: {Username}");
            LoginSuccessful?.Invoke();
        }
    }
}