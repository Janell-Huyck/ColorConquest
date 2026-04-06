using System.ComponentModel;

namespace ColorConquest.Core.Models;

/// <summary>
/// A single tile in the puzzle grid. Holds position (row, column), current color state,
/// and the initial color (for reset). Implements INotifyPropertyChanged for UI binding.
/// </summary>
public class Cell : INotifyPropertyChanged
{
    private bool _isPrimaryColor;

    public Cell(int row, int column, bool initialIsPrimaryColor = true)
    {
        Row = row;
        Column = column;
        InitialIsPrimaryColor = initialIsPrimaryColor;
        _isPrimaryColor = initialIsPrimaryColor;
    }

    public int Row { get; }
    public int Column { get; }

    /// <summary>Color this cell had when it was created (used for reset).</summary>
    public bool InitialIsPrimaryColor { get; private set; }

    public bool IsPrimaryColor
    {
        get => _isPrimaryColor;
        set
        {
            if (_isPrimaryColor == value) return;
            _isPrimaryColor = value;
            OnPropertyChanged(nameof(IsPrimaryColor));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

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

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
