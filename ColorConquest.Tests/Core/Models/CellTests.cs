using System.ComponentModel;
using ColorConquest.Core.Models;
using Xunit;
using GameCell = ColorConquest.Core.Models.Cell;

namespace ColorConquest.Tests.Core.Models;

public class CellTests
{
    // ---- Construction ----

    [Fact]
    public void Constructor_SetsRowAndColumn()
    {
        var cell = new GameCell(row: 2, column: 3);

        Assert.Equal(2, cell.Row);
        Assert.Equal(3, cell.Column);
    }

    [Fact]
    public void Constructor_WithNoInitialState_IsPrimaryColorIsTrue()
    {
        var cell = new GameCell(0, 0);

        Assert.True(cell.IsPrimaryColor);
    }

    [Fact]
    public void Constructor_WithInitialState_IsPrimaryColorMatches()
    {
        var cellPrimary = new GameCell(0, 0, initialIsPrimaryColor: true);
        var cellSecondary = new GameCell(0, 0, initialIsPrimaryColor: false);

        Assert.True(cellPrimary.IsPrimaryColor);
        Assert.False(cellSecondary.IsPrimaryColor);
    }

    // ---- Row and Column (read-only) ----

    [Fact]
    public void Row_ReturnsValuePassedToConstructor()
    {
        var cell = new GameCell(1, 0);

        Assert.Equal(1, cell.Row);
    }

    [Fact]
    public void Column_ReturnsValuePassedToConstructor()
    {
        var cell = new GameCell(0, 2);

        Assert.Equal(2, cell.Column);
    }

    // ---- Two-color state (primary vs secondary) ----

    [Fact]
    public void IsPrimaryColor_WhenSet_UpdatesValue()
    {
        var cell = new GameCell(0, 0, initialIsPrimaryColor: true);

        cell.IsPrimaryColor = false;

        Assert.False(cell.IsPrimaryColor);
    }

    [Fact]
    public void IsPrimaryColor_WhenSet_RaisesPropertyChanged()
    {
        var cell = new GameCell(0, 0);
        PropertyChangedEventArgs? captured = null;
        cell.PropertyChanged += (_, e) => captured = e;

        cell.IsPrimaryColor = false;

        Assert.NotNull(captured);
        Assert.Equal(nameof(GameCell.IsPrimaryColor), captured!.PropertyName);
    }

    [Fact]
    public void IsPrimaryColor_WhenSetToSameValue_DoesNotRaisePropertyChanged()
    {
        var cell = new GameCell(0, 0, initialIsPrimaryColor: true);
        var raised = false;
        cell.PropertyChanged += (_, _) => raised = true;

        cell.IsPrimaryColor = true;

        Assert.False(raised);
    }

    // ---- Toggle ----

    [Fact]
    public void Toggle_WhenIsPrimaryColorIsTrue_SetsToFalse()
    {
        var cell = new GameCell(0, 0, initialIsPrimaryColor: true);

        cell.Toggle();

        Assert.False(cell.IsPrimaryColor);
    }

    [Fact]
    public void Toggle_WhenIsPrimaryColorIsFalse_SetsToTrue()
    {
        var cell = new GameCell(0, 0, initialIsPrimaryColor: false);

        cell.Toggle();

        Assert.True(cell.IsPrimaryColor);
    }

    [Fact]
    public void Toggle_WhenCalled_RaisesPropertyChanged()
    {
        var cell = new GameCell(0, 0);
        PropertyChangedEventArgs? captured = null;
        cell.PropertyChanged += (_, e) => captured = e;

        cell.Toggle();

        Assert.NotNull(captured);
        Assert.Equal(nameof(GameCell.IsPrimaryColor), captured!.PropertyName);
    }

    [Fact]
    public void Toggle_CalledTwice_ReturnsToOriginalValue()
    {
        var cell = new GameCell(0, 0, initialIsPrimaryColor: true);

        cell.Toggle();
        cell.Toggle();

        Assert.True(cell.IsPrimaryColor);
    }

    // ---- Initial state & ResetToInitial ----

    [Fact]
    public void InitialIsPrimaryColor_MatchesConstructorArgument()
    {
        var cellPrimary = new GameCell(0, 0, initialIsPrimaryColor: true);
        var cellSecondary = new GameCell(0, 0, initialIsPrimaryColor: false);

        Assert.True(cellPrimary.InitialIsPrimaryColor);
        Assert.False(cellSecondary.InitialIsPrimaryColor);
    }

    [Fact]
    public void ResetToInitial_WhenCellWasToggled_RestoresInitialColor()
    {
        var cell = new GameCell(0, 0, initialIsPrimaryColor: true);
        cell.Toggle();
        Assert.False(cell.IsPrimaryColor);

        cell.ResetToInitial();

        Assert.True(cell.IsPrimaryColor);
    }

    [Fact]
    public void ResetToInitial_WhenInitialWasSecondary_RestoresToSecondary()
    {
        var cell = new GameCell(0, 0, initialIsPrimaryColor: false);
        cell.Toggle();
        Assert.True(cell.IsPrimaryColor);

        cell.ResetToInitial();

        Assert.False(cell.IsPrimaryColor);
    }
}
