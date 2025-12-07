using System;
using MessengerClient.Models;

namespace MessengerClient.Services
{
    public class StateManager
    {
        private User? _currentUser;

        public User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                UserChanged?.Invoke(value);
            }
        }

        public event Action<User?>? UserChanged;
    }
}