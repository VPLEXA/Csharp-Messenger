using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private readonly StateManager _state;
        private readonly AppController _appController;
        private readonly NavigationService _navigation;
        private readonly Window? _parentWindow;

        private string _currentPassword = "";
        private string _newPassword = "";
        private string _confirmPassword = "";
        private string _errorMessage = "";
        private string _successMessage = "";
        private bool _isLoading = false;

        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetField(ref _currentPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetField(ref _newPassword, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetField(ref _confirmPassword, value);
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

        public ICommand ChangePasswordCommand { get; }
        public ICommand CancelCommand { get; }

        public ChangePasswordViewModel(StateManager state, AppController appController, NavigationService navigation, Window? parentWindow = null)
        {
            _state = state;
            _appController = appController;
            _navigation = navigation;
            _parentWindow = parentWindow;
            
            ChangePasswordCommand = new RelayCommand(async _ => await ChangePasswordAsync());
            CancelCommand = new RelayCommand(_ => _navigation.GoBack());
        }

        private async Task ChangePasswordAsync()
        {
            ErrorMessage = "";
            SuccessMessage = "";

            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                ErrorMessage = "Введите текущий пароль";
                return;
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                ErrorMessage = "Введите новый пароль";
                return;
            }

            if (NewPassword.Length < 6)
            {
                ErrorMessage = "Пароль должен содержать минимум 6 символов";
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Пароли не совпадают";
                return;
            }

            if (_state.CurrentUser == null)
            {
                ErrorMessage = "Пользователь не найден";
                return;
            }

            // Проверка текущего пароля (симуляция)
            if (_state.CurrentUser.Password != CurrentPassword && !string.IsNullOrEmpty(_state.CurrentUser.Password))
            {
                ErrorMessage = "Неверный текущий пароль";
                return;
            }

            IsLoading = true;

            try
            {
                // Симуляция смены пароля
                await Task.Delay(1000);
                
                _state.CurrentUser.Password = NewPassword;
                
                SuccessMessage = "Пароль успешно изменен!";
                ErrorMessage = "";
                
                // Возвращаемся назад через 2 секунды
                await Task.Delay(2000);
                _navigation.GoBack();
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
    }
}
