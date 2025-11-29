using Avalonia.Controls;
using MessengerClient.Services;

namespace MessengerClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public StateManager StateManager { get; }
        public NavigationService NavigationService { get; }

        public MainWindowViewModel(ContentControl content)
        {
            StateManager = new StateManager();
            NavigationService = new NavigationService(content, this);
            NavigationService.NavigateToLogin();
        }
    }
}