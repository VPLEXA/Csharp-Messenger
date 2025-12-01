using System; 
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MessengerClient.Models;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private string _messageText = "";
        
        public Chat CurrentChat { get; }
        public ObservableCollection<Message> Messages { get; } = new();
        public User CurrentUser { get; }

        public string MessageText
        {
            get => _messageText;
            set => SetField(ref _messageText, value);
        }

        public ICommand SendMessageCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SendFileCommand { get; }

        public ChatViewModel(Chat chat, User currentUser, NavigationService navigationService)
        {
            CurrentChat = chat;
            CurrentUser = currentUser;
            _navigationService = navigationService;

            SendMessageCommand = new RelayCommand(SendMessage, CanSendMessage);
            BackCommand = new RelayCommand(BackToChatList);
            SendFileCommand = new RelayCommand(SendFile);

            LoadMessages(); // Загружаем тестовые сообщения
        }

        private void LoadMessages()
        {
            // Тестовые сообщения
            Messages.Clear();
            
            var messages = new[]
            {
                new Message 
                { 
                    Id = "1", 
                    Content = "Привет! Как дела?", 
                    SenderId = "other", 
                    Timestamp = System.DateTime.Now.AddMinutes(-30),
                    IsRead = true
                },
                new Message 
                { 
                    Id = "2", 
                    Content = "Привет! Все отлично, работаю над проектом", 
                    SenderId = "me", 
                    Timestamp = System.DateTime.Now.AddMinutes(-25),
                    IsRead = true
                },
                new Message 
                { 
                    Id = "3", 
                    Content = "Круто! Когда покажешь результат?", 
                    SenderId = "other", 
                    Timestamp = System.DateTime.Now.AddMinutes(-20),
                    IsRead = true
                },
                new Message 
                { 
                    Id = "4", 
                    Content = "Думаю к концу недели будет готово", 
                    SenderId = "me", 
                    Timestamp = System.DateTime.Now.AddMinutes(-15),
                    IsRead = true
                }
            };

            foreach (var message in messages)
            {
                Messages.Add(message);
            }
        }

        private bool CanSendMessage() => !string.IsNullOrWhiteSpace(MessageText);

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageText))
                return;

            var newMessage = new Message
            {
                Id = Guid.NewGuid().ToString(),
                Content = MessageText,
                SenderId = "me",
                Timestamp = DateTime.Now,
                IsRead = false
            };

            Messages.Add(newMessage);
            MessageText = "";
            
            // В будущем здесь будет отправка на сервер
            Console.WriteLine($"Message sent: {newMessage.Content}");
        }

        private void SendFile()
        {
            // Заглушка для отправки файла
            Console.WriteLine("Send file clicked");
        }

        private void BackToChatList()
        {
            // Вернемся к списку чатов
            // В будущем заменим на нормальную навигацию
            _navigationService.NavigateToChatList();
        }
    }
}