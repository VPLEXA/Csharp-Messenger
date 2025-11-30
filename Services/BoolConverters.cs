using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MessengerClient.Services
{
    public static class BoolConverters
    {
        public static readonly IValueConverter IsGreaterThanZero = new FuncValueConverter<int, bool>(
            value => value > 0);
    }
}