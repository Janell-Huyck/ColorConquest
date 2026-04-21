namespace ColorConquest.Core.Services;

public class GameDisplayPreferences
{
    private const string ShowMoveCountKey = "show_move_count";
    private const string ShowGameTimerKey = "show_game_timer";

    private readonly IPreferences _preferences;

    public GameDisplayPreferences(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public bool GetShowMoveCount() => _preferences.Get(ShowMoveCountKey, "true") == "true";
    public void SetShowMoveCount(bool value) => _preferences.Set(ShowMoveCountKey, value ? "true" : "false");
    public bool GetShowGameTimer() => _preferences.Get(ShowGameTimerKey, "true") == "true";
    public void SetShowGameTimer(bool value) => _preferences.Set(ShowGameTimerKey, value ? "true" : "false");

    public void Reset()
    {
        _preferences.Set(ShowMoveCountKey, "true");
        _preferences.Set(ShowGameTimerKey, "true");
    }
}
