using ColorConquest.Core;
using ColorConquest.Core.Models;
using Xunit;

namespace ColorConquest.Tests.Core.Models;

public class BoardTests
{
    // ---- Construction & dimensions ----

    [Fact]
    public void Constructor_WithRowsAndColumns_SetsRowCountAndColumnCount()
    {
        var board = new Board(rows: 4, columns: 6);

        Assert.Equal(4, board.RowCount);
        Assert.Equal(6, board.ColumnCount);
    }

    [Fact]
    public void Constructor_WithNoArguments_UsesDefaultRowCountAndColumnCount_5x5()
    {
        var board = new Board();

        Assert.Equal(GameConstants.DefaultRowCount, board.RowCount);
        Assert.Equal(GameConstants.DefaultColumnCount, board.ColumnCount);
    }

    [Fact]
    public void Constructor_CreatesCorrectNumberOfCells()
    {
        var board = new Board(rows: 3, columns: 4);

        var cellCount = 0;
        for (var row = 0; row < board.RowCount; row++)
        for (var column = 0; column < board.ColumnCount; column++)
        {
            _ = board.GetCell(row, column);
            cellCount++;
        }

        Assert.Equal(3 * 4, cellCount);
    }

    // ---- All cells initialized to primary color ----

    [Fact]
    public void Constructor_InitializesAllCellsToPrimaryColor()
    {
        var board = new Board(rows: 2, columns: 3);

        for (var row = 0; row < board.RowCount; row++)
        for (var column = 0; column < board.ColumnCount; column++)
        {
            var cell = board.GetCell(row, column);
            Assert.True(cell.IsPrimaryColor);
        }
    }

    // ---- Each cell has correct position ----

    [Fact]
    public void GetCell_ReturnsCellWithMatchingRowAndColumn()
    {
        var board = new Board(rows: 5, columns: 5);

        var cell = board.GetCell(2, 3);

        Assert.Equal(2, cell.Row);
        Assert.Equal(3, cell.Column);
    }

    [Fact]
    public void GetCell_EachPositionReturnsSameCellInstance()
    {
        var board = new Board(rows: 2, columns: 2);

        var first = board.GetCell(0, 1);
        var second = board.GetCell(0, 1);

        Assert.Same(first, second);
    }

    [Fact]
    public void GetCell_DifferentPositionsReturnDifferentCells()
    {
        var board = new Board(rows: 2, columns: 2);

        var cell00 = board.GetCell(0, 0);
        var cell01 = board.GetCell(0, 1);
        var cell10 = board.GetCell(1, 0);
        var cell11 = board.GetCell(1, 1);

        Assert.NotSame(cell00, cell01);
        Assert.NotSame(cell00, cell10);
        Assert.NotSame(cell00, cell11);
        Assert.NotSame(cell01, cell10);
        Assert.NotSame(cell01, cell11);
        Assert.NotSame(cell10, cell11);
    }

    // ---- Constructor validation ----

    [Fact]
    public void Constructor_WhenRowsZero_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Board(rows: 0, columns: 5));
    }

    [Fact]
    public void Constructor_WhenColumnsZero_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Board(rows: 5, columns: 0));
    }

    [Fact]
    public void Constructor_WhenRowsNegative_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Board(rows: -1, columns: 5));
    }

    [Fact]
    public void Constructor_WhenColumnsNegative_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Board(rows: 5, columns: -1));
    }

    // ---- GetCell validation ----

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(5, 0)]
    [InlineData(0, 5)]
    public void GetCell_OutOfRange_ThrowsArgumentOutOfRangeException(int row, int column)
    {
        var board = new Board(rows: 5, columns: 5);

        Assert.Throws<ArgumentOutOfRangeException>(() => board.GetCell(row, column));
    }

    // ---- Initial state & reset ----

    [Fact]
    public void ResetToInitialState_AfterChangingCells_RestoresAllToPrimary()
    {
        var board = new Board(rows: 2, columns: 2);
        board.GetCell(0, 0).IsPrimaryColor = false;
        board.GetCell(1, 1).IsPrimaryColor = false;

        board.ResetToInitialState();

        Assert.True(board.GetCell(0, 0).IsPrimaryColor);
        Assert.True(board.GetCell(0, 1).IsPrimaryColor);
        Assert.True(board.GetCell(1, 0).IsPrimaryColor);
        Assert.True(board.GetCell(1, 1).IsPrimaryColor);
    }

    [Fact]
    public void ResetToInitialState_WhenNoChanges_LeavesAllPrimary()
    {
        var board = new Board(rows: 2, columns: 2);

        board.ResetToInitialState();

        for (var row = 0; row < board.RowCount; row++)
        for (var column = 0; column < board.ColumnCount; column++)
            Assert.True(board.GetCell(row, column).IsPrimaryColor);
    }

    // ---- Toggle cell and adjacent cells ----

    [Fact]
    public void ToggleCellAndAdjacent_WhenCenterCell_TogglesCenterAndFourNeighbors()
    {
        var board = new Board(rows: 3, columns: 3);

        board.ToggleCellAndAdjacent(1, 1);

        Assert.False(board.GetCell(1, 1).IsPrimaryColor); // center
        Assert.False(board.GetCell(0, 1).IsPrimaryColor); // up
        Assert.False(board.GetCell(2, 1).IsPrimaryColor); // down
        Assert.False(board.GetCell(1, 0).IsPrimaryColor); // left
        Assert.False(board.GetCell(1, 2).IsPrimaryColor); // right

        Assert.True(board.GetCell(0, 0).IsPrimaryColor);  // corners unchanged
        Assert.True(board.GetCell(0, 2).IsPrimaryColor);
        Assert.True(board.GetCell(2, 0).IsPrimaryColor);
        Assert.True(board.GetCell(2, 2).IsPrimaryColor);
    }

    [Fact]
    public void ToggleCellAndAdjacent_WhenTopLeftCorner_TogglesOnlyExistingNeighbors()
    {
        var board = new Board(rows: 3, columns: 3);

        board.ToggleCellAndAdjacent(0, 0);

        Assert.False(board.GetCell(0, 0).IsPrimaryColor); // corner
        Assert.False(board.GetCell(0, 1).IsPrimaryColor); // right
        Assert.False(board.GetCell(1, 0).IsPrimaryColor); // down

        Assert.True(board.GetCell(1, 1).IsPrimaryColor);  // diagonals and others unchanged
        Assert.True(board.GetCell(0, 2).IsPrimaryColor);
        Assert.True(board.GetCell(2, 2).IsPrimaryColor);
    }

    [Fact]
    public void ToggleCellAndAdjacent_WhenTopEdgeNonCorner_TogglesCenterAndThreeNeighbors()
    {
        var board = new Board(rows: 3, columns: 3);

        board.ToggleCellAndAdjacent(0, 1);

        Assert.False(board.GetCell(0, 1).IsPrimaryColor); // edge
        Assert.False(board.GetCell(0, 0).IsPrimaryColor); // left
        Assert.False(board.GetCell(0, 2).IsPrimaryColor); // right
        Assert.False(board.GetCell(1, 1).IsPrimaryColor); // down

        Assert.True(board.GetCell(1, 0).IsPrimaryColor);  // others unchanged
        Assert.True(board.GetCell(1, 2).IsPrimaryColor);
        Assert.True(board.GetCell(2, 1).IsPrimaryColor);
    }

    // ---- Move count & win detection ----

    [Fact]
    public void MoveCount_IsZeroBeforeAnyMoves()
    {
        var board = new Board(rows: 3, columns: 3);

        Assert.Equal(0, board.MoveCount);
    }

    [Fact]
    public void MoveCount_IncrementsByOnePerToggleCellAndAdjacentCall()
    {
        var board = new Board(rows: 3, columns: 3);

        board.ToggleCellAndAdjacent(1, 1);
        board.ToggleCellAndAdjacent(0, 0);

        Assert.Equal(2, board.MoveCount);
    }

    [Fact]
    public void AreAllCellsSameColor_WhenAllPrimary_ReturnsTrue()
    {
        var board = new Board(rows: 2, columns: 2);

        Assert.True(board.AreAllCellsSameColor());
    }

    [Fact]
    public void AreAllCellsSameColor_WhenMixed_ReturnsFalse()
    {
        var board = new Board(rows: 2, columns: 2);
        board.GetCell(0, 0).IsPrimaryColor = false;

        Assert.False(board.AreAllCellsSameColor());
    }

    [Fact]
    public void AreAllCellsSameColor_WhenAllSecondary_ReturnsTrue()
    {
        var board = new Board(rows: 2, columns: 2);

        for (var row = 0; row < board.RowCount; row++)
        for (var column = 0; column < board.ColumnCount; column++)
            board.GetCell(row, column).IsPrimaryColor = false;

        Assert.True(board.AreAllCellsSameColor());
    }
}
