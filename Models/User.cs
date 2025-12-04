using System;

namespace MessengerClient.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Профиль
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Status { get; set; } = "🟢 Онлайн";
    }
}