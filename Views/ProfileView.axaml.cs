using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using System;
using MessengerClient.Services;
using Avalonia.VisualTree;

namespace MessengerClient.Views
{
    
    public partial class ProfileView : UserControl
    {
        private bool _isEditing = false;

        // Элементы для редактирования (nullable для устранения предупреждений)
        private TextBox? _nameTextBox;
        private TextBox? _emailTextBox;
        private ComboBox? _statusComboBox;
        private TextBox? _bioTextBox;
        private Border? _avatarBorder;
        private TextBlock? _avatarText;
        private Button? _saveBtn;
        private Button? _cancelBtn;
        private Button? _editBtn;
        private Button? _changePasswordBtn;
        private Button? _settingsBtn;
        private Button? _backBtn;

        public ProfileView()
            {
                CreateUI();
            }

        private void CreateUI()
        {
            var stackPanel = new StackPanel
            {
                Spacing = 20,
                Margin = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Заголовок
            var title = new TextBlock
            {
                Text = "👤 МОЙ ПРОФИЛЬ",
                FontSize = 24,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.Parse("#2b579a"))
            };
            stackPanel.Children.Add(title);

            // Контейнер для аватара и кнопки загрузки
            var avatarContainer = new StackPanel
            {
                Spacing = 10,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Аватар
            _avatarBorder = new Border
            {
                Width = 120,
                Height = 120,
                Background = new SolidColorBrush(Color.Parse("#3498db")),
                CornerRadius = new CornerRadius(60),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            _avatarText = new TextBlock
            {
                Text = "U",
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                FontSize = 48,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            _avatarBorder.Child = _avatarText;
            avatarContainer.Children.Add(_avatarBorder);

            // Кнопка загрузки аватара
            var uploadAvatarBtn = CreateButton("📷 Изменить фото", "#9b59b6");
            uploadAvatarBtn.Width = 200;
            uploadAvatarBtn.Click += OnUploadAvatarClicked!;
            avatarContainer.Children.Add(uploadAvatarBtn);

            stackPanel.Children.Add(avatarContainer);

            // Информация
            var infoGrid = new Grid();
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Имя
            _nameTextBox = new TextBox();
            AddInfoRow(infoGrid, 0, "Имя:", "Тест Пользователь", _nameTextBox);

            // Email
            _emailTextBox = new TextBox();
            AddInfoRow(infoGrid, 1, "Email:", "test@example.com", _emailTextBox);

            // Статус
            var statusLabel = new TextBlock
            {
                Text = "Статус:",
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 5),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(statusLabel, 2);
            Grid.SetColumn(statusLabel, 0);
            infoGrid.Children.Add(statusLabel);

            _statusComboBox = new ComboBox
            {
                Margin = new Thickness(10, 5, 0, 5),
                VerticalAlignment = VerticalAlignment.Center,
                Items = { "🟢 Онлайн", "⚪ Офлайн", "🔕 Не беспокоить" },
                SelectedIndex = 0
            };
            Grid.SetRow(_statusComboBox, 2);
            Grid.SetColumn(_statusComboBox, 1);
            infoGrid.Children.Add(_statusComboBox);

            // Био
            var bioLabel = new TextBlock
            {
                Text = "О себе:",
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 5),
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetRow(bioLabel, 3);
            Grid.SetColumn(bioLabel, 0);
            infoGrid.Children.Add(bioLabel);

            _bioTextBox = new TextBox
            {
                Margin = new Thickness(10, 5, 0, 5),
                VerticalAlignment = VerticalAlignment.Top,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.Wrap,
                Height = 80,
                Text = "Привет! Я использую Messenger"
            };
            Grid.SetRow(_bioTextBox, 3);
            Grid.SetColumn(_bioTextBox, 1);
            infoGrid.Children.Add(_bioTextBox);

            stackPanel.Children.Add(infoGrid);

            // Кнопки
            var buttonsStack = new StackPanel
            {
                Spacing = 10,
                Margin = new Thickness(0, 30, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Кнопка Редактировать
            _editBtn = CreateButton("✏️ Редактировать", "#2b579a");
            _editBtn.Click += OnEditClicked!;
            buttonsStack.Children.Add(_editBtn);

            // Кнопка Сохранить (скрыта по умолчанию)
            _saveBtn = CreateButton("💾 Сохранить", "#27ae60");
            _saveBtn.Click += OnSaveClicked!;
            _saveBtn.IsVisible = false;
            buttonsStack.Children.Add(_saveBtn);

            // Кнопка Отмена (скрыта по умолчанию)
            _cancelBtn = CreateButton("❌ Отмена", "#e74c3c");
            _cancelBtn.Click += OnCancelClicked!;
            _cancelBtn.IsVisible = false;
            buttonsStack.Children.Add(_cancelBtn);

            // Кнопка Сменить пароль
            _changePasswordBtn = CreateButton("🔒 Сменить пароль", "#3498db");
            _changePasswordBtn.Click += OnChangePasswordClicked!;
            buttonsStack.Children.Add(_changePasswordBtn);

            // Кнопка Настройки
            _settingsBtn = CreateButton("⚙️ Настройки", "#7f8c8d");
            _settingsBtn.Click += (s, e) => _controller.ShowSettings();
            buttonsStack.Children.Add(_settingsBtn);

            // Кнопка Назад
            _backBtn = CreateButton("← Назад к чатам", "#95a5a6");
            _backBtn.Click += (s, e) => _controller.ShowChatList();
            buttonsStack.Children.Add(_backBtn);

            stackPanel.Children.Add(buttonsStack);

            this.Content = stackPanel;

            // Загружаем данные профиля
            LoadProfileData();
        }

        private void AddInfoRow(Grid grid, int row, string label, string defaultValue, TextBox textBox)
        {
            // Label
            var labelText = new TextBlock
            {
                Text = label,
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 5),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(labelText, row);
            Grid.SetColumn(labelText, 0);
            grid.Children.Add(labelText);

            // Value
            textBox.Margin = new Thickness(10, 5, 0, 5);
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.Text = defaultValue;
            textBox.IsReadOnly = true;

            Grid.SetRow(textBox, row);
            Grid.SetColumn(textBox, 1);
            grid.Children.Add(textBox);
        }

        private Button CreateButton(string text, string color)
        {
            return new Button
            {
                Content = text,
                Width = 250,
                Height = 45,
                Background = new SolidColorBrush(Color.Parse(color)),
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }

        private void LoadProfileData()
        {
            try
            {
                // Временная реализация - можно доработать после добавления методов в AppController
                var profileData = new UserProfileData();

                if (_nameTextBox != null) _nameTextBox.Text = profileData.Name;
                if (_emailTextBox != null) _emailTextBox.Text = profileData.Email;
                if (_statusComboBox != null) _statusComboBox.SelectedIndex = profileData.Status;
                if (_bioTextBox != null) _bioTextBox.Text = profileData.Bio;

                // Обновляем аватар
                if (_avatarText != null && !string.IsNullOrEmpty(profileData.AvatarInitial))
                {
                    _avatarText.Text = profileData.AvatarInitial;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки профиля: {ex.Message}");
            }
        }

        private void OnEditClicked(object sender, RoutedEventArgs e)
        {
            _isEditing = true;
            UpdateEditMode();
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            if (_nameTextBox == null || _emailTextBox == null || _statusComboBox == null || _bioTextBox == null || _avatarText == null)
                return;

            // Сохраняем данные профиля
            var profileData = new UserProfileData
            {
                Name = _nameTextBox.Text,
                Email = _emailTextBox.Text,
                Status = _statusComboBox.SelectedIndex,
                Bio = _bioTextBox.Text,
                AvatarInitial = _avatarText.Text
            };

            // TODO: Вызвать метод сохранения в AppController после его реализации
            // _controller?.SaveProfileData(profileData);
            System.Diagnostics.Debug.WriteLine($"Профиль сохранен: {profileData.Name}");

            _isEditing = false;
            UpdateEditMode();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            // Восстанавливаем исходные данные
            LoadProfileData();

            _isEditing = false;
            UpdateEditMode();
        }

        private void OnChangePasswordClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Реализовать окно смены пароля
            System.Diagnostics.Debug.WriteLine("Смена пароля");
        }

        private void OnUploadAvatarClicked(object sender, RoutedEventArgs e)
        {
            if (_avatarText == null) return;

            // Временная реализация - изменение инициалов
            if (_nameTextBox != null && !string.IsNullOrEmpty(_nameTextBox.Text))
            {
                _avatarText.Text = _nameTextBox.Text[0].ToString();
            }

            System.Diagnostics.Debug.WriteLine("Загрузка аватара");
        }

        private void UpdateEditMode()
        {
            bool isEditing = _isEditing;

            // Переключаем режим редактирования полей
            if (_nameTextBox != null) _nameTextBox.IsReadOnly = !isEditing;
            if (_emailTextBox != null) _emailTextBox.IsReadOnly = !isEditing;
            if (_statusComboBox != null) _statusComboBox.IsEnabled = isEditing;
            if (_bioTextBox != null) _bioTextBox.IsReadOnly = !isEditing;

            // Показываем/скрываем кнопки
            if (_editBtn != null) _editBtn.IsVisible = !isEditing;
            if (_saveBtn != null) _saveBtn.IsVisible = isEditing;
            if (_cancelBtn != null) _cancelBtn.IsVisible = isEditing;
            if (_changePasswordBtn != null) _changePasswordBtn.IsVisible = !isEditing;
            if (_settingsBtn != null) _settingsBtn.IsVisible = !isEditing;
            if (_backBtn != null) _backBtn.IsVisible = !isEditing;

            // Визуальные изменения для режима редактирования
            var editColor = isEditing ? "#f1c40f" : "#ffffff";
            var backgroundColor = new SolidColorBrush(Color.Parse(editColor));

            if (isEditing)
            {
                if (_nameTextBox != null) _nameTextBox.Background = backgroundColor;
                if (_emailTextBox != null) _emailTextBox.Background = backgroundColor;
                if (_bioTextBox != null) _bioTextBox.Background = backgroundColor;
            }
            else
            {
                if (_nameTextBox != null) _nameTextBox.Background = Brushes.Transparent;
                if (_emailTextBox != null) _emailTextBox.Background = Brushes.Transparent;
                if (_bioTextBox != null) _bioTextBox.Background = Brushes.Transparent;
            }
        }
    }
    
    // Класс для хранения данных профиля
    public class UserProfileData
    {
        public string Name { get; set; } = "Тест Пользователь";
        public string Email { get; set; } = "test@example.com";
        public int Status { get; set; } = 0; // 0-онлайн, 1-офлайн, 2-не беспокоить
        public string Bio { get; set; } = "Привет! Я использую Messenger";
        public string AvatarInitial { get; set; } = "T";
    }
}