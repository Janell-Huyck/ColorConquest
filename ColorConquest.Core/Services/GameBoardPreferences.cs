using ColorConquest.Core;

namespace ColorConquest.Core.Services;

public class GameBoardPreferences
{
    private const string BoardSizeKey = "board_size";
    private const string EasyValue = "easy";
    private const string MediumValue = "medium";
    private const string HardValue = "hard";

    private readonly IPreferences _preferences;

    public GameBoardPreferences(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public BoardSize GetBoardSize()
    {
        var value = _preferences.Get(BoardSizeKey, MediumValue);
        return value switch
        {
            EasyValue => BoardSize.Easy,
            HardValue => BoardSize.Hard,
            _ => BoardSize.Medium
        };
    }

    public void SetBoardSize(BoardSize boardSize)
    {
        var value = boardSize switch
        {
            BoardSize.Easy => EasyValue,
            BoardSize.Hard => HardValue,
            _ => MediumValue
        };
        _preferences.Set(BoardSizeKey, value);
    }

    public (int Rows, int Columns) GetBoardDimensions() => BoardSizeExtensions.GetDimensions(GetBoardSize());
    public void Reset() => _preferences.Set(BoardSizeKey, MediumValue);
}
