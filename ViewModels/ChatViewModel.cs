using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MessengerClient.Models;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly Chat _chat;
        private readonly User _currentUser;
        private readonly NavigationService _navigation;
        private readonly Window? _parentWindow;

        private string _messageText = "";
        private string _searchText = "";

        public Chat CurrentChat => _chat;
        public User CurrentUser => _currentUser;

        public ObservableCollection<Message> Messages { get; }
        public ObservableCollection<Message> FilteredMessages { get; }

        public string MessageText
        {
            get => _messageText;
            set => SetField(ref _messageText, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetField(ref _searchText, value))
                {
                    FilterMessages();
                }
            }
        }

        public ICommand SendMessageCommand { get; }
        public ICommand SendFileCommand { get; }
        public ICommand SendImageCommand { get; }
        public ICommand EditMessageCommand { get; }
        public ICommand DeleteMessageCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SearchCommand { get; }

        public ChatViewModel(Chat chat, User currentUser, NavigationService navigation, Window? parentWindow = null)
        {
            _chat = chat;
            _currentUser = currentUser;
            _navigation = navigation;
            _parentWindow = parentWindow;

            Messages = new ObservableCollection<Message>();
            FilteredMessages = new ObservableCollection<Message>();

            SendMessageCommand = new RelayCommand(_ => SendMessage(), _ => !string.IsNullOrWhiteSpace(MessageText));
            SendFileCommand = new RelayCommand(async _ => await SendFileAsync());
            SendImageCommand = new RelayCommand(async _ => await SendImageAsync());
            EditMessageCommand = new RelayCommand(p => EditMessage(p as Message));
            DeleteMessageCommand = new RelayCommand(p => DeleteMessage(p as Message));
            BackCommand = new RelayCommand(_ => _navigation.GoBack());
            SearchCommand = new RelayCommand(_ => { /* TODO: Реализовать поиск в чате */ });

            LoadMessages();
            
            // Симуляция получения сообщений в реальном времени
            SimulateRealTimeMessages();
        }

        private void LoadMessages()
        {
            Messages.Clear();
            FilteredMessages.Clear();

            // Симуляция загрузки сообщений
            var otherUserId = _chat.ParticipantIds.FirstOrDefault(id => id != _currentUser.Id) ?? "other_user";
            
            var messages = new[]
            {
                new Message
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = _chat.Id,
                    SenderId = _currentUser.Id,
                    Content = "Привет! Как дела?",
                    Type = MessageType.Text,
                    Timestamp = DateTime.Now.AddMinutes(-10),
                    Status = MessageStatus.Read
                },
                new Message
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = _chat.Id,
                    SenderId = otherUserId,
                    Content = "Отлично, спасибо! А у тебя?",
                    Type = MessageType.Text,
                    Timestamp = DateTime.Now.AddMinutes(-8),
                    Status = MessageStatus.Read
                },
                new Message
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = _chat.Id,
                    SenderId = _currentUser.Id,
                    Content = "Тоже всё хорошо!",
                    Type = MessageType.Text,
                    Timestamp = DateTime.Now.AddMinutes(-5),
                    Status = MessageStatus.Delivered
                }
            };

            foreach (var msg in messages)
            {
                Messages.Add(msg);
                FilteredMessages.Add(msg);
            }
        }

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageText)) return;

            var message = new Message
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = _chat.Id,
                SenderId = _currentUser.Id,
                Content = MessageText,
                Type = MessageType.Text,
                Timestamp = DateTime.Now,
                Status = MessageStatus.Sending
            };

            Messages.Add(message);
            FilteredMessages.Add(message);
            MessageText = "";

            // Симуляция отправки
            Task.Run(async () =>
            {
                await Task.Delay(500);
                message.Status = MessageStatus.Sent;
                OnPropertyChanged(nameof(Messages));

                await Task.Delay(1000);
                message.Status = MessageStatus.Delivered;
                OnPropertyChanged(nameof(Messages));
            });
        }

        private async Task SendFileAsync()
        {
            if (_parentWindow == null) return;

            try
            {
                var files = await _parentWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Выберите файл",
                    AllowMultiple = false
                });

                if (files.Count > 0 && files[0] != null)
                {
                    var file = files[0];
                    var filePath = file.Path.IsAbsoluteUri ? file.Path.AbsolutePath : file.Path.LocalPath;
                    var fileName = file.Name;

                    var message = new Message
                    {
                        Id = Guid.NewGuid().ToString(),
                        ChatId = _chat.Id,
                        SenderId = _currentUser.Id,
                        Content = $"📎 {fileName}",
                        Type = MessageType.Document,
                        MediaPath = filePath,
                        MediaName = fileName,
                        Timestamp = DateTime.Now,
                        Status = MessageStatus.Sending
                    };

                    Messages.Add(message);
                    FilteredMessages.Add(message);

                    // Симуляция отправки
                    await Task.Delay(500);
                    message.Status = MessageStatus.Sent;
                    await Task.Delay(1000);
                    message.Status = MessageStatus.Delivered;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке файла: {ex.Message}");
            }
        }

        private async Task SendImageAsync()
        {
            if (_parentWindow == null) return;

            try
            {
                var files = await _parentWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Выберите изображение",
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
                    var filePath = file.Path.IsAbsoluteUri ? file.Path.AbsolutePath : file.Path.LocalPath;

                    var message = new Message
                    {
                        Id = Guid.NewGuid().ToString(),
                        ChatId = _chat.Id,
                        SenderId = _currentUser.Id,
                        Content = "📷 Изображение",
                        Type = MessageType.Image,
                        MediaPath = filePath,
                        MediaName = file.Name,
                        Timestamp = DateTime.Now,
                        Status = MessageStatus.Sending
                    };

                    Messages.Add(message);
                    FilteredMessages.Add(message);

                    // Симуляция отправки
                    await Task.Delay(500);
                    message.Status = MessageStatus.Sent;
                    await Task.Delay(1000);
                    message.Status = MessageStatus.Delivered;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке изображения: {ex.Message}");
            }
        }

        private void EditMessage(Message? message)
        {
            if (message == null || message.SenderId != _currentUser.Id) return;

            MessageText = message.Content;
            message.EditedAt = DateTime.Now;
            OnPropertyChanged(nameof(Messages));
        }

        private void DeleteMessage(Message? message)
        {
            if (message == null || message.SenderId != _currentUser.Id) return;

            message.IsDeleted = true;
            message.Content = "Сообщение удалено";
            OnPropertyChanged(nameof(Messages));
        }

        private void FilterMessages()
        {
            FilteredMessages.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? Messages
                : Messages.Where(m => m.Content != null && m.Content.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var msg in filtered)
            {
                FilteredMessages.Add(msg);
            }
        }

        private void SimulateRealTimeMessages()
        {
            // Симуляция получения сообщений от другого пользователя
            Task.Run(async () =>
            {
                await Task.Delay(30000); // Через 30 секунд

                var otherUserId = _chat.ParticipantIds.FirstOrDefault(id => id != _currentUser.Id) ?? "other_user";
                
                var newMessage = new Message
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = _chat.Id,
                    SenderId = otherUserId,
                    Content = "Новое сообщение!",
                    Type = MessageType.Text,
                    Timestamp = DateTime.Now,
                    Status = MessageStatus.Delivered
                };

                Messages.Add(newMessage);
                FilteredMessages.Add(newMessage);
                
                // Показываем уведомление (симуляция)
                // В реальном приложении здесь будет вызов NotificationService
                Console.WriteLine($"[НОВОЕ СООБЩЕНИЕ] {_chat.Name}: {newMessage.Content}");
            });
        }
    }
}
