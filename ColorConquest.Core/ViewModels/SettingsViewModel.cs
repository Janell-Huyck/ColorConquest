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
	private readonly ThemePreferences _themePreferences;
	private readonly ThemeViewModel _themeViewModel;
	private readonly GameBoardPreferences _gameBoardPreferences;
	private readonly GameDisplayPreferences _gameDisplayPreferences;
	private readonly TileColorPreferences _tileColorPreferences;


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

	[ObservableProperty]
	private bool isColorPaletteVisible;

	[ObservableProperty]
	private string? colorPaletteTarget;

	private IEnumerable<BoardSizeOption> _boardSizeOptions = Array.Empty<BoardSizeOption>();
	public IEnumerable<BoardSizeOption> BoardSizeOptions
	{
		get => _boardSizeOptions;
		private set
		{
			_boardSizeOptions = value;
			OnPropertyChanged();
		}
	}



	// RelayCommand to reset both theme and all other settings
	[RelayCommand]
	public void ResetAllToDefaults()
	{
		_themeViewModel.ResetToDefault();
		ResetToDefaults();
	}

	public SettingsViewModel(
		ThemePreferences themePreferences,
		GameBoardPreferences gameBoardPreferences,
		GameDisplayPreferences gameDisplayPreferences,
		TileColorPreferences tileColorPreferences,
		ThemeViewModel themeViewModel)
	{
		_themePreferences = themePreferences;
		_gameBoardPreferences = gameBoardPreferences;
		_gameDisplayPreferences = gameDisplayPreferences;
		_tileColorPreferences = tileColorPreferences;
		_themeViewModel = themeViewModel;

		// Theme is now managed by ThemeViewModel
		showMoveCount = _gameDisplayPreferences.GetShowMoveCount();
		showGameTimer = _gameDisplayPreferences.GetShowGameTimer();
		selectedBoardSize = _gameBoardPreferences.GetBoardSize();
		availableColors = new ObservableCollection<TileColorOption>(_tileColorPreferences.GetAvailableColors());
		selectedPrimaryColor = _tileColorPreferences.GetPrimaryColor();
		selectedSecondaryColor = _tileColorPreferences.GetSecondaryColor();

		isColorPaletteVisible = false;
		colorPaletteTarget = null;

		BoardSizeOptions = new List<BoardSizeOption>
		{
			new BoardSizeOption(BoardSize.Easy, "Easy 3×3", SelectedBoardSize == BoardSize.Easy),
			new BoardSizeOption(BoardSize.Medium, "Medium 5×5", SelectedBoardSize == BoardSize.Medium),
			new BoardSizeOption(BoardSize.Hard, "Hard 9×9", SelectedBoardSize == BoardSize.Hard)
		};

		UpdateBoardSizeSelections();
	}

	[RelayCommand]
	private async Task BackToGame()
	{
		await Microsoft.Maui.Controls.Shell.Current.GoToAsync("//GamePage");
	}

	[RelayCommand]
	public void SelectPaletteColor(TileColorOption option)
	{
		if (ColorPaletteTarget == "Secondary")
			SetSecondaryColor(option);
		else
			SetPrimaryColor(option);
	}

	[RelayCommand]
	private void SetBoardSize(BoardSize size)
	{
		if (SelectedBoardSize != size)
		{
			SelectedBoardSize = size;
			_gameBoardPreferences.SetBoardSize(size);
			UpdateBoardSizeSelections();
		}
	}

	[RelayCommand]
	private void ResetToDefaults()
	{
		// Theme reset is handled by ThemeViewModel. Do not reset theme here.
		_gameBoardPreferences.Reset();
		_gameDisplayPreferences.Reset();
		_tileColorPreferences.Reset();

		// Theme is now managed by ThemeViewModel

		ShowMoveCount = _gameDisplayPreferences.GetShowMoveCount();
		ShowGameTimer = _gameDisplayPreferences.GetShowGameTimer();
		SelectedBoardSize = _gameBoardPreferences.GetBoardSize();
		AvailableColors = new ObservableCollection<TileColorOption>(_tileColorPreferences.GetAvailableColors());
		SelectedPrimaryColor = _tileColorPreferences.GetPrimaryColor();
		SelectedSecondaryColor = _tileColorPreferences.GetSecondaryColor();
		IsColorPaletteVisible = false;
		ColorPaletteTarget = null;
		UpdateBoardSizeSelections();
	}

	private void UpdateBoardSizeSelections()
	{
		BoardSizeOptions = new List<BoardSizeOption>
		{
			new BoardSizeOption(BoardSize.Easy, "Easy 3×3", SelectedBoardSize == BoardSize.Easy),
			new BoardSizeOption(BoardSize.Medium, "Medium 5×5", SelectedBoardSize == BoardSize.Medium),
			new BoardSizeOption(BoardSize.Hard, "Hard 9×9", SelectedBoardSize == BoardSize.Hard)
		};
	}

	[RelayCommand]
	private void SetPrimaryColor(TileColorOption option)
	{
		if (SelectedPrimaryColor != option)
		{
			SelectedPrimaryColor = option;
			_tileColorPreferences.SetPrimaryColorKey(option.Key);
		}
		IsColorPaletteVisible = false;
		ColorPaletteTarget = null;
	}

	[RelayCommand]
	private void SetSecondaryColor(TileColorOption option)
	{
		if (SelectedSecondaryColor != option)
		{
			SelectedSecondaryColor = option;
			_tileColorPreferences.SetSecondaryColorKey(option.Key);
		}
		IsColorPaletteVisible = false;
		ColorPaletteTarget = null;
	}

	[RelayCommand]
	private void ShowColorPalette(string? target)
	{
		IsColorPaletteVisible = true;
		ColorPaletteTarget = target;
	}

	[RelayCommand]
	private void HideColorPalette()
	{
		IsColorPaletteVisible = false;
		ColorPaletteTarget = null;
	}

	// Theme is now managed by ThemeViewModel

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
		UpdateBoardSizeSelections();
	}
}
