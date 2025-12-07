using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string _searchText = "";

        public ObservableCollection<Chat> Chats { get; }
        public ObservableCollection<Chat> FilteredChats { get; }
        public User CurrentUser => _currentUser;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetField(ref _searchText, value))
                {
                    FilterChats();
                }
            }
        }

        public Chat? SelectedChat
        {
            get => _selectedChat;
            set
            {
                SetField(ref _selectedChat, value);
                if (value != null) _navigationService.NavigateToChat(value);
            }
        }

        public ICommand LogoutCommand { get; }

        public ChatListViewModel(User currentUser, NavigationService navigationService)
        {
            _currentUser = currentUser;
            _navigationService = navigationService;

            Chats = new ObservableCollection<Chat>();
            FilteredChats = new ObservableCollection<Chat>();
            LogoutCommand = new RelayCommand(_ => _navigationService.NavigateToLogin());

            LoadChats();
        }

        private void LoadChats()
        {
            Chats.Clear();
            FilteredChats.Clear();
            
            // Начинаем с пустого списка чатов
            // Чаты будут добавляться при создании новых диалогов
        }

        private void FilterChats()
        {
            FilteredChats.Clear();
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? Chats
                : Chats.Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                  (c.LastMessage?.Content?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false));

            foreach (var chat in filtered)
            {
                FilteredChats.Add(chat);
            }
        }

        public void NavigateToProfile()
        {
            _navigationService.NavigateToProfile();
        }
    }
}
