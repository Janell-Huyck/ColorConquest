namespace ColorConquest.Core.Services;

public class PreferencesReset
{
    private readonly ThemePreferences _themePreferences;
    private readonly GameBoardPreferences _gameBoardPreferences;
    private readonly GameDisplayPreferences _gameDisplayPreferences;
    private readonly TileColorPreferences _tileColorPreferences;

    public PreferencesReset(
        ThemePreferences themePreferences,
        GameBoardPreferences gameBoardPreferences,
        GameDisplayPreferences gameDisplayPreferences,
        TileColorPreferences tileColorPreferences)
    {
        _themePreferences = themePreferences;
        _gameBoardPreferences = gameBoardPreferences;
        _gameDisplayPreferences = gameDisplayPreferences;
        _tileColorPreferences = tileColorPreferences;
    }

    public void ResetAll()
    {
        _themePreferences.Reset();
        _gameBoardPreferences.Reset();
        _gameDisplayPreferences.Reset();
        _tileColorPreferences.Reset();
    }
}
