using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MessengerClient.Services
{
    public static class StringConverters
    {
        public static readonly IValueConverter FirstLetter = new FuncValueConverter<string, string>(
            value => string.IsNullOrEmpty(value) ? "?" : value[0].ToString().ToUpper());
    }
}

public static class BoolConverters
{
    public static readonly IValueConverter IsGreaterThanZero = new FuncValueConverter<int, bool>(
        value => value > 0);
}