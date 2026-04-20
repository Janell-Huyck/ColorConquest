namespace ColorConquest.Core.Services;

public enum UserTheme { Light, Dark }

public static class ThemePreferences
{
    public static UserTheme GetSavedTheme() => UserTheme.Light; // TODO: Implement persistent storage
    public static void SaveTheme(UserTheme theme) { /* TODO: Implement persistent storage */ }
}
