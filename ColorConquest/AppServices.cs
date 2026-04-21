using ColorConquest.Services;
using ColorConquest.Core.Services;

namespace ColorConquest
{
    public static class AppServices
    {
        public static ColorConquest.Core.Services.IPreferences Preferences { get; } = new ColorConquest.Core.Services.PersistentPreferences();
        public static ColorConquest.Core.Services.ThemePreferences ThemePreferences { get; } = new ColorConquest.Core.Services.ThemePreferences(Preferences);
        public static ColorConquest.Core.Services.GameBoardPreferences GameBoardPreferences { get; } = new ColorConquest.Core.Services.GameBoardPreferences(Preferences);
        public static ColorConquest.Core.Services.GameDisplayPreferences GameDisplayPreferences { get; } = new ColorConquest.Core.Services.GameDisplayPreferences(Preferences);
        public static ColorConquest.Core.Services.TileColorPreferences TileColorPreferences { get; } = new ColorConquest.Core.Services.TileColorPreferences(Preferences);

        // Global theme changed event
        public static event Action<bool>? ThemeChanged;

        public static void RaiseThemeChanged(bool isDark)
        {
            ThemeChanged?.Invoke(isDark);
        }
    }
}
