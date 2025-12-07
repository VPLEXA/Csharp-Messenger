using System;

namespace MessengerClient.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = ""; // В реальном приложении - хеш
        public string Avatar { get; set; } = "/Assets/default-avatar.png";
        public bool IsOnline { get; set; } = true;
        public UserStatus Status { get; set; } = UserStatus.Online;
        public string Bio { get; set; } = "";
        public DateTime LastSeen { get; set; } = DateTime.Now;
        public NotificationSettings NotificationSettings { get; set; } = new NotificationSettings();
    }

    public enum UserStatus
    {
        Online,
        Offline,
        DoNotDisturb,
        Away
    }

    public class NotificationSettings
    {
        public bool Enabled { get; set; } = true;
        public bool SoundEnabled { get; set; } = true;
        public bool BannerOnly { get; set; } = false;
        public bool SmartNotifications { get; set; } = true; // Не показывать если активен в диалоге
    }
}