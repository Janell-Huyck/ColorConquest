using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ColorConquest.Core;
using ColorConquest.Core.Services;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;

namespace ColorConquest.Core.ViewModels;


public partial class SettingsViewModel : ObservableObject
{
	[RelayCommand]
	public void SelectPaletteColor(TileColorOption option)
	{
		if (ColorPaletteTarget == "Secondary")
			SetSecondaryColor(option);
		else
			SetPrimaryColor(option);
	}
	private readonly ThemePreferences _themePreferences;
	private readonly GameBoardPreferences _gameBoardPreferences;
	private readonly GameDisplayPreferences _gameDisplayPreferences;
	private readonly TileColorPreferences _tileColorPreferences;

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

	// New: Expose board size options for CollectionView binding
	public IEnumerable<BoardSizeOption> BoardSizeOptions { get; }

	// New: Expose SetBoardSizeCommand for CollectionView item tap
	public ICommand SetBoardSizeCommand { get; }

	// New: Expose ResetToDefaults as a command for the button
	public IRelayCommand ResetToDefaultsCommand { get; }

	// Modal state for color palette
	[ObservableProperty]
	private bool isColorPaletteVisible;

	// Which color is being edited ("Primary" or "Secondary")
	[ObservableProperty]
	private string? colorPaletteTarget;

	// Command to open the color palette modal
	[RelayCommand]
	public void ShowColorPalette(string target)
	{
		ColorPaletteTarget = target;
		IsColorPaletteVisible = true;
	}

	// Command to close the color palette modal
	[RelayCommand]
	public void HideColorPalette()
	{
		IsColorPaletteVisible = false;
		ColorPaletteTarget = null;
	}
	public SettingsViewModel(
		ThemePreferences themePreferences,
		GameBoardPreferences gameBoardPreferences,
		GameDisplayPreferences gameDisplayPreferences,
		TileColorPreferences tileColorPreferences)
	{
		_themePreferences = themePreferences;
		_gameBoardPreferences = gameBoardPreferences;
		_gameDisplayPreferences = gameDisplayPreferences;
		_tileColorPreferences = tileColorPreferences;

		// Load initial values from preferences/services
		isDarkTheme = _themePreferences.GetSavedTheme() == UserTheme.Dark;
		showMoveCount = _gameDisplayPreferences.GetShowMoveCount();
		showGameTimer = _gameDisplayPreferences.GetShowGameTimer();
		selectedBoardSize = _gameBoardPreferences.GetBoardSize();
		availableColors = new ObservableCollection<TileColorOption>(_tileColorPreferences.GetAvailableColors());
		selectedPrimaryColor = _tileColorPreferences.GetPrimaryColor();
		selectedSecondaryColor = _tileColorPreferences.GetSecondaryColor();

		isColorPaletteVisible = false;
		colorPaletteTarget = null;

		// Provide board size options for the UI
		BoardSizeOptions = new[]
		{
			new BoardSizeOption(BoardSize.Easy, "Easy 3×3"),
			new BoardSizeOption(BoardSize.Medium, "Medium 5×5"),
			new BoardSizeOption(BoardSize.Hard, "Hard 9×9")
		};

		SetBoardSizeCommand = new RelayCommand<BoardSize>(SetBoardSize);
		ResetToDefaultsCommand = new RelayCommand(ResetToDefaults);

		// Ensure IsSelected is set for the initial board size
		UpdateBoardSizeSelections();
	}

	// Helper class for board size options
	public partial class BoardSizeOption : ObservableObject
	{
		public BoardSize Size { get; }
		public string Label { get; }
		[ObservableProperty]
		private bool isSelected;

		public BoardSizeOption(BoardSize size, string label)
		{
			Size = size;
			Label = label;
		}
	}

	private void SetBoardSize(BoardSize size)
	{
		SelectedBoardSize = size;
		UpdateBoardSizeSelections();
	}

	// Now called by ResetToDefaultsCommand
	public void ResetToDefaults()
	{
		_themePreferences.Reset();
		_gameBoardPreferences.Reset();
		_gameDisplayPreferences.Reset();
		_tileColorPreferences.Reset();
		// Reload all properties from preferences/services
		IsDarkTheme = _themePreferences.GetSavedTheme() == UserTheme.Dark;
		ShowMoveCount = _gameDisplayPreferences.GetShowMoveCount();
		ShowGameTimer = _gameDisplayPreferences.GetShowGameTimer();
		SelectedBoardSize = _gameBoardPreferences.GetBoardSize();
		AvailableColors = new ObservableCollection<TileColorOption>(_tileColorPreferences.GetAvailableColors());
		SelectedPrimaryColor = _tileColorPreferences.GetPrimaryColor();
		SelectedSecondaryColor = _tileColorPreferences.GetSecondaryColor();
	}

	private void UpdateBoardSizeSelections()
    {
        foreach (var option in BoardSizeOptions)
        {
            if (option is BoardSizeOption bso)
                bso.IsSelected = (bso.Size == SelectedBoardSize);
        }
    }

	partial void OnIsDarkThemeChanged(bool value)
	{
		_themePreferences.SaveTheme(value ? UserTheme.Dark : UserTheme.Light);
		// UI layer is responsible for updating Application.Current.UserAppTheme
	}

	partial void OnShowMoveCountChanged(bool value)
	{
		_gameDisplayPreferences.SetShowMoveCount(value);
	}

	partial void OnShowGameTimerChanged(bool value)
	{
		_gameDisplayPreferences.SetShowGameTimer(value);
	}

	partial void OnSelectedBoardSizeChanged(BoardSize value)
	{
		_gameBoardPreferences.SetBoardSize(value);
	}

	[RelayCommand]
	public void SetPrimaryColor(TileColorOption option)
	{
		_tileColorPreferences.SetPrimaryColorKey(option.Key);
		SelectedPrimaryColor = option;
		IsColorPaletteVisible = false;
		ColorPaletteTarget = null;
	}

	[RelayCommand]
	public void SetSecondaryColor(TileColorOption option)
	{
		_tileColorPreferences.SetSecondaryColorKey(option.Key);
		SelectedSecondaryColor = option;
		IsColorPaletteVisible = false;
		ColorPaletteTarget = null;
	}
}
