using ColorConquest.Core.Services;
using Microsoft.Maui.ApplicationModel;
namespace ColorConquest.Services;

/// <summary>
/// Applies theme colors to Shell. XAML <c>AppThemeBinding</c> in global styles follows the OS theme
/// on some platforms (e.g. Mac Catalyst), not the in-app toggle. We set explicit Shell colors from
/// the same preference the Settings page saves.
/// </summary>
public static class ThemeChrome
{
    public static AppTheme ToAppTheme(UserTheme userTheme) => userTheme == UserTheme.Dark ? AppTheme.Dark : AppTheme.Light;
    public static UserTheme ToUserTheme(AppTheme appTheme) => appTheme == AppTheme.Dark ? UserTheme.Dark : UserTheme.Light;
    private static readonly Color LightPage = Colors.White;
    private static readonly Color DarkPage = Color.FromArgb("#1F1F1F");
    private static readonly Color FlyoutLight = Colors.White;

    public static bool IsDarkFromPreferences() => AppServices.ThemePreferences.GetSavedTheme() == UserTheme.Dark;

    /// <summary>Shared page/card surface (same value Shell uses behind content in dark mode).</summary>
    public static Color Surface(bool dark) => dark ? DarkPage : LightPage;

    /// <summary>Call after app start and whenever the user changes the in-app theme.</summary>
    public static void ApplyToApplication()
    {
        var app = Application.Current;
        if (app?.MainPage is not Shell shell)
            return;

        var dark = IsDarkFromPreferences();

        // Page.BackgroundColor on Shell drives the main content area behind flyout pages.
        shell.BackgroundColor = dark ? DarkPage : LightPage;
        shell.FlyoutBackgroundColor = dark ? DarkPage : FlyoutLight;
    }
}
