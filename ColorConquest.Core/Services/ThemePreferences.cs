namespace ColorConquest.Core.Services;

public enum UserTheme { Light, Dark }

public class ThemePreferences
{
    private const string ThemeKey = "theme_preference";
    private const string LightValue = "light";
    private const string DarkValue = "dark";

    private readonly IPreferences _preferences;

    public ThemePreferences(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public UserTheme GetSavedTheme()
    {
        var value = _preferences.Get(ThemeKey, LightValue);
        return value == DarkValue ? UserTheme.Dark : UserTheme.Light;
    }

    public void SaveTheme(UserTheme theme)
    {
        _preferences.Set(ThemeKey, theme == UserTheme.Dark ? DarkValue : LightValue);
    }

    public void Reset() => _preferences.Set(ThemeKey, LightValue);
}
