using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using ColorConquest.Core.Models;
using GameCell = ColorConquest.Core.Models.Cell;

namespace ColorConquest.Core.ViewModels;

/// <summary>
/// ViewModel for the Game page. Holds the board and exposes a flat list of cells
/// for binding, plus a command for when a cell is tapped.
/// </summary>
public class GameViewModel : INotifyPropertyChanged
{
    private Board _board;
    private bool _hasGameStarted;
    private bool _showMoveCount = true;
    private bool _showGameTimer = true;
    private readonly Stopwatch _gameStopwatch = new();
    private string _elapsedDisplay = "0:00";

    public GameViewModel(Board? board = null)
    {
        if (board is null)
        {
            var random = new Random();
            const int scrambleMoves = 15;
            _board = Board.CreateScrambled(GameConstants.DefaultRowCount, GameConstants.DefaultColumnCount, scrambleMoves, random);
        }
        else
        {
            _board = board;
        }
        Cells = new ObservableCollection<GameCell>();
        ReloadCellsFromBoard();
        CellTappedCommand = new Command<GameCell>(OnCellTapped);
        ResetCommand = new Command(OnReset);
        NewGameCommand = new Command(OnNewGame);
    }

    public ObservableCollection<GameCell> Cells { get; }
    public int RowCount => _board.RowCount;
    public int ColumnCount => _board.ColumnCount;
    public int MoveCount => _board.MoveCount;
    public bool ShowMoveCount
    {
        get => _showMoveCount;
        private set
        {
            if (_showMoveCount == value) return;
            _showMoveCount = value;
            OnPropertyChanged(nameof(ShowMoveCount));
        }
    }

    public bool ShowGameTimer
    {
        get => _showGameTimer;
        private set
        {
            if (_showGameTimer == value) return;
            _showGameTimer = value;
            OnPropertyChanged(nameof(ShowGameTimer));
        }
    }

    public string ElapsedDisplay
    {
        get => _elapsedDisplay;
        private set
        {
            if (_elapsedDisplay == value) return;
            _elapsedDisplay = value;
            OnPropertyChanged(nameof(ElapsedDisplay));
        }
    }

    public bool IsWon { get; private set; }
    public ICommand CellTappedCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand NewGameCommand { get; }
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnCellTapped(GameCell? cell)
    {
        if (cell is null) return;
        if (IsWon) return;
        _board.ToggleCellAndAdjacent(cell.Row, cell.Column);
        OnPropertyChanged(nameof(MoveCount));

        if (!_hasGameStarted)
        {
            _hasGameStarted = true;
            _gameStopwatch.Start();
        }

        if (_hasGameStarted && _board.AreAllCellsSameColor())
        {
            IsWon = true;
            _gameStopwatch.Stop();
            OnPropertyChanged(nameof(IsWon));
            RefreshElapsedDisplay();
        }
    }

    private void OnReset()
    {
        _board.ResetToInitialState();
        _hasGameStarted = false;
        IsWon = false;
        _gameStopwatch.Reset();
        OnPropertyChanged(nameof(MoveCount));
        OnPropertyChanged(nameof(IsWon));
        RefreshElapsedDisplay();
    }

    private void OnNewGame()
    {
        var random = new Random();
        var rows = _board.RowCount;
        var columns = _board.ColumnCount;
        const int scrambleMoves = 15;

        _board = Board.CreateScrambled(rows, columns, scrambleMoves, random);
        ReloadCellsFromBoard();
        _hasGameStarted = false;
        IsWon = false;
        _gameStopwatch.Reset();
        OnPropertyChanged(nameof(IsWon));
        RefreshElapsedDisplay();
    }

    private void ReloadCellsFromBoard()
    {
        Cells.Clear();
        for (var row = 0; row < _board.RowCount; row++)
        for (var column = 0; column < _board.ColumnCount; column++)
            Cells.Add(_board.GetCell(row, column));
        OnPropertyChanged(nameof(RowCount));
        OnPropertyChanged(nameof(ColumnCount));
        OnPropertyChanged(nameof(MoveCount));
    }

    public void RefreshThemeDependentVisuals()
    {
        foreach (var cell in Cells)
            cell.NotifyThemeChanged();
    }

    public void SetShowMoveCount(bool show)
    {
        ShowMoveCount = show;
    }

    public void SetShowGameTimer(bool show)
    {
        ShowGameTimer = show;
    }

    public void RefreshElapsedDisplay()
    {
        var e = _gameStopwatch.Elapsed;
        ElapsedDisplay = $"{(int)e.TotalMinutes}:{e.Seconds:D2}";
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
