using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MessengerClient.ViewModels;
using FontWeight = Avalonia.Media.FontWeight;

namespace MessengerClient.Views
{
    public partial class ChatListView : UserControl
    {
        private ChatListViewModel? _viewModel;
        private StackPanel? _chatsPanel;
        private TextBlock? _usernameText;
        private TextBlock? _chatsCountText;
        
        public ChatListView()
        {
            InitializeComponent();
            
            // Ждем инициализации
            this.Initialized += (s, e) =>
            {
                _chatsPanel = this.FindControl<StackPanel>("ChatsPanel");
                _usernameText = this.FindControl<TextBlock>("UsernameText");
                _chatsCountText = this.FindControl<TextBlock>("ChatsCountText");
                
                var profileBtn = this.FindControl<Button>("ProfileBtn");
                var logoutBtn = this.FindControl<Button>("LogoutBtn");
                
                if (profileBtn != null) profileBtn.Click += ProfileBtn_Click;
                if (logoutBtn != null) logoutBtn.Click += LogoutBtn_Click;
                
                // Обновляем UI если ViewModel уже установлен
                if (_viewModel != null)
                {
                    UpdateUI();
                }
            };
            
            this.DataContextChanged += OnDataContextChanged;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnDataContextChanged(object? sender, EventArgs e)
        {
            Console.WriteLine("DataContext changed");
            
            _viewModel = DataContext as ChatListViewModel;
            
            if (_viewModel != null)
            {
                Console.WriteLine($"ViewModel set. User: {_viewModel.CurrentUser.Username}");
                
                // Обновляем UI если элементы уже инициализированы
                if (_chatsPanel != null)
                {
                    UpdateUI();
                }
            }
        }

        private void UpdateUI()
        {
            if (_viewModel == null || _usernameText == null || _chatsPanel == null) 
            {
                Console.WriteLine($"UpdateUI skipped: ViewModel={_viewModel != null}, UsernameText={_usernameText != null}, ChatsPanel={_chatsPanel != null}");
                return;
            }
            
            Console.WriteLine($"Updating UI. Chats: {_viewModel.Chats.Count}");
            
            // Обновляем имя пользователя
            _usernameText.Text = _viewModel.CurrentUser.Username;
            
            // Очищаем и добавляем чаты
            _chatsPanel.Children.Clear();
            
            foreach (var chat in _viewModel.FilteredChats)
            {
                var chatItem = CreateChatItem(chat);
                _chatsPanel.Children.Add(chatItem);
            }
            
            // Подписываемся на изменения FilteredChats
            _viewModel.FilteredChats.CollectionChanged += (s, e) =>
            {
                if (_chatsPanel != null)
                {
                    _chatsPanel.Children.Clear();
                    foreach (var chat in _viewModel.FilteredChats)
                    {
                        var chatItem = CreateChatItem(chat);
                        _chatsPanel.Children.Add(chatItem);
                    }
                }
            };
            
            // Обновляем счетчик
            UpdateChatsCount();
        }

        private Border CreateChatItem(Models.Chat chat)
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
                if (border.Tag is Models.Chat selectedChat && _viewModel != null)
                {
                    _viewModel.SelectedChat = selectedChat;
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
                Foreground = new SolidColorBrush(Color.Parse("#666"))
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

        private void UpdateChatsCount()
        {
            if (_viewModel != null && _chatsCountText != null)
            {
                _chatsCountText.Text = $"{_viewModel.Chats.Count} chats";
            }
        }

        private void ProfileBtn_Click(object? sender, RoutedEventArgs e)
        {
            Console.WriteLine("Profile button clicked");
            if (_viewModel != null)
            {
                _viewModel.NavigateToProfile();
            }
        }

        private void LogoutBtn_Click(object? sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.LogoutCommand.Execute(null);
            }
        }
    }
}