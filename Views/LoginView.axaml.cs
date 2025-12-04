using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using MessengerClient.Models;
using MessengerClient.Services;

namespace MessengerClient.Views
{
    public class LoginView : UserControl
    {
        private readonly AuthService _authService;
        
        private TextBox _emailBox = null!;
        private TextBox _passwordBox = null!;
        private TextBlock _errorText = null!;
        
        public LoginView()
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
                Text = "Вход в Messenger",
                FontSize = 24,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = new SolidColorBrush(Color.Parse("#2b579a"))
            };
            stackPanel.Children.Add(title);

            // Email
            var emailLabel = new TextBlock
            {
                Text = "Email:",
                FontWeight = FontWeight.SemiBold
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

            // Кнопка Login
            var loginBtn = CreateButton("Войти", "#2b579a");
            loginBtn.Click += OnLoginClicked;
            stackPanel.Children.Add(loginBtn);

            // Кнопка Register
            var registerBtn = CreateButton("Регистрация", "#27ae60");
            registerBtn.Click += (s, e) =>
            {
                var window = this.FindAncestorOfType<Window>();
                if (window?.DataContext is AppController controller)
                {
                    controller.ShowRegister();
                }
            };
            stackPanel.Children.Add(registerBtn);

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

            // Demo аккаунт
            var demo = new TextBlock
            {
                Text = "Для демо: test@test.com / 123456",
                Foreground = new SolidColorBrush(Color.Parse("#666")),
                FontSize = 12,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10)
            };
            stackPanel.Children.Add(demo);

            this.Content = stackPanel;
            
            // Автозаполнение для демо
            _emailBox.Text = "test@test.com";
            _passwordBox.Text = "123456";
        }
        
        private void OnFieldChanged(object sender, TextChangedEventArgs e)
        {
            _errorText.Text = "";
        }
        
        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            var email = _emailBox.Text.Trim();
            var password = _passwordBox.Text;
            
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                _errorText.Text = "Введите email и пароль";
                return;
            }
            
            if (!email.Contains("@"))
            {
                _errorText.Text = "Введите корректный email";
                return;
            }
            
            var user = _authService.Login(email, password);
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
                _errorText.Text = "Неверный email или пароль";
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