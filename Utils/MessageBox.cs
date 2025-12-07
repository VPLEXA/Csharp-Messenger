using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace MessengerClient
{
    public static class MessageBox
    {
        public static async Task ShowAsync(Window parent, string message, string title)
        {
            var dialog = new Window
            {
                Title = title,
                Content = new TextBlock { Text = message, Margin = new Thickness(20) },
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            
            await dialog.ShowDialog(parent);
        }
    }
}