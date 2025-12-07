using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MessengerClient.Views
{
    public partial class ForgotPasswordView : UserControl
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

