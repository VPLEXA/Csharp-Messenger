using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using MessengerClient.Models;

namespace MessengerClient.Services
{
    public class AuthService
    {
        private const string USERS_FILE = "users.json";
        private const string SESSION_FILE = "current_session.json";
        
        private List<User> _users = new();
        
        public AuthService()
        {
            LoadUsers();
        }
        
        private void LoadUsers()
        {
            try
            {
                if (File.Exists(USERS_FILE))
                {
                    var json = File.ReadAllText(USERS_FILE);
                    _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки пользователей: {ex.Message}");
                _users = new List<User>();
            }
        }
        
        private void SaveUsers()
        {
            try
            {
                var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(USERS_FILE, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения пользователей: {ex.Message}");
            }
        }
        
        public bool Register(string username, string email, string password)
        {
            // Проверка уникальности email
            if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            
            // Проверка уникальности username
            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = password, // В реальном приложении нужно хешировать!
                FullName = username,
                Bio = "Новый пользователь Messenger",
                Status = "🟢 Онлайн"
            };
            
            _users.Add(user);
            SaveUsers();
            
            // Автоматический логин после регистрации
            SaveSession(user);
            
            return true;
        }
        
        public User? Login(string email, string password)
        {
            var user = _users.FirstOrDefault(u => 
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                u.PasswordHash == password); // В реальном приложении сравнивать хеши
            
            if (user != null)
            {
                user.IsOnline = true;
                SaveUsers();
                SaveSession(user);
            }
            
            return user;
        }
        
        public void Logout(User user)
        {
            user.IsOnline = false;
            SaveUsers();
            DeleteSession();
        }
        
        public User? GetCurrentUser()
        {
            try
            {
                if (File.Exists(SESSION_FILE))
                {
                    var json = File.ReadAllText(SESSION_FILE);
                    return JsonSerializer.Deserialize<User>(json);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки сессии: {ex.Message}");
            }
            
            return null;
        }
        
        private void SaveSession(User user)
        {
            try
            {
                var json = JsonSerializer.Serialize(user);
                File.WriteAllText(SESSION_FILE, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения сессии: {ex.Message}");
            }
        }
        
        private void DeleteSession()
        {
            try
            {
                if (File.Exists(SESSION_FILE))
                {
                    File.Delete(SESSION_FILE);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления сессии: {ex.Message}");
            }
        }
        
        public bool UserExists(string email)
        {
            return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}