namespace ColorConquest.Core;

/// <summary>Maps to board dimensions: Easy 3×3, Medium 5×5, Hard 9×9.</summary>
public enum BoardSize
{
    Easy,
    Medium,
    Hard
}

public static class BoardSizeExtensions
{
    public static (int Rows, int Columns) GetDimensions(BoardSize size) =>
        size switch
        {
            BoardSize.Easy => (3, 3),
            BoardSize.Medium => (5, 5),
            BoardSize.Hard => (9, 9),
            _ => (5, 5)
        };

    /// <summary>Scaled from 15 random toggles on a 5×5 board.</summary>
    public static int ScrambleMoveCount(int rows, int columns) =>
        Math.Max(8, rows * columns * 15 / 25);
}
