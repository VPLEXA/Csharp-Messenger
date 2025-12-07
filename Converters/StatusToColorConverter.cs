using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MessengerClient.Models;

namespace MessengerClient.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is UserStatus status)
            {
                return status switch
                {
                    UserStatus.Online => Brushes.Green,
                    UserStatus.Offline => Brushes.Gray,
                    UserStatus.DoNotDisturb => Brushes.Red,
                    UserStatus.Away => Brushes.Orange,
                    _ => Brushes.Gray
                };
            }
            return Brushes.Gray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}