using System.Collections.Generic;

namespace MessengerClient.Models
{
    public class Chat
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> ParticipantIds { get; set; } = new();
        public Message? LastMessage { get; set; }
        public int UnreadCount { get; set; }
    }
}