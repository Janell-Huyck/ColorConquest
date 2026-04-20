using System.ComponentModel;
using ColorConquest.Core.ViewModels;
using ColorConquest.Services;

namespace ColorConquest.Views;

public partial class GamePage : ContentPage
{
    private const double TileSpacing = 4;
    private const double MinTileSize = 26;
    private const double MaxTileSize = 48;

    private readonly GameViewModel _viewModel;
    private bool _elapsedTickerActive;
    private int _appliedRows;
    private int _appliedColumns;

    public GamePage()
    {
        InitializeComponent();
        var (rows, columns) = GameBoardPreferences.GetBoardDimensions();
        _viewModel = new GameViewModel(rows, columns);
        _appliedRows = rows;
        _appliedColumns = columns;
        BindingContext = _viewModel;
        BoardScrollArea.SizeChanged += OnBoardScrollAreaSizeChanged;
        PageLayoutGrid.SizeChanged += OnPageLayoutGridSizeChanged;
        ApplyGameGridLayout();
    }

    private void OnBoardScrollAreaSizeChanged(object? sender, EventArgs e) =>
        UpdateBoardLayoutMetrics();

    private void OnPageLayoutGridSizeChanged(object? sender, EventArgs e) =>
        Dispatcher.Dispatch(UpdateBoardLayoutMetrics);

    private void ApplyGameGridLayout()
    {
        GameGrid.ItemsLayout = new GridItemsLayout(_viewModel.ColumnCount, ItemsLayoutOrientation.Vertical)
        {
            VerticalItemSpacing = TileSpacing,
            HorizontalItemSpacing = TileSpacing
        };
        UpdateBoardLayoutMetrics();
    }

    /// <summary>
    /// Fit the whole grid inside the board ScrollView when possible (standard phones + 9×9).
    /// Falls back to scroll if the viewport is too small even at MinTileSize.
    /// </summary>
    private void UpdateBoardLayoutMetrics()
    {
        var w = BoardScrollArea.Width;
        var h = BoardScrollArea.Height;
        if (w <= 0 || h <= 0)
            return;

        var cols = _viewModel.ColumnCount;
        var rows = _viewModel.RowCount;
        if (cols <= 0 || rows <= 0)
            return;

        var cellFromW = (w - (cols - 1) * TileSpacing) / cols;
        var cellFromH = (h - (rows - 1) * TileSpacing) / rows;
        var cell = Math.Min(cellFromW, cellFromH);
        cell = Math.Clamp(cell, MinTileSize, MaxTileSize);

        _viewModel.SetTileDisplaySize(cell);

        var gridW = cols * cell + (cols - 1) * TileSpacing;
        var gridH = rows * cell + (rows - 1) * TileSpacing;
        GameGrid.WidthRequest = gridW;
        GameGrid.HeightRequest = gridH;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.PropertyChanged -= OnGameViewModelPropertyChanged;
        _viewModel.PropertyChanged += OnGameViewModelPropertyChanged;

        var (rows, columns) = GameBoardPreferences.GetBoardDimensions();
        if (rows != _appliedRows || columns != _appliedColumns)
        {
            _viewModel.RecreateBoardForDimensions(rows, columns);
            _appliedRows = rows;
            _appliedColumns = columns;
            ApplyGameGridLayout();
        }

        GameSessionSnapshot.ReportMoveCount(_viewModel.MoveCount);

        _viewModel.SetShowMoveCount(GameDisplayPreferences.GetShowMoveCount());
        _viewModel.SetShowGameTimer(GameDisplayPreferences.GetShowGameTimer());
        _viewModel.RefreshElapsedDisplay();
        _viewModel.OnThemeChanged();
        _viewModel.OnColorsChanged();
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
        TileColorPreferences.ColorsChanged += OnColorsChanged;

        _viewModel.StartTimer();

        Dispatcher.Dispatch(UpdateBoardLayoutMetrics);
    }

    protected override void OnDisappearing()
    {
        _viewModel.PropertyChanged -= OnGameViewModelPropertyChanged;
        _viewModel.StopTimer();
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged -= OnRequestedThemeChanged;
        TileColorPreferences.ColorsChanged -= OnColorsChanged;
        base.OnDisappearing();
    }

    private void OnGameViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GameViewModel.MoveCount))
            GameSessionSnapshot.ReportMoveCount(_viewModel.MoveCount);

        if (e.PropertyName is nameof(GameViewModel.RowCount) or nameof(GameViewModel.ColumnCount))
            Dispatcher.Dispatch(UpdateBoardLayoutMetrics);
    }

    private void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        _viewModel.OnThemeChanged();
    }

    private void OnColorsChanged(object? sender, EventArgs e)
    {
        _viewModel.OnColorsChanged();
    }
}
