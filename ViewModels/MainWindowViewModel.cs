using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MessengerClient.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object? _currentViewModel;

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}