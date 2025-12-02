using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Thickness = Avalonia.Thickness;
using MessengerClient.Models;
using MessengerClient.Views;

namespace MessengerClient
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // ВРЕМЕННО: сразу показываем список чатов с тестовыми данными
                var mainWindow = new MainWindow();
                
                // Создаем тестового пользователя
                var testUser = new User { Id = "1", Username = "TestUser", Email = "test@example.com" };
                
                // Создаем тестовые чаты
                var testChats = new System.Collections.Generic.List<Chat>
                {
                    new Chat { Id = "1", Name = "John Doe", UnreadCount = 2,
                        LastMessage = new Message { Content = "Hello there!", Timestamp = System.DateTime.Now.AddMinutes(-5) }},
                    new Chat { Id = "2", Name = "Alice Smith", UnreadCount = 0,
                        LastMessage = new Message { Content = "How are you?", Timestamp = System.DateTime.Now.AddHours(-1) }},
                    new Chat { Id = "3", Name = "Bob Johnson", UnreadCount = 1,
                        LastMessage = new Message { Content = "Meeting tomorrow", Timestamp = System.DateTime.Now.AddDays(-1) }},
                    new Chat { Id = "4", Name = "Emma Wilson", UnreadCount = 0,
                        LastMessage = new Message { Content = "Did you see the file?", Timestamp = System.DateTime.Now.AddDays(-2) }}
                };
                
                // Создаем упрощенный UI прямо здесь
                var stackPanel = new StackPanel();
                
                // Заголовок
                var header = new Border
                {
                    Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#2b579a")),
                    Padding = new Thickness(15),
                    Child = new TextBlock 
                    { 
                        Text = "MESSENGER - DIRECT TEST", 
                        FontSize = 18, 
                        FontWeight = Avalonia.Media.FontWeight.Bold,
                        Foreground = Avalonia.Media.Brushes.White
                    }
                };
                stackPanel.Children.Add(header);
                
                // Тестовые чаты
                foreach (var chat in testChats)
                {
                    var chatBorder = new Border
                    {
                        Background = Avalonia.Media.Brushes.White,
                        Margin = new Thickness(10, 5),
                        Padding = new Thickness(15),
                        BorderBrush = Avalonia.Media.Brushes.LightGray,
                        BorderThickness = new Thickness(1)
                    };
                    
                    var chatText = new TextBlock
                    {
                        Text = $"{chat.Name}: {(chat.LastMessage?.Content ?? "No message")}",                       
                        FontWeight = Avalonia.Media.FontWeight.Bold
                    };
                    
                    chatBorder.Child = chatText;
                    stackPanel.Children.Add(chatBorder);
                }
                
                // Кнопка
                var testButton = new Button
                {
                    Content = "TEST BUTTON",
                    Margin = new Thickness(20),
                    Padding = new Thickness(15),
                    Background = Avalonia.Media.Brushes.Green,
                    Foreground = Avalonia.Media.Brushes.White,
                    FontWeight = Avalonia.Media.FontWeight.Bold
                };
                testButton.Click += (s, e) =>
                {
                    Console.WriteLine("Test button clicked!");
                };
                stackPanel.Children.Add(testButton);
                
                // Устанавливаем контент
                mainWindow.Content = stackPanel;
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}