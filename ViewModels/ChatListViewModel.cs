using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MessengerClient.Models;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class ChatListViewModel : ViewModelBase
    {
        private readonly User _currentUser;
        private readonly NavigationService _navigationService;
        private Chat? _selectedChat;

        public ObservableCollection<Chat> Chats { get; }
        public User CurrentUser => _currentUser;

        public Chat? SelectedChat
        {
            get => _selectedChat;
            set
            {
                SetField(ref _selectedChat, value);
                if (value != null)
                {
                    _navigationService.NavigateToChat(value);
                }
            }
        }

        public ICommand LogoutCommand { get; }

        public ChatListViewModel(User currentUser, NavigationService navigationService)
        {
            _currentUser = currentUser;
            _navigationService = navigationService;
            
            Chats = new ObservableCollection<Chat>();
            LogoutCommand = new RelayCommand(Logout);
            
            Console.WriteLine($"ChatListViewModel created for user: {currentUser.Username}");
            
            LoadChatsAsync();
        }

        private async void LoadChatsAsync()
        {
            await Task.Delay(100);
            
            Console.WriteLine("Loading chats...");
            Chats.Clear();
            
            var chats = new[]
            {
                new Chat { Id = "1", Name = "John Doe", UnreadCount = 2,
                    LastMessage = new Message { Content = "Hello there!", Timestamp = DateTime.Now.AddMinutes(-5) }},
                new Chat { Id = "2", Name = "Alice Smith", UnreadCount = 0,
                    LastMessage = new Message { Content = "How are you?", Timestamp = DateTime.Now.AddHours(-1) }},
                new Chat { Id = "3", Name = "Bob Johnson", UnreadCount = 1,
                    LastMessage = new Message { Content = "Meeting tomorrow", Timestamp = DateTime.Now.AddDays(-1) }},
                new Chat { Id = "4", Name = "Emma Wilson", UnreadCount = 0,
                    LastMessage = new Message { Content = "Did you see the file?", Timestamp = DateTime.Now.AddDays(-2) }}
            };

            foreach (var chat in chats)
            {
                Console.WriteLine($"Adding chat: {chat.Name}");
                Chats.Add(chat);
            }
            
            Console.WriteLine($"Total chats loaded: {Chats.Count}");
        }

        private void Logout()
        {
            _navigationService.NavigateToLogin();
        }
    }
}