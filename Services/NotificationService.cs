using System;
using System.Threading.Tasks;
using MessengerClient.Models;

namespace MessengerClient.Services
{
    public class NotificationService
    {
        private readonly StateManager _stateManager;

        public NotificationService(StateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task ShowNotificationAsync(string title, string message, NotificationType type = NotificationType.Info)
        {
            var user = _stateManager.CurrentUser;
            if (user?.NotificationSettings == null)
            {
                return;
            }

            var settings = user.NotificationSettings;

            // Проверяем, включены ли уведомления
            if (!settings.Enabled)
            {
                return;
            }

            // Проверяем умные уведомления (не показывать, если пользователь активен)
            if (settings.SmartNotifications && user.IsOnline)
            {
                // В реальном приложении здесь будет проверка, активен ли пользователь в диалоге
                // Для демо просто пропускаем
            }

            // Симуляция показа уведомления
            Console.WriteLine($"[УВЕДОМЛЕНИЕ] {title}: {message}");

            // В реальном приложении здесь будет вызов системных уведомлений ОС
            // Например, через Avalonia.Notification или нативные API

            await Task.CompletedTask;
        }

        public async Task ShowMessageNotificationAsync(Message message, Chat chat)
        {
            var user = _stateManager.CurrentUser;
            if (user?.NotificationSettings == null)
            {
                return;
            }

            var settings = user.NotificationSettings;

            if (!settings.Enabled)
            {
                return;
            }

            // Проверяем умные уведомления
            if (settings.SmartNotifications && user.IsOnline)
            {
                // В реальном приложении проверяем, открыт ли этот чат
                return;
            }

            var title = chat.Name;
            var content = message.Type == MessageType.Image 
                ? "📷 Изображение" 
                : message.Type == MessageType.Document 
                    ? $"📎 {message.MediaName ?? "Файл"}" 
                    : message.Content;

            Console.WriteLine($"[НОВОЕ СООБЩЕНИЕ] {title}: {content}");

            // В реальном приложении здесь будет:
            // - Звук (если SoundEnabled)
            // - Баннер (если BannerOnly или обычное уведомление)
            // - Системное уведомление ОС

            await Task.CompletedTask;
        }
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        Success
    }
}

