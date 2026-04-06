namespace ColorConquest.Services;

public static class GameDisplayPreferences
{
    private const string ShowMoveCountKey = "show_move_count";

    public static bool GetShowMoveCount() => Preferences.Default.Get(ShowMoveCountKey, true);

    public static void SetShowMoveCount(bool value) => Preferences.Default.Set(ShowMoveCountKey, value);
}
