using System.Globalization;
using ColorConquest.Services;

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

        return isPrimary
            ? Color.FromArgb(AppServices.TileColorPreferences.GetPrimaryColor().Hex)
            : Color.FromArgb(AppServices.TileColorPreferences.GetSecondaryColor().Hex);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();

}
