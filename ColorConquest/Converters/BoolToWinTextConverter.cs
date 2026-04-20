using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ColorConquest.Converters
{
    public class BoolToWinTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isWon && isWon)
                return "You win!";
            return " ";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
