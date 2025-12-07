using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MessengerClient.Views
{
    public partial class ChangePasswordView : UserControl
    {
        public ChangePasswordView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

