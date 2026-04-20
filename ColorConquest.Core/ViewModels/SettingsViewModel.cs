using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ColorConquest.Core;
using ColorConquest.Core.Services;
using System.Collections.ObjectModel;

namespace ColorConquest.Core.ViewModels;

public class BoardSizeOption
{
    public BoardSize Size { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public partial class SettingsViewModel : ObservableObject
{
    public IReadOnlyList<BoardSizeOption> BoardSizeOptions { get; } = new List<BoardSizeOption>
    {
        new BoardSizeOption { Size = BoardSize.Easy, Label = "Easy (3×3)", Description = "" },
        new BoardSizeOption { Size = BoardSize.Medium, Label = "Medium (5×5)", Description = "" },
        new BoardSizeOption { Size = BoardSize.Hard, Label = "Hard (9×9)", Description = "" }
    };
    [ObservableProperty]
    private bool isDarkTheme;

    [ObservableProperty]
    private bool showMoveCount;

    [ObservableProperty]
    private bool showGameTimer;

    [ObservableProperty]
    private BoardSize selectedBoardSize;

    [ObservableProperty]
    private ObservableCollection<TileColorOption> availableColors = new();

    [ObservableProperty]
    private TileColorOption? selectedPrimaryColor;

    [ObservableProperty]
    private TileColorOption? selectedSecondaryColor;

    public SettingsViewModel()
    {
        // Load initial values from preferences/services
        isDarkTheme = ThemePreferences.GetSavedTheme() == UserTheme.Dark;
        showMoveCount = GameDisplayPreferences.GetShowMoveCount();
        showGameTimer = GameDisplayPreferences.GetShowGameTimer();
        selectedBoardSize = GameBoardPreferences.GetBoardSize();
        availableColors = new ObservableCollection<TileColorOption>(TileColorPreferences.GetAvailableColors());
        selectedPrimaryColor = TileColorPreferences.GetPrimaryColor();
        selectedSecondaryColor = TileColorPreferences.GetSecondaryColor();
    }

    partial void OnIsDarkThemeChanged(bool value)
    {
        ThemePreferences.SaveTheme(value ? UserTheme.Dark : UserTheme.Light);
        // Platform theme update should be handled in the app layer, not in Core
    }

    partial void OnShowMoveCountChanged(bool value)
    {
        GameDisplayPreferences.SetShowMoveCount(value);
    }

    partial void OnShowGameTimerChanged(bool value)
    {
        GameDisplayPreferences.SetShowGameTimer(value);
    }

    partial void OnSelectedBoardSizeChanged(BoardSize value)
    {
        GameBoardPreferences.SetBoardSize(value);
    }

    [RelayCommand]
    private void SetPrimaryColor(TileColorOption option)
    {
        TileColorPreferences.SetPrimaryColorKey(option.Key);
        SelectedPrimaryColor = option;
    }

    [RelayCommand]
    private void SetSecondaryColor(TileColorOption option)
    {
        TileColorPreferences.SetSecondaryColorKey(option.Key);
        SelectedSecondaryColor = option;
    }
}
