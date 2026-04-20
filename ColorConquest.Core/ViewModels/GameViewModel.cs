
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ColorConquest.Core.Models;
using GameCell = ColorConquest.Core.Models.Cell;

namespace ColorConquest.Core.ViewModels;

/// <summary>
/// ViewModel for the Game page. Holds the board and exposes a flat list of cells
/// for binding, plus a command for when a cell is tapped.
/// </summary>
public partial class GameViewModel : ObservableObject
{
    private Board _board;
    private bool _hasGameStarted;
    private bool _timerActive;
    private CancellationTokenSource? _timerCts;
    [ObservableProperty]
    private bool showMoveCount = true;

    [ObservableProperty]
    private bool showGameTimer = true;

    private readonly Stopwatch _gameStopwatch = new();

    [ObservableProperty]
    private string elapsedDisplay = "0:00";

    [ObservableProperty]
    private double tileDisplaySize = 48;

    public ObservableCollection<GameCell> Cells { get; private set; }
    public int RowCount => _board.RowCount;

    public void SetTileDisplaySize(double size) => TileDisplaySize = size;

    public void StartTimer()
    {
        if (_timerActive) return;
        _timerActive = true;
        _timerCts = new CancellationTokenSource();
        _ = RunTimerAsync(_timerCts.Token);
    }

    public void StopTimer()
    {
        _timerActive = false;
        _timerCts?.Cancel();
    }

    private async Task RunTimerAsync(CancellationToken token)
    {
        while (_timerActive && !token.IsCancellationRequested)
        {
            await Task.Delay(1000, token);
            RefreshElapsedDisplay();
        }
    }

    public void OnThemeChanged()
    {
        RefreshThemeDependentVisuals();
    }

    public void OnColorsChanged()
    {
        RefreshThemeDependentVisuals();
    }

    /// <summary>5×5 scrambled board (used by tests and as legacy default).</summary>
    public GameViewModel() : this(GameConstants.DefaultRowCount, GameConstants.DefaultColumnCount)
    {
    }

    /// <summary>New scrambled game with the given dimensions.</summary>
    public GameViewModel(int rows, int columns)
    {
        var random = new Random();
        var moves = BoardSizeExtensions.ScrambleMoveCount(rows, columns);
        _board = Board.CreateScrambled(rows, columns, moves, random);
        Cells = new ObservableCollection<GameCell>();

        ReloadCellsFromBoard();
    }

    /// <summary>Use an existing board (unit tests).</summary>
    public GameViewModel(Board board)
    {
        _board = board;
        Cells = new ObservableCollection<GameCell>();
        ReloadCellsFromBoard();
    }

    public int ColumnCount => _board.ColumnCount;
    public int MoveCount => _board.MoveCount;


    [ObservableProperty]
    private bool isWon;

    public string WinMessage => IsWon ? "You win!" : " ";

    partial void OnIsWonChanged(bool value)
    {
        OnPropertyChanged(nameof(WinMessage));
    }
    [RelayCommand]
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
            RefreshElapsedDisplay();
        }
    }

    [RelayCommand]
    private void OnReset()
    {
        _board.ResetToInitialState();
        _hasGameStarted = false;
        IsWon = false;
        _gameStopwatch.Reset();
        OnPropertyChanged(nameof(MoveCount));
        RefreshElapsedDisplay();
    }

    [RelayCommand]
    private void OnNewGame()
    {
        var random = new Random();
        var rows = _board.RowCount;
        var columns = _board.ColumnCount;
        var moves = BoardSizeExtensions.ScrambleMoveCount(rows, columns);

        _board = Board.CreateScrambled(rows, columns, moves, random);
        ReloadCellsFromBoard();
        _hasGameStarted = false;
        IsWon = false;
        _gameStopwatch.Reset();
        RefreshElapsedDisplay();
    }
    /// <summary>Replace the board when Settings board size changes (may change row/column count).</summary>
    public void RecreateBoardForDimensions(int rows, int columns)
    {
        if (rows == _board.RowCount && columns == _board.ColumnCount)
            return;

        var random = new Random();
        var moves = BoardSizeExtensions.ScrambleMoveCount(rows, columns);
        _board = Board.CreateScrambled(rows, columns, moves, random);
        ReloadCellsFromBoard();
        _hasGameStarted = false;
        IsWon = false;
        _gameStopwatch.Reset();
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

    public void SetShowMoveCount(bool show) => ShowMoveCount = show;
    public void SetShowGameTimer(bool show) => ShowGameTimer = show;

    public void RefreshElapsedDisplay()
    {
        var e = _gameStopwatch.Elapsed;
        ElapsedDisplay = $"{(int)e.TotalMinutes}:{e.Seconds:D2}";
    }
}
