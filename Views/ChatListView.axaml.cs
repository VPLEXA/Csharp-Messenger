using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MessengerClient.ViewModels;

namespace MessengerClient.Views
{
    public partial class ChatListView : UserControl
    {
        public ChatListView()
        {
            InitializeComponent();

            // Отладочная информация при загрузке
            this.Initialized += (s, e) =>
            {
                Console.WriteLine("ChatListView Initialized");
                Console.WriteLine($"DataContext is null: {DataContext == null}");

                if (DataContext is ChatListViewModel vm)
                {
                    Console.WriteLine($"ChatListViewModel has {vm.Chats.Count} chats");
                    foreach (var chat in vm.Chats)
                    {
                        Console.WriteLine($"  - {chat.Name}");
                    }
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Test button clicked!");

            if (DataContext is ChatListViewModel vm)
            {
                Console.WriteLine($"Chats count: {vm.Chats.Count}");
            }
        }
        private void OpenChat1_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Opening chat 1...");
            
            if (DataContext is ChatListViewModel vm && vm.Chats.Count > 0)
            {
                vm.SelectedChat = vm.Chats[0];
            }
        }
    }
}