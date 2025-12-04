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
            CreateUI();
        }

        private void CreateUI()
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
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush(Color.Parse("#2b579a"))
            };
            stackPanel.Children.Add(title);

            AddSetting(stackPanel, "🔔 Уведомления", "Включены");
            AddSetting(stackPanel, "🎨 Тема", "Светлая");
            AddSetting(stackPanel, "🔒 Приватность", "Только друзья");
            AddSetting(stackPanel, "📱 Использование данных", "Обычное");
            AddSetting(stackPanel, "🌐 Язык", "Русский");

            var buttonsStack = new StackPanel
            {
                Spacing = 10,
                Margin = new Thickness(0, 30, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var saveBtn = CreateButton("💾 Сохранить настройки", "#2b579a");
            saveBtn.Click += (s, e) => System.Diagnostics.Debug.WriteLine("Настройки сохранены");
            buttonsStack.Children.Add(saveBtn);

            var backBtn = CreateButton("← Назад в профиль", "#95a5a6");
            backBtn.Click += (s, e) =>
            {
                var window = this.FindAncestorOfType<Window>();
                if (window?.DataContext is AppController controller)
                {
                    controller.ShowProfile();
                }
            };
            buttonsStack.Children.Add(backBtn);

            stackPanel.Children.Add(buttonsStack);

            this.Content = stackPanel;
        }
        
        private void AddSetting(StackPanel panel, string name, string value)
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var nameText = new TextBlock
            {
                Text = name,
                FontWeight = FontWeight.SemiBold,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(nameText, 0);
            grid.Children.Add(nameText);

            var valueBtn = new Button
            {
                Content = value,
                Background = Brushes.Transparent,
                Foreground = new SolidColorBrush(Color.Parse("#3498db")),
                BorderThickness = new Thickness(0),
                FontWeight = FontWeight.SemiBold
            };
            valueBtn.Click += (s, e) => System.Diagnostics.Debug.WriteLine($"Изменить {name}");
            Grid.SetColumn(valueBtn, 1);
            grid.Children.Add(valueBtn);

            panel.Children.Add(grid);
        }
        
        private Button CreateButton(string text, string color)
        {
            return new Button
            {
                Content = text,
                Width = 250,
                Height = 45,
                Background = new SolidColorBrush(Color.Parse(color)),
                Foreground = Brushes.White,
                FontWeight = FontWeight.Bold,
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }
    }
}