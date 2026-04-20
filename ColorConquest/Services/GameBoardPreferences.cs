using ColorConquest.Core;

namespace ColorConquest.Services;

public static class GameBoardPreferences
{
    private const string DifficultyKey = "board_difficulty";
    private const string EasyValue = "easy";
    private const string MediumValue = "medium";
    private const string HardValue = "hard";

    public static BoardDifficulty GetDifficulty()
    {
        var saved = Preferences.Default.Get(DifficultyKey, MediumValue);
        return saved switch
        {
            EasyValue => BoardDifficulty.Easy,
            HardValue => BoardDifficulty.Hard,
            _ => BoardDifficulty.Medium
        };
    }

    public static void SetDifficulty(BoardDifficulty difficulty)
    {
        var value = difficulty switch
        {
            BoardDifficulty.Easy => EasyValue,
            BoardDifficulty.Hard => HardValue,
            _ => MediumValue
        };
        Preferences.Default.Set(DifficultyKey, value);
    }

    public static (int Rows, int Columns) GetBoardDimensions() =>
        BoardDifficultySizes.GetDimensions(GetDifficulty());
}
