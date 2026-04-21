using CommunityToolkit.Mvvm.Input;

using ColorConquest.Core.ViewModels;
using ColorConquest.Core;
using ColorConquest.Core.Services;
using Microsoft.Maui.Controls.Shapes;
using ColorConquest.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace ColorConquest.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsViewModel ViewModel => (SettingsViewModel)BindingContext;
    private ThemeViewModel ThemeVm;

    private void ThemeSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if (ThemeVm != null)
            ThemeVm.IsDarkTheme = e.Value;
    }

    // Parameterless constructor for Shell navigation (resolves ViewModel from DI)
    public SettingsPage() : this((App.Services!).GetService(typeof(SettingsViewModel)) as SettingsViewModel
        ?? throw new InvalidOperationException("SettingsViewModel not found in DI container."))
    {
    }

    // Restored: Applies theming and layout to the settings page UI
    private void ApplyForcedThemeToUi(bool? forceDark = null)
    {
        var dark = forceDark ?? ThemeChrome.IsDarkFromPreferences();
        ThemeChrome.ApplyToApplication(dark);

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
        ThemeVm = (App.Services!).GetService(typeof(ThemeViewModel)) as ThemeViewModel
            ?? throw new InvalidOperationException("ThemeViewModel not found in DI container.");
        // Listen for theme changes via messenger
        WeakReferenceMessenger.Default.Register<ThemeChangedMessage>(this, (r, m) => ApplyTheme(m.IsDarkTheme));
        ApplyTheme(ThemeVm.IsDarkTheme);

    }

    private void ApplyTheme(bool isDark)
    {
        if (Application.Current is not null)
            Application.Current.UserAppTheme = ThemeChrome.ToAppTheme(isDark ? UserTheme.Dark : UserTheme.Light);
        ApplyForcedThemeToUi(isDark);
        if (ThemeSwitch != null)
            ThemeSwitch.IsToggled = isDark;
    }

    protected override void OnAppearing()
    {
        if (BindingContext == null)
            BindingContext = App.Services.GetService<SettingsViewModel>();
        base.OnAppearing();
        // Always apply the current theme
        ApplyTheme(ThemeVm.IsDarkTheme);
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged += OnAppRequestedThemeChanged;
    }
    // No longer needed: OnThemeChanged handled by messenger

    protected override void OnDisappearing()
    {
        if (Application.Current is not null)
            Application.Current.RequestedThemeChanged -= OnAppRequestedThemeChanged;
        base.OnDisappearing();
    }

    private void OnAppRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e) =>
        ApplyForcedThemeToUi();

    // ...existing code for ApplyForcedThemeToUi() can remain for UI theming...

}
