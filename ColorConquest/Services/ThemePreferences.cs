namespace ColorConquest.Services;

public class ThemePreferences
{
    private const string ThemePreferenceKey = "theme_preference";
    private const string LightThemeValue = "light";
    private const string DarkThemeValue = "dark";

    private readonly ColorConquest.Core.Services.IPreferences _preferences;

    public ThemePreferences(ColorConquest.Core.Services.IPreferences preferences)
    {
        _preferences = preferences;
    }

    public AppTheme GetSavedTheme()
    {
        var savedTheme = _preferences.Get(ThemePreferenceKey, LightThemeValue);
        return savedTheme == LightThemeValue ? AppTheme.Light : AppTheme.Dark;
    }

    public void SaveTheme(AppTheme theme)
    {
        var value = theme == AppTheme.Light ? LightThemeValue : DarkThemeValue;
        _preferences.Set(ThemePreferenceKey, value);
    }

    public void Reset() => _preferences.Set(ThemePreferenceKey, LightThemeValue);
}
