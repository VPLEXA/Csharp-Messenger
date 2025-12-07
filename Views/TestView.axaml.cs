using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MessengerClient.Views
{
    public partial class TestView : UserControl
    {
        private int _counter = 0;
        private Button? _addButton;
        private StackPanel? _testList;
        private TextBlock? _statusText;
        
        public TestView()
        {
            InitializeComponent();
            
            // Ждем инициализации
            this.Initialized += (s, e) =>
            {
                _addButton = this.FindControl<Button>("AddButton");
                _testList = this.FindControl<StackPanel>("TestList");
                _statusText = this.FindControl<TextBlock>("StatusText");
                
                if (_addButton != null)
                {
                    _addButton.Click += AddButton_Click;
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_testList == null || _statusText == null) return;
            
            _counter++;
            var item = new Border
            {
                Background = Avalonia.Media.Brushes.LightGray,
                Margin = new Avalonia.Thickness(0, 2),
                Padding = new Avalonia.Thickness(10)
            };
            
            item.Child = new TextBlock 
            { 
                Text = $"Test Item {_counter}" 
            };
            
            _testList.Children.Add(item);
            _statusText.Text = $"Added item {_counter}";
        }
    }
}