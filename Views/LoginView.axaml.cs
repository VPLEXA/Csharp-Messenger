using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MessengerClient.Models;
using MessengerClient.Services;

namespace MessengerClient.Views
{
    public partial class LoginView : UserControl
    {
        private readonly AppController _controller;
        private TextBlock? _errorText;
        
        public LoginView() : this(null!) { }
        
        public LoginView(AppController controller)
        {
            _controller = controller;
            InitializeComponent();
            
            // Создаем UI в коде
            BuildUI();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void BuildUI()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 15,
                Width = 300,
                Background = new SolidColorBrush(Color.Parse("#f0f0f0"))
            };

            // Заголовок
            var title = new TextBlock
            {
                Text = "Messenger Login",
                FontSize = 20,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(title);

            // Email - используем Button для получения клика
            var emailLabel = new TextBlock
            {
                Text = "Email (click to enter):",
                FontWeight = FontWeight.SemiBold
            };
            stackPanel.Children.Add(emailLabel);

            var emailButton = new Button
            {
                Height = 40,
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            var emailText = new TextBlock
            {
                Text = "Click to enter email",
                Foreground = Brushes.Gray,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0)
            };
            emailButton.Content = emailText;
            
            emailButton.Click += (s, e) => ShowEmailDialog(emailText);
            stackPanel.Children.Add(emailButton);

            // Password
            var passwordLabel = new TextBlock
            {
                Text = "Password (click to enter):",
                FontWeight = FontWeight.SemiBold,
                Margin = new Thickness(0, 10, 0, 0)
            };
            stackPanel.Children.Add(passwordLabel);

            var passwordButton = new Button
            {
                Height = 40,
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            var passwordText = new TextBlock
            {
                Text = "Click to enter password",
                Foreground = Brushes.Gray,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0)
            };
            passwordButton.Content = passwordText;
            
            passwordButton.Click += (s, e) => ShowPasswordDialog(passwordText);
            stackPanel.Children.Add(passwordButton);

            // Кнопка Login
            var loginBtn = new Button
            {
                Content = "Login",
                Height = 40,
                Background = new SolidColorBrush(Color.Parse("#2b579a")),
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 20, 0, 0)
            };
            
            // Сохраняем тексты для использования в логине
            loginBtn.Click += (s, e) => 
            {
                if (emailText == null || passwordText == null || _errorText == null) return;
                
                var email = emailText.Text;
                var password = passwordText.Text;
                
                if (email == "Click to enter email" || password == "Click to enter password")
                {
                    _errorText.Text = "Please enter email and password";
                    return;
                }
                
                if (!email.Contains("@"))
                {
                    _errorText.Text = "Please enter a valid email";
                    return;
                }
                
                _errorText.Text = "";
                
                _controller.CurrentUser = new User
                {
                    Id = "1",
                    Username = email.Split('@')[0],
                    Email = email,
                    IsOnline = true
                };
                
                _controller.ShowChatList();
            };
            
            stackPanel.Children.Add(loginBtn);

            // Текст ошибки
            _errorText = new TextBlock
            {
                Foreground = Brushes.Red,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                MaxWidth = 280
            };
            stackPanel.Children.Add(_errorText);

            this.Content = stackPanel;
        }
        
        private void ShowEmailDialog(TextBlock emailText)
        {
            // Заглушка - в реальном приложении здесь будет диалог
            emailText.Text = "test@example.com";
            emailText.Foreground = Brushes.Black;
        }
        
        private void ShowPasswordDialog(TextBlock passwordText)
        {
            // Заглушка - в реальном приложении здесь будет диалог
            passwordText.Text = "password123";
            passwordText.Foreground = Brushes.Black;
        }
    }
}