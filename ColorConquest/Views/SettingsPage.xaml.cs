
using ColorConquest.Core.ViewModels;
using ColorConquest.Core;
using ColorConquest.Core.Services;
using Microsoft.Maui.Controls.Shapes;
using ColorConquest.Services;

namespace ColorConquest.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsViewModel ViewModel => (SettingsViewModel)BindingContext;

    // Parameterless constructor for Shell navigation (resolves ViewModel from DI)
    public SettingsPage() : this(App.Services.GetService(typeof(SettingsViewModel)) as SettingsViewModel
        ?? throw new InvalidOperationException("SettingsViewModel not found in DI container."))
    {
    }

    // Restored: Applies theming and layout to the settings page UI
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
        BoardSizeTitleLabel.TextColor = headline;
        BoardSizeSubtitleLabel.TextColor = muted;
        // Board size UI is now handled by CollectionView and DataTemplate.

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
        // All palette and selection UI is now handled by ViewModel and XAML bindings.
    }

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        // Subscribe to ViewModel property changes for immediate theme update
        if (viewModel != null)
            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        ApplyForcedThemeToUi();
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SettingsViewModel.IsDarkTheme))
        {
            // Update the app-wide theme immediately
            if (BindingContext is SettingsViewModel svm)
            {
                var appTheme = svm.IsDarkTheme
                    ? ThemeChrome.ToAppTheme(ColorConquest.Core.Services.UserTheme.Dark)
                    : ThemeChrome.ToAppTheme(ColorConquest.Core.Services.UserTheme.Light);
                if (Application.Current is not null)
                    Application.Current.UserAppTheme = appTheme;
            }
            ApplyForcedThemeToUi();
        }
    }

    protected override void OnAppearing()
    {
        if (BindingContext == null)
            BindingContext = App.Services.GetService<SettingsViewModel>();
        // No ThemeChanged event in new ViewModel
        base.OnAppearing();
        // Always sync ViewModel and toggle with the current app theme
        var isDark = ThemeChrome.IsDarkFromPreferences();
        if (BindingContext is ColorConquest.Core.ViewModels.SettingsViewModel svm)
        {
            svm.IsDarkTheme = isDark;
            // Sync app theme to ViewModel
            Application.Current.UserAppTheme = ThemeChrome.ToAppTheme(isDark ? ColorConquest.Core.Services.UserTheme.Dark : ColorConquest.Core.Services.UserTheme.Light);
        }
        if (ThemeSwitch != null)
            ThemeSwitch.IsToggled = isDark;
        ApplyForcedThemeToUi();
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += OnAppRequestedThemeChanged;
    }
    private void OnThemeChanged(object? sender, bool isDark)
    {
        if (Application.Current is not null)
            Application.Current.UserAppTheme = ThemeChrome.ToAppTheme(isDark ? ColorConquest.Core.Services.UserTheme.Dark : ColorConquest.Core.Services.UserTheme.Light);
        if (ThemeSwitch != null)
            ThemeSwitch.IsToggled = isDark;
    }

    protected override void OnDisappearing()
    {
        // Unsubscribe from ViewModel property changes
        if (BindingContext is SettingsViewModel vm)
            vm.PropertyChanged -= OnViewModelPropertyChanged;
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged -= OnAppRequestedThemeChanged;
        base.OnDisappearing();
    }

    private void OnAppRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e) =>
        ApplyForcedThemeToUi();

    // ...existing code for ApplyForcedThemeToUi() can remain for UI theming...

}
