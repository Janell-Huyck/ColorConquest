using ColorConquest.Core.ViewModels;

namespace ColorConquest.Views;

public partial class GamePage : ContentPage
{
    public GamePage()
    {
        InitializeComponent();
        var viewModel = new GameViewModel();
        BindingContext = viewModel;
        // Grid layout: same number of columns as the board, with gaps between tiles
        const int cellSize = 48;
        const int spacing = 4;
        var gridLayout = new GridItemsLayout(viewModel.ColumnCount, ItemsLayoutOrientation.Vertical)
        {
            VerticalItemSpacing = spacing,
            HorizontalItemSpacing = spacing
        };
        GameGrid.ItemsLayout = gridLayout;
        // Fixed grid size so cells stay 48×48 and don't shrink/expand with the window
        GameGrid.WidthRequest = viewModel.ColumnCount * cellSize + (viewModel.ColumnCount - 1) * spacing;
        GameGrid.HeightRequest = viewModel.RowCount * cellSize + (viewModel.RowCount - 1) * spacing;
    }
}
