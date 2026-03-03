using System.Globalization;

namespace ColorConquest.Converters;

/// <summary>
/// Converts Cell.IsPrimaryColor (bool) to a Color for the tile background.
/// True -> app Primary color; False -> app Secondary color.
/// </summary>
public class IsPrimaryToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool isPrimary)
            return Colors.Gray;
        var key = isPrimary ? "Primary" : "Secondary";
        if (Application.Current?.Resources.TryGetValue(key, out var resource) == true && resource is Color color)
            return color;
        return isPrimary ? Color.FromArgb("#512BD4") : Color.FromArgb("#DFD8F7");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
