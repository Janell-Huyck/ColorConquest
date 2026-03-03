using System.Collections.ObjectModel;
using System.Windows.Input;
using ColorConquest.Core.Models;
using GameCell = ColorConquest.Core.Models.Cell;

namespace ColorConquest.Core.ViewModels;

/// <summary>
/// ViewModel for the Game page. Holds the board and exposes a flat list of cells
/// for binding, plus a command for when a cell is tapped.
/// </summary>
public class GameViewModel
{
    private readonly Board _board;
    private bool _hasGameStarted;

    public GameViewModel(Board? board = null)
    {
        _board = board ?? new Board();
        Cells = new ObservableCollection<GameCell>();
        for (var row = 0; row < _board.RowCount; row++)
        for (var column = 0; column < _board.ColumnCount; column++)
            Cells.Add(_board.GetCell(row, column));
        CellTappedCommand = new Command<GameCell>(OnCellTapped);
        ResetCommand = new Command(OnReset);
    }

    public ObservableCollection<GameCell> Cells { get; }
    public int RowCount => _board.RowCount;
    public int ColumnCount => _board.ColumnCount;
    public int MoveCount => _board.MoveCount;
    // TODO: surface win state to the UI (e.g., show a message or navigate) when IsWon becomes true.
    public bool IsWon { get; private set; }
    public ICommand CellTappedCommand { get; }
    public ICommand ResetCommand { get; }

    private void OnCellTapped(GameCell? cell)
    {
        if (cell is null) return;
        _board.ToggleCellAndAdjacent(cell.Row, cell.Column);

        if (!_hasGameStarted)
            _hasGameStarted = true;

        if (_hasGameStarted && _board.AreAllCellsSameColor())
            IsWon = true;
    }

    private void OnReset()
    {
        _board.ResetToInitialState();
        _hasGameStarted = false;
        IsWon = false;
    }
}
