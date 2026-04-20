using CommunityToolkit.Mvvm.ComponentModel;

namespace ColorConquest.Core.Models;

/// <summary>
/// A single tile in the puzzle grid. Holds position (row, column), current color state,
/// and the initial color (for reset). Implements ObservableObject for UI binding.
/// </summary>
public partial class Cell : ObservableObject
{
    public Cell(int row, int column, bool initialIsPrimaryColor = true)
    {
        Row = row;
        Column = column;
        InitialIsPrimaryColor = initialIsPrimaryColor;
        isPrimaryColor = initialIsPrimaryColor;
    }

    public int Row { get; }
    public int Column { get; }

    /// <summary>Color this cell had when it was created (used for reset).</summary>
    public bool InitialIsPrimaryColor { get; private set; }

    [ObservableProperty]
    private bool isPrimaryColor;

    public void Toggle()
    {
        IsPrimaryColor = !IsPrimaryColor;
    }

    /// <summary>Restores this cell to its initial color.</summary>
    public void ResetToInitial()
    {
        IsPrimaryColor = InitialIsPrimaryColor;
    }

    /// <summary>Updates the initial color to match the current color.</summary>
    public void CaptureCurrentAsInitial()
    {
        InitialIsPrimaryColor = IsPrimaryColor;
    }

    /// <summary>
    /// Raises a property-changed event for IsPrimaryColor without changing state.
    /// Useful when visual color mapping changes due to app theme changes.
    /// </summary>
    public void NotifyThemeChanged()
    {
        OnPropertyChanged(nameof(IsPrimaryColor));
    }
}
