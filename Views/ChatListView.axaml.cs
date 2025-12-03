using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MessengerClient.Models;
using MessengerClient.Services;

namespace MessengerClient.Views
{
    public partial class ChatListView : UserControl
    {
        private readonly AppController _controller;
        private StackPanel? _chatsPanel;
        private TextBlock? _chatsCountText;
        
        public ChatListView(AppController controller)
        {
            _controller = controller;
            InitializeComponent();
            
            this.Initialized += (s, e) =>
            {
                _chatsPanel = this.FindControl<StackPanel>("ChatsPanel");
                _chatsCountText = this.FindControl<TextBlock>("ChatsCountText");
                
                var profileBtn = this.FindControl<Button>("ProfileBtn");
                var logoutBtn = this.FindControl<Button>("LogoutBtn");
                
                if (profileBtn != null) profileBtn.Click += (s, e) => _controller.ShowProfile();
                if (logoutBtn != null) logoutBtn.Click += (s, e) => _controller.Logout();
                
                LoadChats();
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void LoadChats()
        {
            if (_chatsPanel == null || _chatsCountText == null) return;
            
            _chatsPanel.Children.Clear();
            
            // Тестовые чаты
            var chats = new[]
            {
                new Chat 
                { 
                    Id = "1", 
                    Name = "John Doe", 
                    UnreadCount = 2,
                    LastMessage = new Message 
                    { 
                        Content = "Hello there! How are you doing?", 
                        Timestamp = DateTime.Now.AddMinutes(-15)
                    }
                },
                new Chat 
                { 
                    Id = "2", 
                    Name = "Alice Smith", 
                    UnreadCount = 0,
                    LastMessage = new Message 
                    { 
                        Content = "Did you finish the project?", 
                        Timestamp = DateTime.Now.AddHours(-2)
                    }
                },
                new Chat 
                { 
                    Id = "3", 
                    Name = "Bob Johnson", 
                    UnreadCount = 3,
                    LastMessage = new Message 
                    { 
                        Content = "Meeting tomorrow at 10 AM", 
                        Timestamp = DateTime.Now.AddDays(-1)
                    }
                },
                new Chat 
                { 
                    Id = "4", 
                    Name = "Emma Wilson", 
                    UnreadCount = 0,
                    LastMessage = new Message 
                    { 
                        Content = "Check out this photo!", 
                        Timestamp = DateTime.Now.AddDays(-2)
                    }
                }
            };

            foreach (var chat in chats)
            {
                var chatItem = CreateChatItem(chat);
                _chatsPanel.Children.Add(chatItem);
            }
            
            _chatsCountText.Text = $"{chats.Length} chats";
        }
        
        private Border CreateChatItem(Chat chat)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.Parse("#f8f9fa")),
                Margin = new Thickness(0, 2),
                Padding = new Thickness(15),
                CornerRadius = new CornerRadius(5),
                Tag = chat,
                Cursor = new Cursor(StandardCursorType.Hand)
            };

            border.PointerEntered += (s, e) =>
            {
                border.Background = new SolidColorBrush(Color.Parse("#e0e0e0"));
            };

            border.PointerExited += (s, e) =>
            {
                border.Background = new SolidColorBrush(Color.Parse("#f8f9fa"));
            };

            border.Tapped += (s, e) =>
            {
                if (border.Tag is Chat selectedChat)
                {
                    _controller.ShowChat(selectedChat);
                }
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Avatar
            var avatarBorder = new Border
            {
                Width = 50,
                Height = 50,
                Background = new SolidColorBrush(Color.Parse("#3498db")),
                CornerRadius = new CornerRadius(25),
                Margin = new Thickness(0, 0, 15, 0)
            };

            var avatarText = new TextBlock
            {
                Text = chat.Name.Length > 0 ? chat.Name[0].ToString().ToUpper() : "?",
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            avatarBorder.Child = avatarText;
            Grid.SetColumn(avatarBorder, 0);
            grid.Children.Add(avatarBorder);

            // Chat Info
            var infoPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            var nameText = new TextBlock
            {
                Text = chat.Name,
                FontWeight = FontWeight.Bold,
                FontSize = 14
            };

            var lastMessageText = new TextBlock
            {
                Text = chat.LastMessage?.Content ?? "",
                Foreground = new SolidColorBrush(Color.Parse("#666")),
                TextTrimming = TextTrimming.CharacterEllipsis,
                MaxWidth = 250
            };

            infoPanel.Children.Add(nameText);
            infoPanel.Children.Add(lastMessageText);
            Grid.SetColumn(infoPanel, 1);
            grid.Children.Add(infoPanel);

            // Time & Unread
            var rightPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            var timeText = new TextBlock
            {
                Text = chat.LastMessage?.Timestamp.ToString("HH:mm") ?? "",
                Foreground = new SolidColorBrush(Color.Parse("#999")),
                FontSize = 11
            };

            rightPanel.Children.Add(timeText);

            if (chat.UnreadCount > 0)
            {
                var unreadBorder = new Border
                {
                    Background = new SolidColorBrush(Color.Parse("#e74c3c")),
                    CornerRadius = new CornerRadius(10),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 5, 0, 0),
                    Padding = new Thickness(6, 3)
                };

                var unreadText = new TextBlock
                {
                    Text = chat.UnreadCount.ToString(),
                    Foreground = Brushes.White,
                    FontSize = 10,
                    FontWeight = FontWeight.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                unreadBorder.Child = unreadText;
                rightPanel.Children.Add(unreadBorder);
            }

            Grid.SetColumn(rightPanel, 2);
            grid.Children.Add(rightPanel);

            border.Child = grid;
            return border;
        }
    }
}