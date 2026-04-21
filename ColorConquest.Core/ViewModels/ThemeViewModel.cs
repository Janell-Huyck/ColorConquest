using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ColorConquest.Core.Services;

namespace ColorConquest.Core.ViewModels;

public partial class ThemeViewModel : ObservableRecipient
{
    private readonly ThemePreferences _themePreferences;

    [ObservableProperty]
    private bool isDarkTheme;

    public ThemeViewModel(ThemePreferences themePreferences)
    {
        _themePreferences = themePreferences;
        isDarkTheme = _themePreferences.GetSavedTheme() == UserTheme.Dark;
    }

    partial void OnIsDarkThemeChanged(bool value)
    {
        _themePreferences.SaveTheme(value ? UserTheme.Dark : UserTheme.Light);
        WeakReferenceMessenger.Default.Send(new ThemeChangedMessage(value));
    }

    public void ResetToDefault()
    {
        _themePreferences.Reset();
        IsDarkTheme = _themePreferences.GetSavedTheme() == UserTheme.Dark;
    }
}

public sealed record ThemeChangedMessage(bool IsDarkTheme);
