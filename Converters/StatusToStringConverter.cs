using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MessengerClient.Models;

namespace MessengerClient.Converters
{
    public class StatusToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is UserStatus status)
            {
                return status switch
                {
                    UserStatus.Online => "Онлайн",
                    UserStatus.Offline => "Офлайн",
                    UserStatus.DoNotDisturb => "Не беспокоить",
                    UserStatus.Away => "Отошел",
                    _ => status.ToString()
                };
            }
            return value?.ToString() ?? "";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

