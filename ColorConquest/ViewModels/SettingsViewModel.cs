using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ColorConquest.Core;
using ColorConquest.Services;
using System.Collections.ObjectModel;

namespace ColorConquest.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
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
        isDarkTheme = ThemePreferences.GetSavedTheme() == AppTheme.Dark;
        showMoveCount = GameDisplayPreferences.GetShowMoveCount();
        showGameTimer = GameDisplayPreferences.GetShowGameTimer();
        selectedBoardSize = GameBoardPreferences.GetBoardSize();
        availableColors = new ObservableCollection<TileColorOption>(TileColorPreferences.GetAvailableColors());
        selectedPrimaryColor = TileColorPreferences.GetPrimaryColor();
        selectedSecondaryColor = TileColorPreferences.GetSecondaryColor();
    }

    partial void OnIsDarkThemeChanged(bool value)
    {
        ThemePreferences.SaveTheme(value ? AppTheme.Dark : AppTheme.Light);
        if (Application.Current is not null)
            Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
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
