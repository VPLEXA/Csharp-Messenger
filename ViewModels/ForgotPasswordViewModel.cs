using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        private readonly NavigationService _navigation;
        private readonly Window? _parentWindow;

        private string _email = "";
        private string _errorMessage = "";
        private string _successMessage = "";
        private bool _isLoading = false;

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set => SetField(ref _successMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        public ICommand SendResetLinkCommand { get; }
        public ICommand BackToLoginCommand { get; }

        public ForgotPasswordViewModel(NavigationService navigation, Window? parentWindow = null)
        {
            _navigation = navigation;
            _parentWindow = parentWindow;
            SendResetLinkCommand = new RelayCommand(async _ => await SendResetLinkAsync());
            BackToLoginCommand = new RelayCommand(_ => _navigation.NavigateToLogin());
        }

        private async Task SendResetLinkAsync()
        {
            ErrorMessage = "";
            SuccessMessage = "";

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Введите email адрес";
                return;
            }

            if (!IsValidEmail(Email))
            {
                ErrorMessage = "Некорректный email адрес";
                return;
            }

            IsLoading = true;

            try
            {
                // Симуляция отправки письма
                await Task.Delay(1500);
                
                SuccessMessage = $"Инструкции по восстановлению пароля отправлены на {Email}";
                ErrorMessage = "";
                
                // В реальном приложении здесь будет API вызов
                Console.WriteLine($"Password reset requested for: {Email}");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
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

