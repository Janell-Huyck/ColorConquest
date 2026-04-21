using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using ColorConquest.Core;

namespace ColorConquest.Core.Converters
{
    public class BoardSizeToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is BoardSize boardSize && parameter is BoardSize paramSize)
            {
                return boardSize == paramSize;
            }
            // Fallback: try to parse string
            if (value is BoardSize boardSize2 && parameter is string paramStr && Enum.TryParse<BoardSize>(paramStr, out var paramEnum))
            {
                return boardSize2 == paramEnum;
            }
            return false;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked)
            {
                if (parameter is BoardSize paramSize)
                    return paramSize;
                if (parameter is string paramStr && Enum.TryParse(typeof(BoardSize), paramStr, out var result))
                    return result;
            }
            return Binding.DoNothing;
        }
    }
}
