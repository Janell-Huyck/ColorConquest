using ColorConquest.Core;

namespace ColorConquest.Core.Services;

public static class GameBoardPreferences
{
    public static BoardSize GetBoardSize() => BoardSize.Medium; // TODO: Implement persistent storage
    public static void SetBoardSize(BoardSize boardSize) { /* TODO: Implement persistent storage */ }
    public static (int Rows, int Columns) GetBoardDimensions() => BoardSizeExtensions.GetDimensions(GetBoardSize());
}
