using ColorConquest.Core;

namespace ColorConquest.Services;

public static class GameBoardPreferences
{
    private const string BoardSizeKey = "board_size";
    private const string EasyValue = "easy";
    private const string MediumValue = "medium";
    private const string HardValue = "hard";

    public static BoardSize GetBoardSize()
    {
        var saved = Preferences.Default.Get(BoardSizeKey, MediumValue);
        return saved switch
        {
            EasyValue => BoardSize.Easy,
            HardValue => BoardSize.Hard,
            _ => BoardSize.Medium
        };
    }

    public static void SetBoardSize(BoardSize boardSize)
    {
        var value = boardSize switch
        {
            BoardSize.Easy => EasyValue,
            BoardSize.Hard => HardValue,
            _ => MediumValue
        };
        Preferences.Default.Set(BoardSizeKey, value);
    }

    public static (int Rows, int Columns) GetBoardDimensions() =>
        BoardSizeExtensions.GetDimensions(GetBoardSize());
}
