using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MessengerClient.Services;

namespace MessengerClient.Views
{
    public partial class ProfileView : UserControl
    {
        private readonly AppController _controller;
        
        public ProfileView(AppController controller)
        {
            _controller = controller;
            InitializeComponent();
            
            var backBtn = this.FindControl<Button>("BackBtn");
            if (backBtn != null) backBtn.Click += (s, e) => _controller.ShowChatList();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}