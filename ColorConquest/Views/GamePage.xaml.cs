using ColorConquest.Core.ViewModels;
using ColorConquest.Services;

namespace ColorConquest.Views;

public partial class GamePage : ContentPage
{
    private readonly GameViewModel _viewModel;
    private bool _elapsedTickerActive;

    public GamePage()
    {
        InitializeComponent();
        _viewModel = new GameViewModel();
        BindingContext = _viewModel;
        // Grid layout: same number of columns as the board, with gaps between tiles
        const int cellSize = 48;
        const int spacing = 4;
        var gridLayout = new GridItemsLayout(_viewModel.ColumnCount, ItemsLayoutOrientation.Vertical)
        {
            VerticalItemSpacing = spacing,
            HorizontalItemSpacing = spacing
        };
        GameGrid.ItemsLayout = gridLayout;
        // Fixed grid size so cells stay 48×48 and don't shrink/expand with the window
        GameGrid.WidthRequest = _viewModel.ColumnCount * cellSize + (_viewModel.ColumnCount - 1) * spacing;
        GameGrid.HeightRequest = _viewModel.RowCount * cellSize + (_viewModel.RowCount - 1) * spacing;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.SetShowMoveCount(GameDisplayPreferences.GetShowMoveCount());
        _viewModel.SetShowGameTimer(GameDisplayPreferences.GetShowGameTimer());
        _viewModel.RefreshElapsedDisplay();
        _viewModel.RefreshThemeDependentVisuals();
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
        TileColorPreferences.ColorsChanged += OnColorsChanged;

        _elapsedTickerActive = true;
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), ElapsedTickerTick);
    }

    protected override void OnDisappearing()
    {
        _elapsedTickerActive = false;
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged -= OnRequestedThemeChanged;
        TileColorPreferences.ColorsChanged -= OnColorsChanged;
        base.OnDisappearing();
    }

    private bool ElapsedTickerTick()
    {
        if (!_elapsedTickerActive)
            return false;

        _viewModel.RefreshElapsedDisplay();
        return true;
    }

    private void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        _viewModel.RefreshThemeDependentVisuals();
    }

    private void OnColorsChanged(object? sender, EventArgs e)
    {
        _viewModel.RefreshThemeDependentVisuals();
    }
}
