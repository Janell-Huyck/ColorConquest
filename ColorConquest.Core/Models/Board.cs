using ColorConquest.Core;

namespace ColorConquest.Core.Models;

/// <summary>
/// Game board: a grid of cells. Supports configurable dimensions (default 5x5).
/// Each cell stores its own initial state; use ResetToInitialState() to restore all.
/// </summary>
public class Board
{
    private readonly Cell[,] _cells;

    public Board() : this(GameConstants.DefaultRowCount, GameConstants.DefaultColumnCount)
    {
    }

    public Board(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentOutOfRangeException(nameof(rows), "Rows and columns must be positive.");

        RowCount = rows;
        ColumnCount = columns;
        _cells = new Cell[rows, columns];

        for (var rowIndex = 0; rowIndex < rows; rowIndex++)
        for (var columnIndex = 0; columnIndex < columns; columnIndex++)
            _cells[rowIndex, columnIndex] = new Cell(rowIndex, columnIndex, initialIsPrimaryColor: true);
    }

    public int RowCount { get; }
    public int ColumnCount { get; }

    /// <summary>
    /// Number of moves applied to this board (one per ToggleCellAndAdjacent call).
    /// </summary>
    public int MoveCount { get; private set; }

    public Cell GetCell(int row, int column)
    {
        if (row < 0 || row >= RowCount)
            throw new ArgumentOutOfRangeException(nameof(row));
        if (column < 0 || column >= ColumnCount)
            throw new ArgumentOutOfRangeException(nameof(column));

        return _cells[row, column];
    }

    /// <summary>
    /// Toggles the cell at (row, column) and any orthogonally adjacent cells
    /// (up, down, left, right) that exist within the board.
    /// </summary>
    public void ToggleCellAndAdjacent(int row, int column)
    {
        // Will throw for invalid row/column just like GetCell.
        if (row < 0 || row >= RowCount)
            throw new ArgumentOutOfRangeException(nameof(row));
        if (column < 0 || column >= ColumnCount)
            throw new ArgumentOutOfRangeException(nameof(column));

        var neighborOffsets = new (int RowOffset, int ColumnOffset)[]
        {
            (0, 0),   // center
            (-1, 0),  // up
            (1, 0),   // down
            (0, -1),  // left
            (0, 1)    // right
        };

        foreach (var (rowOffset, columnOffset) in neighborOffsets)
        {
            var neighborRow = row + rowOffset;
            var neighborColumn = column + columnOffset;

            if (neighborRow < 0 || neighborRow >= RowCount) continue;
            if (neighborColumn < 0 || neighborColumn >= ColumnCount) continue;

            _cells[neighborRow, neighborColumn].Toggle();
        }

        MoveCount++;
    }

    /// <summary>
    /// Returns true when every cell has the same IsPrimaryColor value (all primary or all secondary).
    /// </summary>
    public bool AreAllCellsSameColor()
    {
        var firstCell = _cells[0, 0];
        var targetColor = firstCell.IsPrimaryColor;

        for (var row = 0; row < RowCount; row++)
        for (var column = 0; column < ColumnCount; column++)
        {
            if (_cells[row, column].IsPrimaryColor != targetColor)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Restores every cell to its initial state.
    /// </summary>
    public void ResetToInitialState()
    {
        for (var row = 0; row < RowCount; row++)
        for (var column = 0; column < ColumnCount; column++)
            _cells[row, column].ResetToInitial();
    }
}
