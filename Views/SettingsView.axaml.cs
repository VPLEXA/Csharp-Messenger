using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using MessengerClient.Services;

namespace MessengerClient.Views
{
    public class SettingsView : UserControl
    {
        public SettingsView()
        {
            var stackPanel = new StackPanel
            {
                Spacing = 20,
                Margin = new Thickness(20),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var title = new TextBlock
            {
                Text = "⚙️ НАСТРОЙКИ",
                FontSize = 24,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stackPanel.Children.Add(title);

            var backBtn = new Button
            {
                Content = "← Назад",
                Width = 200,
                Height = 40
            };
            backBtn.Click += (s, e) =>
            {
                var window = this.FindAncestorOfType<Window>();
                if (window?.DataContext is AppController controller)
                {
                    controller.ShowProfile();
                }
            };
            stackPanel.Children.Add(backBtn);

            this.Content = stackPanel;
        }
    }
}