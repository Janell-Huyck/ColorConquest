using ColorConquest.Core;
using ColorConquest.Core.Models;
using ColorConquest.Core.ViewModels;
using Xunit;

namespace ColorConquest.Tests.Core.ViewModels;

public class GameViewModelTests
{
    // ---- Initialization ----

    [Fact]
    public void Constructor_CreatesCellsWithCountEqualToRowCountTimesColumnCount()
    {
        var viewModel = new GameViewModel();

        Assert.Equal(
            GameConstants.DefaultRowCount * GameConstants.DefaultColumnCount,
            viewModel.Cells.Count);
    }

    [Fact]
    public void RowCount_ReturnsBoardRowCount()
    {
        var viewModel = new GameViewModel();

        Assert.Equal(GameConstants.DefaultRowCount, viewModel.RowCount);
    }

    [Fact]
    public void ColumnCount_ReturnsBoardColumnCount()
    {
        var viewModel = new GameViewModel();

        Assert.Equal(GameConstants.DefaultColumnCount, viewModel.ColumnCount);
    }

    [Fact]
    public void Constructor_CellsAreInRowMajorOrder()
    {
        var viewModel = new GameViewModel();
        var cells = viewModel.Cells;

        // First row: (0,0), (0,1), (0,2), (0,3), (0,4)
        Assert.Equal(0, cells[0].Row);
        Assert.Equal(0, cells[0].Column);
        Assert.Equal(0, cells[4].Row);
        Assert.Equal(4, cells[4].Column);
        // Second row starts at index 5
        Assert.Equal(1, cells[5].Row);
        Assert.Equal(0, cells[5].Column);
    }

    // ---- CellTappedCommand ----

    [Fact]
    public void CellTappedCommand_WhenExecutedWithCell_TogglesThatCell()
    {
        var viewModel = new GameViewModel();
        var cell = viewModel.Cells[0];
        Assert.True(cell.IsPrimaryColor);

        viewModel.CellTappedCommand.Execute(cell);

        Assert.False(cell.IsPrimaryColor);
    }

    [Fact]
    public void CellTappedCommand_WhenExecutedWithNull_DoesNotThrow()
    {
        var viewModel = new GameViewModel();

        var ex = Record.Exception(() => viewModel.CellTappedCommand.Execute(null));

        Assert.Null(ex);
    }

    [Fact]
    public void CellTappedCommand_WhenExecutedTwiceWithSameCell_ReturnsToOriginalState()
    {
        var viewModel = new GameViewModel();
        var tappedCell = viewModel.Cells[0];      // (row 0, col 0)
        var rightNeighbor = viewModel.Cells[1];   // (row 0, col 1)
        var belowNeighbor = viewModel.Cells[5];   // (row 1, col 0)

        var tappedInitial = tappedCell.IsPrimaryColor;
        var rightInitial = rightNeighbor.IsPrimaryColor;
        var belowInitial = belowNeighbor.IsPrimaryColor;

        viewModel.CellTappedCommand.Execute(tappedCell);
        viewModel.CellTappedCommand.Execute(tappedCell);

        Assert.Equal(tappedInitial, tappedCell.IsPrimaryColor);
        Assert.Equal(rightInitial, rightNeighbor.IsPrimaryColor);
        Assert.Equal(belowInitial, belowNeighbor.IsPrimaryColor);
    }

    // ---- ResetCommand ----

    [Fact]
    public void ResetCommand_WhenExecuted_RestoresAllCellsToInitialState()
    {
        var viewModel = new GameViewModel();
        viewModel.Cells[0].IsPrimaryColor = false;
        viewModel.Cells[5].IsPrimaryColor = false;

        viewModel.ResetCommand.Execute(null);

        foreach (var cell in viewModel.Cells)
            Assert.True(cell.IsPrimaryColor);
    }

    // ---- MoveCount & win detection ----

    [Fact]
    public void MoveCount_ExposedFromBoard()
    {
        var board = new Board(rows: 1, columns: 1);
        var viewModel = new GameViewModel(board);
        var cell = viewModel.Cells[0];

        Assert.Equal(0, viewModel.MoveCount);

        viewModel.CellTappedCommand.Execute(cell);
        viewModel.CellTappedCommand.Execute(cell);

        Assert.Equal(2, viewModel.MoveCount);
    }

    [Fact]
    public void IsWon_IsFalseBeforeAnyMove_EvenThoughAllCellsSame()
    {
        var board = new Board(rows: 1, columns: 1);
        var viewModel = new GameViewModel(board);

        Assert.False(viewModel.IsWon);
    }

    [Fact]
    public void IsWon_BecomesTrue_WhenAllCellsSameColorAfterMove()
    {
        var board = new Board(rows: 1, columns: 1);
        var viewModel = new GameViewModel(board);
        var cell = viewModel.Cells[0];

        viewModel.CellTappedCommand.Execute(cell); // toggles the only cell

        Assert.True(viewModel.IsWon);
    }
}
