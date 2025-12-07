using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly NavigationService? _navigation;

        private string _email = "";
        private string _password = "";
        private string _errorMessage = "";

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
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
        public ICommand ForgotPasswordCommand { get; }
        public ICommand GoogleLoginCommand { get; }
        public ICommand AppleLoginCommand { get; }

        public LoginViewModel(NavigationService navigation)
        {
            _navigation = navigation;
            LoginCommand = new RelayCommand(_ => Login());
            ForgotPasswordCommand = new RelayCommand(_ => NavigateToForgotPassword());
            GoogleLoginCommand = new RelayCommand(_ => LoginWithGoogle());
            AppleLoginCommand = new RelayCommand(_ => LoginWithApple());
        }

        private void Login()
        {
            ErrorMessage = "";

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Введите email и пароль";
                return;
            }

            if (!IsValidEmail(Email))
            {
                ErrorMessage = "Некорректный email адрес";
                return;
            }

            Console.WriteLine($"Logging in: {Email}");
            
            // Симуляция проверки пароля
            // В реальном приложении здесь будет запрос к серверу
            if (_navigation != null)
            {
                var stateManager = _navigation.GetStateManager();
                stateManager.CurrentUser = new Models.User
                {
                    Username = Email.Split('@')[0],
                    Email = Email,
                    Password = Password,
                    IsOnline = true,
                    Status = Models.UserStatus.Online
                };
            }
            
            _navigation?.NavigateToChatList();
        }

        private void NavigateToForgotPassword()
        {
            _navigation?.NavigateToForgotPassword();
        }

        private void LoginWithGoogle()
        {
            ErrorMessage = "";
            Console.WriteLine("Google login clicked");
            // Симуляция входа через Google
            if (_navigation != null)
            {
                var stateManager = _navigation.GetStateManager();
                stateManager.CurrentUser = new Models.User
                {
                    Username = "google_user",
                    Email = "user@gmail.com",
                    IsOnline = true,
                    Status = Models.UserStatus.Online
                };
            }
            _navigation?.NavigateToChatList();
        }

        private void LoginWithApple()
        {
            ErrorMessage = "";
            Console.WriteLine("Apple login clicked");
            // Симуляция входа через Apple
            if (_navigation != null)
            {
                var stateManager = _navigation.GetStateManager();
                stateManager.CurrentUser = new Models.User
                {
                    Username = "apple_user",
                    Email = "user@icloud.com",
                    IsOnline = true,
                    Status = Models.UserStatus.Online
                };
            }
            _navigation?.NavigateToChatList();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}