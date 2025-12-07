using System;
using System.Collections.Generic;

namespace MessengerClient.Models
{
    public class Chat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public ChatType Type { get; set; } = ChatType.Private;
        public List<string> ParticipantIds { get; set; } = new List<string>();
        public Message? LastMessage { get; set; }
        public int UnreadCount { get; set; }
        public DateTime LastActivity { get; set; } = DateTime.Now;
        public bool IsPinned { get; set; } = false;
    }

    public enum ChatType
    {
        Private,
        Group
    }
}
