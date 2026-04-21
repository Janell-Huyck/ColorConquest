using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ColorConquest.Core.Models;

namespace ColorConquest.Core.ViewModels;

public partial class HomeAnimationViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ColorConquest.Core.Models.Cell> animatedCells = new();

    [ObservableProperty]
    private int boardSize = 3;

    [ObservableProperty]
    private bool isAnimating;

    [RelayCommand]
    public async Task PlayDemoAsync()
    {
        if (IsAnimating) return;
        IsAnimating = true;
        var random = new System.Random();
        // Create a 3x3 checkerboard pattern (unwinnable)
        var board = new Board(BoardSize, BoardSize);
        for (int row = 0; row < BoardSize; row++)
            for (int col = 0; col < BoardSize; col++)
                board.GetCell(row, col).IsPrimaryColor = ((row + col) % 2 == 0);
        // Copy board cells to AnimatedCells for display
        AnimatedCells.Clear();
        for (int row = 0; row < BoardSize; row++)
            for (int col = 0; col < BoardSize; col++)
                AnimatedCells.Add(board.GetCell(row, col));

        await Task.Delay(500);

        // Animate random moves indefinitely
        for (int move = 0; move < 1000; move++)
        {
            int row = random.Next(BoardSize);
            int col = random.Next(BoardSize);
            board.ToggleCellAndAdjacent(row, col);
            // Update AnimatedCells to reflect the board state
            for (int r = 0; r < BoardSize; r++)
                for (int c = 0; c < BoardSize; c++)
                {
                    var cell = AnimatedCells[r * BoardSize + c];
                    var boardCell = board.GetCell(r, c);
                    if (cell.IsPrimaryColor != boardCell.IsPrimaryColor)
                        cell.IsPrimaryColor = boardCell.IsPrimaryColor;
                }
            await Task.Delay(350);
        }
        await Task.Delay(900);
        IsAnimating = false;
    }
}
