namespace ColorConquest.Services;

public static class ThemePreferences
{
    private const string ThemePreferenceKey = "theme_preference";
    private const string LightThemeValue = "light";
    private const string DarkThemeValue = "dark";

    public static AppTheme GetSavedTheme()
    {
        var savedTheme = Preferences.Default.Get(ThemePreferenceKey, LightThemeValue);
        return savedTheme == LightThemeValue ? AppTheme.Light : AppTheme.Dark;
    }

    public static void SaveTheme(AppTheme theme)
    {
        var value = theme == AppTheme.Light ? LightThemeValue : DarkThemeValue;
        Preferences.Default.Set(ThemePreferenceKey, value);
    }
}
