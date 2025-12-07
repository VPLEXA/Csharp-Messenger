using System;

namespace MessengerClient.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ChatId { get; set; } = "";
        public string SenderId { get; set; } = "";
        public string Content { get; set; } = "";
        public MessageType Type { get; set; } = MessageType.Text;
        public string? MediaPath { get; set; } // Путь к файлу (изображение, документ)
        public string? MediaName { get; set; } // Имя файла
        public long? MediaSize { get; set; } // Размер файла в байтах
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public DateTime? EditedAt { get; set; }
        public MessageStatus Status { get; set; } = MessageStatus.Sending;
        public bool IsDeleted { get; set; } = false;
    }

    public enum MessageType
    {
        Text,
        Image,
        Document,
        System // Системное сообщение
    }

    public enum MessageStatus
    {
        Sending,
        Sent,
        Delivered,
        Read
    }
}
