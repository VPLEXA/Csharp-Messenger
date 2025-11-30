using System.Collections.ObjectModel;
using System.Linq;
using MessengerClient.Models;
using MessengerClient.Services;
using System.Windows.Input;

namespace MessengerClient.ViewModels
{
    public class ChatListViewModel : ViewModelBase
    {
        private readonly StateManager _stateManager;
        private readonly NavigationService _navigationService;
        private Chat? _selectedChat;

        public ObservableCollection<Chat> Chats { get; } = new();
        public User? CurrentUser => _stateManager.CurrentUser;

        public Chat? SelectedChat
        {
            get => _selectedChat;
            set
            {
                SetField(ref _selectedChat, value);
                if (value != null)
                {
                    _stateManager.SetCurrentChat(value);
                    // Завтра добавим навигацию к чату
                }
            }
        }

        public ICommand LogoutCommand { get; }

        public ChatListViewModel(StateManager stateManager, NavigationService navigationService)
        {
            _stateManager = stateManager;
            _navigationService = navigationService;
            LogoutCommand = new RelayCommand(Logout);

            LoadChats(); // Загружаем тестовые чаты
        }

        private void LoadChats()
        {
            // Тестовые данные - потом заменим на реальные
            Chats.Clear();

            var chats = new[]
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

            foreach (var chat in chats)
            {
                Chats.Add(chat);
            }
        }

        private void Logout()
        {
            _stateManager.CurrentUser = null;
            _navigationService.NavigateToLogin();
        }
    }


}