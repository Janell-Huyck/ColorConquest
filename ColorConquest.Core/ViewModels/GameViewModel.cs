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
    private double _tileDisplaySize = 48;

    /// <summary>Width and height of each board cell in device-independent units; view sets this so large boards fit on small screens.</summary>
    public double TileDisplaySize
    {
        get => _tileDisplaySize;
        private set
        {
            if (Math.Abs(_tileDisplaySize - value) < 0.25)
                return;
            _tileDisplaySize = value;
            OnPropertyChanged(nameof(TileDisplaySize));
        }
    }

    public void SetTileDisplaySize(double size) => TileDisplaySize = size;

    /// <summary>5×5 scrambled board (used by tests and as legacy default).</summary>
    public GameViewModel() : this(GameConstants.DefaultRowCount, GameConstants.DefaultColumnCount)
    {
    }

    /// <summary>New scrambled game with the given dimensions.</summary>
    public GameViewModel(int rows, int columns)
    {
        var random = new Random();
        var moves = BoardDifficultySizes.ScrambleMoveCount(rows, columns);
        _board = Board.CreateScrambled(rows, columns, moves, random);
        Cells = new ObservableCollection<GameCell>();
        CellTappedCommand = new Command<GameCell>(OnCellTapped);
        ResetCommand = new Command(OnReset);
        NewGameCommand = new Command(OnNewGame);
        ReloadCellsFromBoard();
    }

    /// <summary>Use an existing board (unit tests).</summary>
    public GameViewModel(Board board)
    {
        _board = board;
        Cells = new ObservableCollection<GameCell>();
        CellTappedCommand = new Command<GameCell>(OnCellTapped);
        ResetCommand = new Command(OnReset);
        NewGameCommand = new Command(OnNewGame);
        ReloadCellsFromBoard();
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

    /// <summary>Replace the board when Settings difficulty changes (may change row/column count).</summary>
    public void RecreateBoardForDimensions(int rows, int columns)
    {
        if (rows == _board.RowCount && columns == _board.ColumnCount)
            return;

        var random = new Random();
        var moves = BoardDifficultySizes.ScrambleMoveCount(rows, columns);
        _board = Board.CreateScrambled(rows, columns, moves, random);
        ReloadCellsFromBoard();
        _hasGameStarted = false;
        IsWon = false;
        _gameStopwatch.Reset();
        OnPropertyChanged(nameof(IsWon));
        RefreshElapsedDisplay();
    }

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
        var moves = BoardDifficultySizes.ScrambleMoveCount(rows, columns);

        _board = Board.CreateScrambled(rows, columns, moves, random);
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
