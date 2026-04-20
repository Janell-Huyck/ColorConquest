using ColorConquest.Core;
using ColorConquest.Services;
using Microsoft.Maui.Controls.Shapes;

namespace ColorConquest.Views;

public partial class SettingsPage : ContentPage
{
    private enum PaletteTarget
    {
        None,
        Primary,
        Secondary
    }

    private bool _suppressThemeToggle;
    private bool _suppressMoveCountToggle;
    private bool _suppressGameTimerToggle;
    private IReadOnlyList<TileColorOption> _availableColors = Array.Empty<TileColorOption>();
    private PaletteTarget _activePaletteTarget = PaletteTarget.None;

    public SettingsPage()
    {
        InitializeComponent();
        _suppressThemeToggle = true;
        ThemeSwitch.IsToggled = ThemePreferences.GetSavedTheme() == AppTheme.Dark;
        _suppressThemeToggle = false;

        _suppressMoveCountToggle = true;
        ShowMoveCountSwitch.IsToggled = GameDisplayPreferences.GetShowMoveCount();
        _suppressMoveCountToggle = false;

        _suppressGameTimerToggle = true;
        ShowGameTimerSwitch.IsToggled = GameDisplayPreferences.GetShowGameTimer();
        _suppressGameTimerToggle = false;

        _availableColors = TileColorPreferences.GetAvailableColors();
        ApplyForcedThemeToUi();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ApplyForcedThemeToUi();
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += OnAppRequestedThemeChanged;
    }

    protected override void OnDisappearing()
    {
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged -= OnAppRequestedThemeChanged;
        base.OnDisappearing();
    }

    private void OnAppRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e) =>
        ApplyForcedThemeToUi();

    private void ApplyForcedThemeToUi()
    {
        ThemeChrome.ApplyToApplication();
        var dark = ThemeChrome.IsDarkFromPreferences();

        var surfaceBg = ThemeChrome.Surface(dark);
        var stroke = dark ? Color.FromArgb("#404040") : Color.FromArgb("#ACACAC");
        // Light: semi-transparent black over white reads as gray. Dark: darker veil over dark page.
        var scrim = dark ? Color.FromArgb("#CC000000") : Color.FromArgb("#66000000");
        var headline = dark ? Colors.White : Colors.Black;
        var muted = dark ? Color.FromArgb("#A3A3A3") : Color.FromArgb("#6E6E6E");
        var chevron = dark ? Color.FromArgb("#B0B0B0") : Color.FromArgb("#555555");

        BackgroundColor = surfaceBg;
        SettingsRootGrid.BackgroundColor = surfaceBg;
        SettingsScroll.BackgroundColor = surfaceBg;

        AppearanceSectionBorder.BackgroundColor = surfaceBg;
        AppearanceSectionBorder.Stroke = stroke;

        GameplaySectionBorder.BackgroundColor = surfaceBg;
        GameplaySectionBorder.Stroke = stroke;
        GameplayHeaderLabel.TextColor = headline;
        DifficultyTitleLabel.TextColor = headline;
        DifficultySubtitleLabel.TextColor = muted;
        DifficultyEasyBorder.BackgroundColor = surfaceBg;
        DifficultyEasyBorder.Stroke = stroke;
        DifficultyMediumBorder.BackgroundColor = surfaceBg;
        DifficultyMediumBorder.Stroke = stroke;
        DifficultyHardBorder.BackgroundColor = surfaceBg;
        DifficultyHardBorder.Stroke = stroke;
        DifficultyEasyLabel.TextColor = headline;
        DifficultyMediumLabel.TextColor = headline;
        DifficultyHardLabel.TextColor = headline;
        DifficultyEasyCheckLabel.TextColor = chevron;
        DifficultyMediumCheckLabel.TextColor = chevron;
        DifficultyHardCheckLabel.TextColor = chevron;

        PrimaryTileStrokeBorder.Stroke = stroke;
        PrimaryPickerRowBorder.BackgroundColor = surfaceBg;
        PrimaryPickerRowBorder.Stroke = stroke;
        SecondaryTileStrokeBorder.Stroke = stroke;
        SecondaryPickerRowBorder.BackgroundColor = surfaceBg;
        SecondaryPickerRowBorder.Stroke = stroke;

        ColorPaletteOverlay.BackgroundColor = scrim;
        PaletteModalBorder.BackgroundColor = surfaceBg;
        PaletteModalBorder.Stroke = stroke;

        PaletteTitleLabel.TextColor = headline;
        PaletteInstructionLabel.TextColor = muted;

        SettingsPageTitleLabel.TextColor = headline;
        AppearanceHeaderLabel.TextColor = headline;
        DarkModeTitleLabel.TextColor = headline;
        DarkModeSubtitleLabel.TextColor = muted;
        ShowMoveTitleLabel.TextColor = headline;
        ShowMoveSubtitleLabel.TextColor = muted;
        ShowGameTimerTitleLabel.TextColor = headline;
        ShowGameTimerSubtitleLabel.TextColor = muted;
        TileColorsTitleLabel.TextColor = headline;
        TileColorsSubtitleLabel.TextColor = muted;
        PrimarySectionLabel.TextColor = headline;
        SecondarySectionLabel.TextColor = headline;
        PrimaryColorNameLabel.TextColor = headline;
        SecondaryColorNameLabel.TextColor = headline;
        PrimaryChevronLabel.TextColor = chevron;
        SecondaryChevronLabel.TextColor = chevron;
        FooterHintLabel.TextColor = muted;

        // Always rebuild: Mac Catalyst can leave stale native views (non-zero child count but nothing
        // visible) after theme changes or hiding the overlay; skipping rebuild was unreliable.
        BuildPaletteSwatches(dark);

        UpdateColorLabelsAndPreviews();
        UpdateDifficultySelectionUi();
    }

    private void OnThemeToggled(object? sender, ToggledEventArgs e)
    {
        if (_suppressThemeToggle)
            return;

        var selectedTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        ThemePreferences.SaveTheme(selectedTheme);

        if (Application.Current is not null)
            Application.Current.UserAppTheme = selectedTheme;

        ApplyForcedThemeToUi();
    }

    private void OnShowMoveCountToggled(object? sender, ToggledEventArgs e)
    {
        if (_suppressMoveCountToggle)
            return;

        GameDisplayPreferences.SetShowMoveCount(e.Value);
    }

    private void OnShowGameTimerToggled(object? sender, ToggledEventArgs e)
    {
        if (_suppressGameTimerToggle)
            return;

        GameDisplayPreferences.SetShowGameTimer(e.Value);
    }

    private void UpdateDifficultySelectionUi()
    {
        var d = GameBoardPreferences.GetDifficulty();
        DifficultyEasyCheckLabel.IsVisible = d == BoardDifficulty.Easy;
        DifficultyMediumCheckLabel.IsVisible = d == BoardDifficulty.Medium;
        DifficultyHardCheckLabel.IsVisible = d == BoardDifficulty.Hard;
    }

    private async void OnDifficultyEasyTapped(object? sender, TappedEventArgs e) =>
        await TryApplyDifficultyAsync(BoardDifficulty.Easy);

    private async void OnDifficultyMediumTapped(object? sender, TappedEventArgs e) =>
        await TryApplyDifficultyAsync(BoardDifficulty.Medium);

    private async void OnDifficultyHardTapped(object? sender, TappedEventArgs e) =>
        await TryApplyDifficultyAsync(BoardDifficulty.Hard);

    private async Task TryApplyDifficultyAsync(BoardDifficulty selected)
    {
        var current = GameBoardPreferences.GetDifficulty();
        if (selected == current)
            return;

        if (GameSessionSnapshot.LastReportedMoveCount > 0)
        {
            var confirm = await DisplayAlert(
                "Change difficulty?",
                "You have moves on the current game. Changing difficulty will replace the board and all progress will be lost.",
                "Change",
                "Cancel");
            if (!confirm)
            {
                UpdateDifficultySelectionUi();
                return;
            }
        }

        GameBoardPreferences.SetDifficulty(selected);
        GameSessionSnapshot.ClearProgress();
        UpdateDifficultySelectionUi();
    }

    private void OnPrimaryPaletteRowTapped(object? sender, TappedEventArgs e)
    {
        OpenPalette(PaletteTarget.Primary);
    }

    private void OnSecondaryPaletteRowTapped(object? sender, TappedEventArgs e)
    {
        OpenPalette(PaletteTarget.Secondary);
    }

    private void OpenPalette(PaletteTarget target)
    {
        _activePaletteTarget = target;
        PaletteTitleLabel.Text = target == PaletteTarget.Primary
            ? "Choose primary tile color"
            : "Choose secondary tile color";
        ApplyForcedThemeToUi();
        ColorPaletteOverlay.IsVisible = true;
    }

    private void ClosePalette()
    {
        ColorPaletteOverlay.IsVisible = false;
        _activePaletteTarget = PaletteTarget.None;
    }

    private void OnColorPaletteBackdropTapped(object? sender, TappedEventArgs e)
    {
        ClosePalette();
    }

    private void OnColorPaletteCancelClicked(object? sender, EventArgs e)
    {
        ClosePalette();
    }

    private void OnPaletteSwatchTapped(TileColorOption option)
    {
        switch (_activePaletteTarget)
        {
            case PaletteTarget.Primary:
                TileColorPreferences.SetPrimaryColorKey(option.Key);
                break;
            case PaletteTarget.Secondary:
                TileColorPreferences.SetSecondaryColorKey(option.Key);
                break;
            default:
                return;
        }

        UpdateColorLabelsAndPreviews();
        ClosePalette();
    }

    private void UpdateColorLabelsAndPreviews()
    {
        var primary = TileColorPreferences.GetPrimaryColor();
        var secondary = TileColorPreferences.GetSecondaryColor();
        PrimaryTilePreview.Color = Color.FromArgb(primary.Hex);
        SecondaryTilePreview.Color = Color.FromArgb(secondary.Hex);
        PrimaryColorNameLabel.Text = primary.Name;
        SecondaryColorNameLabel.Text = secondary.Name;
    }

    private void BuildPaletteSwatches(bool dark)
    {
        PaletteSwatchesGrid.Children.Clear();
        PaletteSwatchesGrid.ColumnDefinitions.Clear();
        PaletteSwatchesGrid.RowDefinitions.Clear();

        const int columns = 4;
        for (var c = 0; c < columns; c++)
            PaletteSwatchesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        var rowCount = (int)Math.Ceiling(_availableColors.Count / (double)columns);
        for (var r = 0; r < rowCount; r++)
            PaletteSwatchesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var swatchStroke = dark ? Color.FromArgb("#555555") : Color.FromArgb("#ACACAC");

        for (var i = 0; i < _availableColors.Count; i++)
        {
            var option = _availableColors[i];
            var row = i / columns;
            var col = i % columns;

            var border = new Border
            {
                HeightRequest = 52,
                StrokeThickness = 1,
                Stroke = swatchStroke,
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                BackgroundColor = Color.FromArgb(option.Hex)
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => OnPaletteSwatchTapped(option);
            border.GestureRecognizers.Add(tap);

            Grid.SetRow(border, row);
            Grid.SetColumn(border, col);
            PaletteSwatchesGrid.Children.Add(border);
        }
    }
}
