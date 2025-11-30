using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MessengerClient.Services
{
    public static class StringConverters
    {
        public static readonly IValueConverter ToUpperFirstLetter = new FuncValueConverter<string, string>(
            value => string.IsNullOrEmpty(value) ? string.Empty : char.ToUpper(value[0]) + value.Substring(1).ToLower());
    }

    public static class ObjectConverters
    {
        public static readonly IValueConverter IsGreaterThanZero = new FuncValueConverter<int, bool>(
            value => value > 0);
    }
}