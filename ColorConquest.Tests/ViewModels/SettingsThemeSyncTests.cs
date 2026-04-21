using ColorConquest.Core.ViewModels;
using ColorConquest.Core.Services;
using Xunit;

namespace ColorConquest.Tests.ViewModels;

public class SettingsThemeSyncTests
{
    [Fact]
    public void ViewModel_Initializes_IsDarkTheme_FromPreferences()
    {
        // Arrange
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        var beforeSet = theme.GetSavedTheme();
        theme.SaveTheme(UserTheme.Dark);
        var afterSet = theme.GetSavedTheme();
        var vm = new SettingsViewModel(theme, board, display, tile);
        // Assert
        var vmValue = vm.IsDarkTheme;
        Assert.True(vm.IsDarkTheme, $"ViewModel should reflect dark theme preference on init. BeforeSet: {beforeSet}, AfterSet: {afterSet}, VM: {vmValue}");

        // Reset to known state before next assertion
        theme.SaveTheme(UserTheme.Light);
        vm = new SettingsViewModel(theme, board, display, tile);
        Assert.False(vm.IsDarkTheme, "ViewModel should reflect light theme preference on init");
    }

    [Fact]
    public void Toggling_IsDarkTheme_Updates_Preferences_And_ViewModel()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        theme.SaveTheme(UserTheme.Light);
        var vm = new SettingsViewModel(theme, board, display, tile);
        Assert.False(vm.IsDarkTheme);
        vm.IsDarkTheme = true;
        Assert.True(vm.IsDarkTheme);
        Assert.Equal(UserTheme.Dark, theme.GetSavedTheme());
        vm.IsDarkTheme = false;
        Assert.False(vm.IsDarkTheme);
        Assert.Equal(UserTheme.Light, theme.GetSavedTheme());
    }
}
