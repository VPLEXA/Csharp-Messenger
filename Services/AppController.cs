using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MessengerClient.Models;

namespace MessengerClient.Services
{
    public class AppController
    {
        private readonly HttpClient _httpClient;
        private readonly StateManager _stateManager;

        public AppController(HttpClient httpClient, StateManager stateManager)
        {
            _httpClient = httpClient;
            _stateManager = stateManager;
        }

        public async Task<bool> UpdateUserProfileAsync(User user)
        {
            try
            {
                // Здесь будет реальный API вызов
                // Пример для демонстрации:
                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                // В реальном приложении:
                // var response = await _httpClient.PutAsync($"/api/users/{user.Id}", content);
                // response.EnsureSuccessStatusCode();
                
                // Для демо просто симулируем задержку
                await Task.Delay(500);
                
                // Обновляем состояние в StateManager
                _stateManager.CurrentUser = user;
                
                Console.WriteLine($"Профиль пользователя {user.Username} обновлен");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении профиля: {ex.Message}");
                return false;
            }
        }

        public async Task<User?> GetUserProfileAsync(string userId)
        {
            try
            {
                // Здесь будет реальный API вызов
                // var response = await _httpClient.GetAsync($"/api/users/{userId}");
                // response.EnsureSuccessStatusCode();
                // var json = await response.Content.ReadAsStringAsync();
                // return JsonSerializer.Deserialize<User>(json);
                
                // Для демо возвращаем текущего пользователя
                await Task.Delay(300);
                return _stateManager.CurrentUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении профиля: {ex.Message}");
                return null;
            }
        }

        public void UpdateUserStatus(string userId, UserStatus status)
        {
            if (_stateManager.CurrentUser != null && _stateManager.CurrentUser.Id == userId)
            {
                _stateManager.CurrentUser.Status = status;
                _stateManager.CurrentUser.IsOnline = status == UserStatus.Online || status == UserStatus.Away;
                _stateManager.CurrentUser.LastSeen = DateTime.Now;
                
                // Здесь можно отправить обновление на сервер
                Console.WriteLine($"Статус пользователя обновлен: {status}");
            }
        }
    }
}