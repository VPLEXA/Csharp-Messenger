using System;

namespace MessengerClient.Models
{
    public class Message
    {
        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public string? FilePath { get; set; }
        public bool HasAttachment => !string.IsNullOrEmpty(FilePath);
    }
}