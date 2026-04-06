namespace ColorConquest.Services;

public static class GameDisplayPreferences
{
    private const string ShowMoveCountKey = "show_move_count";
    private const string ShowGameTimerKey = "show_game_timer";

    public static bool GetShowMoveCount() => Preferences.Default.Get(ShowMoveCountKey, true);

    public static void SetShowMoveCount(bool value) => Preferences.Default.Set(ShowMoveCountKey, value);

    public static bool GetShowGameTimer() => Preferences.Default.Get(ShowGameTimerKey, true);

    public static void SetShowGameTimer(bool value) => Preferences.Default.Set(ShowGameTimerKey, value);
}
