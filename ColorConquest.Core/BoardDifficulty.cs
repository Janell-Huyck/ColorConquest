namespace ColorConquest.Core;

/// <summary>Maps to board dimensions: Easy 3×3, Medium 5×5, Hard 9×9.</summary>
public enum BoardDifficulty
{
    Easy,
    Medium,
    Hard
}

public static class BoardDifficultySizes
{
    public static (int Rows, int Columns) GetDimensions(BoardDifficulty difficulty) =>
        difficulty switch
        {
            BoardDifficulty.Easy => (3, 3),
            BoardDifficulty.Medium => (5, 5),
            BoardDifficulty.Hard => (9, 9),
            _ => (5, 5)
        };

    /// <summary>Scaled from 15 random toggles on a 5×5 board.</summary>
    public static int ScrambleMoveCount(int rows, int columns) =>
        Math.Max(8, rows * columns * 15 / 25);
}
