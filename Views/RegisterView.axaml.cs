using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using MessengerClient.Services;

namespace MessengerClient.Views
{
    public class RegisterView : UserControl
    {
        private readonly AuthService _authService;
        
        private TextBox _usernameBox = null!;
        private TextBox _emailBox = null!;
        private TextBox _passwordBox = null!;
        private TextBox _confirmPasswordBox = null!;
        private TextBlock _errorText = null!;
        
        public RegisterView()
        {
            _authService = new AuthService();
            CreateUI();
        }
        
        private void CreateUI()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 15,
                Width = 350,
                Background = new SolidColorBrush(Color.Parse("#f0f0f0")),
                Margin = new Thickness(20)
            };

            // Заголовок
            var title = new TextBlock
            {
                Text = "Регистрация",
                FontSize = 24,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = new SolidColorBrush(Color.Parse("#2b579a"))
            };
            stackPanel.Children.Add(title);

            // Username
            var usernameLabel = new TextBlock
            {
                Text = "Имя пользователя:",
                FontWeight = FontWeight.SemiBold
            };
            stackPanel.Children.Add(usernameLabel);
            
            _usernameBox = new TextBox
            {
                Height = 40,
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(10, 0),
                FontSize = 14
            };
            _usernameBox.TextChanged += OnFieldChanged;
            stackPanel.Children.Add(_usernameBox);

            // Email
            var emailLabel = new TextBlock
            {
                Text = "Email:",
                FontWeight = FontWeight.SemiBold,
                Margin = new Thickness(0, 10, 0, 0)
            };
            stackPanel.Children.Add(emailLabel);
            
            _emailBox = new TextBox
            {
                Height = 40,
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(10, 0),
                FontSize = 14
            };
            _emailBox.TextChanged += OnFieldChanged;
            stackPanel.Children.Add(_emailBox);

            // Password
            var passwordLabel = new TextBlock
            {
                Text = "Пароль:",
                FontWeight = FontWeight.SemiBold,
                Margin = new Thickness(0, 10, 0, 0)
            };
            stackPanel.Children.Add(passwordLabel);
            
            _passwordBox = new TextBox
            {
                Height = 40,
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(10, 0),
                FontSize = 14,
                PasswordChar = '•'
            };
            _passwordBox.TextChanged += OnFieldChanged;
            stackPanel.Children.Add(_passwordBox);

            // Confirm Password
            var confirmLabel = new TextBlock
            {
                Text = "Подтвердите пароль:",
                FontWeight = FontWeight.SemiBold,
                Margin = new Thickness(0, 10, 0, 0)
            };
            stackPanel.Children.Add(confirmLabel);
            
            _confirmPasswordBox = new TextBox
            {
                Height = 40,
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(10, 0),
                FontSize = 14,
                PasswordChar = '•'
            };
            _confirmPasswordBox.TextChanged += OnFieldChanged;
            stackPanel.Children.Add(_confirmPasswordBox);

            // Кнопка Register
            var registerBtn = CreateButton("Зарегистрироваться", "#27ae60");
            registerBtn.Click += OnRegisterClicked;
            stackPanel.Children.Add(registerBtn);

            // Кнопка Back to Login
            var loginBtn = CreateButton("← Назад к входу", "#95a5a6");
            loginBtn.Click += (s, e) =>
            {
                var window = this.FindAncestorOfType<Window>();
                if (window?.DataContext is AppController controller)
                {
                    controller.ShowLogin();
                }
            };
            stackPanel.Children.Add(loginBtn);

            // Текст ошибки
            _errorText = new TextBlock
            {
                Foreground = Brushes.Red,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = 330,
                Margin = new Thickness(0, 10)
            };
            stackPanel.Children.Add(_errorText);

            // Правила
            var rules = new TextBlock
            {
                Text = "Требования:\n• Имя пользователя: 3-20 символов\n• Email должен быть валидным\n• Пароль: минимум 6 символов",
                Foreground = new SolidColorBrush(Color.Parse("#666")),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 10)
            };
            stackPanel.Children.Add(rules);

            this.Content = stackPanel;
        }
        
        private void OnFieldChanged(object sender, TextChangedEventArgs e)
        {
            _errorText.Text = "";
        }
        
        private void OnRegisterClicked(object sender, RoutedEventArgs e)
        {
            var username = _usernameBox.Text.Trim();
            var email = _emailBox.Text.Trim();
            var password = _passwordBox.Text;
            var confirmPassword = _confirmPasswordBox.Text;
            
            // Валидация
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || 
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                _errorText.Text = "Все поля обязательны для заполнения";
                return;
            }
            
            if (username.Length < 3 || username.Length > 20)
            {
                _errorText.Text = "Имя пользователя должно быть от 3 до 20 символов";
                return;
            }
            
            if (!email.Contains("@") || !email.Contains("."))
            {
                _errorText.Text = "Введите корректный email";
                return;
            }
            
            if (password.Length < 6)
            {
                _errorText.Text = "Пароль должен содержать минимум 6 символов";
                return;
            }
            
            if (password != confirmPassword)
            {
                _errorText.Text = "Пароли не совпадают";
                return;
            }
            
            // Регистрация
            if (_authService.Register(username, email, password))
            {
                // Автоматически логинимся и переходим к чатам
                var user = _authService.GetCurrentUser();
                if (user != null)
                {
                    var window = this.FindAncestorOfType<Window>();
                    if (window?.DataContext is AppController controller)
                    {
                        controller.CurrentUser = user;
                        controller.ShowChatList();
                    }
                }
                else
                {
                    _errorText.Text = "Ошибка при создании аккаунта";
                }
            }
            else
            {
                _errorText.Text = "Пользователь с таким email или именем уже существует";
            }
        }
        
        private Button CreateButton(string text, string color)
        {
            return new Button
            {
                Content = text,
                Height = 45,
                Background = new SolidColorBrush(Color.Parse(color)),
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 5)
            };
        }
    }
}