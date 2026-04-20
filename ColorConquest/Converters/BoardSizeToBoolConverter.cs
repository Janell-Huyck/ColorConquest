using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using ColorConquest.Core;

namespace ColorConquest.Converters
{
    public class BoardSizeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BoardSize boardSize && parameter is string param)
            {
                return boardSize.ToString() == param;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string param)
            {
                if (Enum.TryParse(typeof(BoardSize), param, out var result))
                    return result;
            }
            return Binding.DoNothing;
        }
    }
}
