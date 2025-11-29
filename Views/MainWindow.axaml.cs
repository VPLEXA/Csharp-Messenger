using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MessengerClient.ViewModels;

namespace MessengerClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Находим ContentControl после инициализации
            var mainContent = this.FindControl<ContentControl>("MainContent");
            
            // Передаем ContentControl в ViewModel для навигации
            var viewModel = new MainWindowViewModel(mainContent);
            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}