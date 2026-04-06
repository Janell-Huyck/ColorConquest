using System.ComponentModel;
using System.Threading;
using ColorConquest.Core;
using ColorConquest.Core.Models;
using ColorConquest.Core.ViewModels;
using Xunit;
using GameCell = ColorConquest.Core.Models.Cell;

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
        var board = new Board(rows: 5, columns: 5);
        var viewModel = new GameViewModel(board);
        var cell = viewModel.Cells[0];
        var initial = cell.IsPrimaryColor;

        viewModel.CellTappedCommand.Execute(cell);

        Assert.NotEqual(initial, cell.IsPrimaryColor);
    }

    [Fact]
    public void CellTappedCommand_WhenExecutedWithNull_DoesNotThrow()
    {
        var board = new Board(rows: 5, columns: 5);
        var viewModel = new GameViewModel(board);

        var ex = Record.Exception(() => viewModel.CellTappedCommand.Execute(null));

        Assert.Null(ex);
    }

    [Fact]
    public void CellTappedCommand_WhenExecutedTwiceWithSameCell_ReturnsToOriginalState()
    {
        var board = new Board(rows: 5, columns: 5);
        var viewModel = new GameViewModel(board);
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
        var board = new Board(rows: 5, columns: 5);
        var viewModel = new GameViewModel(board);
        viewModel.Cells[0].IsPrimaryColor = false;
        viewModel.Cells[5].IsPrimaryColor = false;

        viewModel.ResetCommand.Execute(null);

        foreach (var cell in viewModel.Cells)
            Assert.Equal(cell.InitialIsPrimaryColor, cell.IsPrimaryColor);
    }

    [Fact]
    public void ResetCommand_AfterWin_ClearsIsWon()
    {
        var board = new Board(rows: 1, columns: 1);
        var viewModel = new GameViewModel(board);
        viewModel.CellTappedCommand.Execute(viewModel.Cells[0]);
        Assert.True(viewModel.IsWon);

        viewModel.ResetCommand.Execute(null);

        Assert.False(viewModel.IsWon);
    }

    [Fact]
    public void ResetCommand_AfterMoves_ResetsMoveCount()
    {
        var board = new Board(rows: 2, columns: 2);
        var viewModel = new GameViewModel(board);
        viewModel.CellTappedCommand.Execute(viewModel.Cells[0]);
        Assert.Equal(1, viewModel.MoveCount);

        viewModel.ResetCommand.Execute(null);

        Assert.Equal(0, viewModel.MoveCount);
    }

    // ---- New game ----

    [Fact]
    public void NewGameCommand_ResetsMoveCount()
    {
        var board = new Board(rows: 2, columns: 2);
        var viewModel = new GameViewModel(board);
        viewModel.CellTappedCommand.Execute(viewModel.Cells[0]);
        Assert.Equal(1, viewModel.MoveCount);

        viewModel.NewGameCommand.Execute(null);

        Assert.Equal(0, viewModel.MoveCount);
    }

    [Fact]
    public void NewGameCommand_AfterWin_ClearsIsWon()
    {
        var board = new Board(rows: 1, columns: 1);
        var viewModel = new GameViewModel(board);
        viewModel.CellTappedCommand.Execute(viewModel.Cells[0]);
        Assert.True(viewModel.IsWon);

        viewModel.NewGameCommand.Execute(null);

        Assert.False(viewModel.IsWon);
    }

    // ---- MoveCount & win detection ----

    [Fact]
    public void MoveCount_ExposedFromBoard()
    {
        var board = new Board(rows: 1, columns: 1);
        var viewModel = new GameViewModel(board);
        var cell = viewModel.Cells[0];

        Assert.Equal(0, viewModel.MoveCount);

        viewModel.CellTappedCommand.Execute(cell);   // first move - wins on 1x1
        viewModel.CellTappedCommand.Execute(cell);   // ignored after win

        Assert.Equal(1, viewModel.MoveCount);
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

    [Fact]
    public void ShowMoveCount_DefaultsTrue()
    {
        var viewModel = new GameViewModel();

        Assert.True(viewModel.ShowMoveCount);
    }

    [Fact]
    public void SetShowMoveCount_UpdatesShowMoveCount()
    {
        var viewModel = new GameViewModel();

        viewModel.SetShowMoveCount(false);

        Assert.False(viewModel.ShowMoveCount);
    }

    // ---- Game timer ----

    [Fact]
    public void ShowGameTimer_DefaultsTrue()
    {
        var viewModel = new GameViewModel();

        Assert.True(viewModel.ShowGameTimer);
    }

    [Fact]
    public void SetShowGameTimer_UpdatesShowGameTimer()
    {
        var viewModel = new GameViewModel();

        viewModel.SetShowGameTimer(false);

        Assert.False(viewModel.ShowGameTimer);
    }

    [Fact]
    public void ElapsedDisplay_IsZeroAfterRefreshInitially()
    {
        var viewModel = new GameViewModel();
        viewModel.RefreshElapsedDisplay();

        Assert.Equal("0:00", viewModel.ElapsedDisplay);
    }

    [Fact]
    public void ResetCommand_ResetsElapsedDisplay()
    {
        var board = new Board(rows: 2, columns: 2);
        var viewModel = new GameViewModel(board);
        viewModel.CellTappedCommand.Execute(viewModel.Cells[0]);
        viewModel.ResetCommand.Execute(null);
        viewModel.RefreshElapsedDisplay();

        Assert.Equal("0:00", viewModel.ElapsedDisplay);
    }

    [Fact]
    public void NewGameCommand_ResetsElapsedDisplay()
    {
        var board = new Board(rows: 2, columns: 2);
        var viewModel = new GameViewModel(board);
        viewModel.CellTappedCommand.Execute(viewModel.Cells[0]);
        viewModel.NewGameCommand.Execute(null);
        viewModel.RefreshElapsedDisplay();

        Assert.Equal("0:00", viewModel.ElapsedDisplay);
    }

    [Fact]
    public void ElapsedDisplay_DoesNotChangeAfterWin_UntilNextRefreshAfterDelay()
    {
        var board = new Board(rows: 1, columns: 1);
        var viewModel = new GameViewModel(board);
        viewModel.CellTappedCommand.Execute(viewModel.Cells[0]);
        Assert.True(viewModel.IsWon);
        viewModel.RefreshElapsedDisplay();
        var first = viewModel.ElapsedDisplay;
        Thread.Sleep(60);
        viewModel.RefreshElapsedDisplay();

        Assert.Equal(first, viewModel.ElapsedDisplay);
    }

    [Fact]
    public void RefreshThemeDependentVisuals_NotifiesAllCells()
    {
        var board = new Board(rows: 2, columns: 2);
        var viewModel = new GameViewModel(board);
        var raised = 0;
        foreach (var cell in viewModel.Cells)
            cell.PropertyChanged += OnCellPropertyChanged;

        viewModel.RefreshThemeDependentVisuals();

        Assert.Equal(4, raised);

        void OnCellPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameCell.IsPrimaryColor))
                raised++;
        }
    }
}
