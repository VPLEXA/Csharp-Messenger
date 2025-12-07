using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using MessengerClient.Models;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly StateManager _state;
        private readonly AppController _appController;
        private readonly NavigationService _navigation;
        private Window? _parentWindow;

        public ProfileViewModel(
            StateManager state, 
            AppController appController, 
            NavigationService navigation,
            Window? parentWindow = null)
        {
            _state = state;
            _appController = appController;
            _navigation = navigation;
            _parentWindow = parentWindow;

            // Инициализируем коллекцию статусов
            Statuses = new ObservableCollection<UserStatus>
            {
                UserStatus.Online,
                UserStatus.Offline,
                UserStatus.DoNotDisturb,
                UserStatus.Away
            };

            // Загружаем данные пользователя
            LoadUserData();

            // Инициализируем команды
            SaveCommand = new RelayCommand(async _ => await SaveProfileAsync());
            CancelCommand = new RelayCommand(_ => Cancel());
            ChangePasswordCommand = new RelayCommand(_ => NavigateToChangePassword());
            ChangeEmailCommand = new RelayCommand(_ => NavigateToChangeEmail());
            SelectAvatarCommand = new RelayCommand(async _ => await SelectAvatarAsync());
            RemoveAvatarCommand = new RelayCommand(_ => RemoveAvatar());
        }

        private void LoadUserData()
        {
            var user = _state.CurrentUser;
            if (user != null)
            {
                Username = user.Username;
                Email = user.Email;
                AvatarPath = user.Avatar;
                SelectedStatus = user.Status;
                Bio = user.Bio;
                IsOnline = user.IsOnline;
                
                // Загружаем настройки уведомлений
                if (user.NotificationSettings != null)
                {
                    NotificationsEnabled = user.NotificationSettings.Enabled;
                    SoundEnabled = user.NotificationSettings.SoundEnabled;
                    BannerOnly = user.NotificationSettings.BannerOnly;
                    SmartNotifications = user.NotificationSettings.SmartNotifications;
                }
            }
        }

        private string _username = "";
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        private string _email = "";
        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        private string _avatarPath = "";
        public string AvatarPath
        {
            get => _avatarPath;
            set => SetField(ref _avatarPath, value);
        }

        private UserStatus _selectedStatus = UserStatus.Online;
        public UserStatus SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SetField(ref _selectedStatus, value))
                {
                    // Автоматически обновляем IsOnline при изменении статуса
                    IsOnline = value == UserStatus.Online || value == UserStatus.Away;
                }
            }
        }

        private bool _isOnline = true;
        public bool IsOnline
        {
            get => _isOnline;
            set => SetField(ref _isOnline, value);
        }

        private string _bio = "";
        public string Bio
        {
            get => _bio;
            set => SetField(ref _bio, value);
        }

        public ObservableCollection<UserStatus> Statuses { get; }

        private bool _notificationsEnabled = true;
        public bool NotificationsEnabled
        {
            get => _notificationsEnabled;
            set => SetField(ref _notificationsEnabled, value);
        }

        private bool _soundEnabled = true;
        public bool SoundEnabled
        {
            get => _soundEnabled;
            set => SetField(ref _soundEnabled, value);
        }

        private bool _bannerOnly = false;
        public bool BannerOnly
        {
            get => _bannerOnly;
            set => SetField(ref _bannerOnly, value);
        }

        private bool _smartNotifications = true;
        public bool SmartNotifications
        {
            get => _smartNotifications;
            set => SetField(ref _smartNotifications, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand ChangeEmailCommand { get; }
        public ICommand SelectAvatarCommand { get; }
        public ICommand RemoveAvatarCommand { get; }

        private async Task SelectAvatarAsync()
        {
            if (_parentWindow == null) return;

            try
            {
                var files = await _parentWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Выберите аватар",
                    AllowMultiple = false,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Изображения")
                        {
                            Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.gif", "*.bmp" }
                        }
                    }
                });

                if (files.Count > 0 && files[0] != null)
                {
                    var file = files[0];
                    // Используем правильный путь в зависимости от типа файла
                    string path;
                    if (file.Path.IsAbsoluteUri)
                    {
                        path = file.Path.AbsolutePath;
                    }
                    else
                    {
                        path = file.Path.LocalPath;
                    }
                    
                    AvatarPath = path;
                    
                    // Сохраняем путь в профиле пользователя сразу
                    if (_state.CurrentUser != null)
                    {
                        _state.CurrentUser.Avatar = path;
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAsync($"Ошибка при выборе аватара: {ex.Message}");
            }
        }

        private void RemoveAvatar()
        {
            AvatarPath = "/Assets/default-avatar.png";
        }

        private async Task SaveProfileAsync()
        {
            try
            {
                if (_state.CurrentUser == null) return;

                // Обновляем данные пользователя
                _state.CurrentUser.Username = Username;
                _state.CurrentUser.Email = Email;
                _state.CurrentUser.Avatar = AvatarPath;
                _state.CurrentUser.Status = SelectedStatus;
                _state.CurrentUser.IsOnline = IsOnline;
                _state.CurrentUser.Bio = Bio;
                _state.CurrentUser.LastSeen = DateTime.Now;
                
                // Сохраняем настройки уведомлений
                if (_state.CurrentUser.NotificationSettings == null)
                {
                    _state.CurrentUser.NotificationSettings = new NotificationSettings();
                }
                _state.CurrentUser.NotificationSettings.Enabled = NotificationsEnabled;
                _state.CurrentUser.NotificationSettings.SoundEnabled = SoundEnabled;
                _state.CurrentUser.NotificationSettings.BannerOnly = BannerOnly;
                _state.CurrentUser.NotificationSettings.SmartNotifications = SmartNotifications;

                // Сохраняем через AppController
                var success = await _appController.UpdateUserProfileAsync(_state.CurrentUser);
                
                if (success)
                {
                    // Возвращаемся назад
                    _navigation.GoBack();
                    
                    // Можно показать уведомление об успешном сохранении
                    await ShowMessageAsync("Профиль успешно сохранен!");
                }
                else
                {
                    await ShowErrorAsync("Не удалось сохранить профиль");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAsync($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void Cancel()
        {
            _navigation.GoBack();
        }

        private void NavigateToChangePassword()
        {
            _navigation.NavigateToChangePassword();
        }

        private async void NavigateToChangeEmail()
        {
            // TODO: Реализовать ChangeEmailView
            await ShowMessageAsync("Функция смены email будет реализована");
        }

        private async Task ShowMessageAsync(string message)
        {
            if (_parentWindow != null)
            {
                await MessageBox.ShowAsync(_parentWindow, message, "Успех");
            }
        }

        private async Task ShowErrorAsync(string error)
        {
            if (_parentWindow != null)
            {
                await MessageBox.ShowAsync(_parentWindow, error, "Ошибка");
            }
        }
    }
}