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
    public partial class ChatView : UserControl
    {
        private readonly AppController _controller;
        private readonly Chat _chat;
        private StackPanel? _messagesPanel;
        private TextBlock? _currentMessageText;
        
        public ChatView() : this(null!, null!) { }
        
        public ChatView(AppController controller, Chat chat)
        {
            _controller = controller;
            _chat = chat;
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
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Header
            var headerBorder = new Border
            {
                Background = new SolidColorBrush(Color.Parse("#2b579a")),
                Padding = new Thickness(10)
            };

            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Back Button
            var backBtn = new Button
            {
                Content = "←",
                Width = 40,
                Height = 40,
                Background = Brushes.Transparent,
                Foreground = Brushes.White,
                FontSize = 20,
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 0, 10, 0)
            };
            backBtn.Click += (s, e) => _controller.ShowChatList();
            Grid.SetColumn(backBtn, 0);
            headerGrid.Children.Add(backBtn);

            // Chat Info
            var chatInfoPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            var chatNameText = new TextBlock
            {
                Text = _chat.Name,
                FontSize = 16,
                FontWeight = FontWeight.Bold,
                Foreground = Brushes.White
            };

            var statusText = new TextBlock
            {
                Text = "online",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.Parse("#a0c8ff"))
            };

            chatInfoPanel.Children.Add(chatNameText);
            chatInfoPanel.Children.Add(statusText);
            Grid.SetColumn(chatInfoPanel, 1);
            headerGrid.Children.Add(chatInfoPanel);

            // Attach Button
            var attachBtn = new Button
            {
                Content = "📎",
                Width = 35,
                Height = 35,
                Background = Brushes.Transparent,
                Foreground = Brushes.White,
                FontSize = 16
            };
            attachBtn.Click += OnAttachClick;
            Grid.SetColumn(attachBtn, 2);
            headerGrid.Children.Add(attachBtn);

            headerBorder.Child = headerGrid;
            Grid.SetRow(headerBorder, 0);
            grid.Children.Add(headerBorder);

            // Messages Area
            var messagesScroll = new ScrollViewer
            {
                Padding = new Thickness(10)
            };

            _messagesPanel = new StackPanel();
            messagesScroll.Content = _messagesPanel;

            Grid.SetRow(messagesScroll, 1);
            grid.Children.Add(messagesScroll);

            // Message Input
            var inputBorder = new Border
            {
                Background = new SolidColorBrush(Color.Parse("#f5f5f5")),
                Padding = new Thickness(10)
            };

            var inputGrid = new Grid();
            inputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            inputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Message Input Button
            var messageButton = new Button
            {
                Height = 40,
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            _currentMessageText = new TextBlock
            {
                Text = "Click to type message...",
                Foreground = Brushes.Gray,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0)
            };
            messageButton.Content = _currentMessageText;
            messageButton.Click += (s, e) => ShowMessageDialog();

            Grid.SetColumn(messageButton, 0);
            inputGrid.Children.Add(messageButton);

            // Send Button
            var sendButton = new Button
            {
                Content = "Send",
                Width = 80,
                Height = 40,
                Background = new SolidColorBrush(Color.Parse("#2b579a")),
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(10, 0, 0, 0)
            };
            sendButton.Click += OnSendClick;

            Grid.SetColumn(sendButton, 1);
            inputGrid.Children.Add(sendButton);

            inputBorder.Child = inputGrid;
            Grid.SetRow(inputBorder, 2);
            grid.Children.Add(inputBorder);

            // Загружаем сообщения
            LoadMessages();

            this.Content = grid;
        }
        
        private void LoadMessages()
        {
            if (_messagesPanel == null) return;
            
            _messagesPanel.Children.Clear();
            
            var messages = new[]
            {
                new Message 
                { 
                    Id = "1", 
                    Content = "Привет! Как дела?", 
                    SenderId = "other", 
                    Timestamp = DateTime.Now.AddMinutes(-30),
                    IsRead = true
                },
                new Message 
                { 
                    Id = "2", 
                    Content = "Привет! Все отлично, работаю над проектом", 
                    SenderId = "me", 
                    Timestamp = DateTime.Now.AddMinutes(-25),
                    IsRead = true
                },
                new Message 
                { 
                    Id = "3", 
                    Content = "Круто! Когда покажешь результат?", 
                    SenderId = "other", 
                    Timestamp = DateTime.Now.AddMinutes(-20),
                    IsRead = true
                },
                new Message 
                { 
                    Id = "4", 
                    Content = "Думаю к концу недели будет готово", 
                    SenderId = "me", 
                    Timestamp = DateTime.Now.AddMinutes(-15),
                    IsRead = true
                }
            };

            foreach (var message in messages)
            {
                var messageItem = CreateMessageItem(message);
                _messagesPanel.Children.Add(messageItem);
            }
        }
        
        private Border CreateMessageItem(Message message)
        {
            var isMyMessage = message.SenderId == "me";
            
            var border = new Border
            {
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12),
                MaxWidth = 300,
                HorizontalAlignment = isMyMessage ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                Margin = new Thickness(0, 5),
                Background = isMyMessage ? 
                    new SolidColorBrush(Color.Parse("#2b579a")) : 
                    new SolidColorBrush(Color.Parse("#e0e0e0"))
            };

            var textBlock = new TextBlock
            {
                Text = message.Content,
                Foreground = isMyMessage ? Brushes.White : Brushes.Black,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = textBlock;
            return border;
        }
        
        private void ShowMessageDialog()
        {
            if (_currentMessageText == null) return;
            
            // Заглушка - в реальном приложении здесь будет диалог
            _currentMessageText.Text = "Hello! This is a test message";
            _currentMessageText.Foreground = Brushes.Black;
        }
        
        private void OnSendClick(object? sender, RoutedEventArgs e)
        {
            if (_currentMessageText == null || _messagesPanel == null) return;
            
            var text = _currentMessageText.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(text) || text == "Click to type message...") 
            {
                Console.WriteLine("No message to send");
                return;
            }
            
            Console.WriteLine($"Sending message: {text}");
            
            var newMessage = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Content = text,
                SenderId = "me",
                Timestamp = DateTime.Now,
                IsRead = false
            };
            
            var messageItem = CreateMessageItem(newMessage);
            _messagesPanel.Children.Add(messageItem);
            
            _currentMessageText.Text = "Click to type message...";
            _currentMessageText.Foreground = Brushes.Gray;
            
            Console.WriteLine("Message sent successfully");
        }
        
        private void OnAttachClick(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Attach file clicked");
        }
    }
}