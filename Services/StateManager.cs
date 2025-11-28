using MessengerClient.Models;
using System.Collections.ObjectModel;

namespace MessengerClient.Services
{
    public class StateManager
    {
        public User? CurrentUser { get; set; }
        public ObservableCollection<Chat> Chats { get; set; } = new();
        public Chat? CurrentChat { get; set; }

        public void AddChat(Chat chat)
        {
            Chats.Add(chat);
        }

        public void SetCurrentChat(Chat chat)
        {
            CurrentChat = chat;
        }
    }
}