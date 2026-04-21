using ColorConquest.Core.Services;
using ColorConquest.Core;
using ColorConquest.Core.ViewModels;
using Xunit;

namespace ColorConquest.Tests.ViewModels;

public class SettingsViewModelResetTests
{
    [Fact]
    public void ResetAllToDefaults_ResetsThemeAndSettings()
    {
        var mem = new InMemoryPreferences();
        var themePrefs = new ThemePreferences(mem);
        var boardPrefs = new GameBoardPreferences(mem);
        var displayPrefs = new GameDisplayPreferences(mem);
        var tilePrefs = new TileColorPreferences(mem);
        var themeVm = new ThemeViewModel(themePrefs);
        var vm = new SettingsViewModel(themePrefs, boardPrefs, displayPrefs, tilePrefs, themeVm);

        // Change all settings from defaults
        themeVm.IsDarkTheme = true;
        boardPrefs.SetBoardSize(BoardSize.Hard);
        displayPrefs.SetShowMoveCount(false);
        displayPrefs.SetShowGameTimer(false);
        tilePrefs.SetPrimaryColorKey("red");
        tilePrefs.SetSecondaryColorKey("blue");

        // Act
        vm.ResetAllToDefaultsCommand.Execute(null);

        // Assert theme
        Assert.False(themeVm.IsDarkTheme);
        Assert.Equal(UserTheme.Light, themePrefs.GetSavedTheme());
        // Assert board
        Assert.Equal(BoardSize.Medium, boardPrefs.GetBoardSize());
        // Assert display
        Assert.True(displayPrefs.GetShowMoveCount());
        Assert.True(displayPrefs.GetShowGameTimer());
        // Assert tile colors
        Assert.Equal(tilePrefs.GetAvailableColors()[0].Key, tilePrefs.GetPrimaryColor().Key);
        Assert.Equal(tilePrefs.GetAvailableColors()[1].Key, tilePrefs.GetSecondaryColor().Key);
    }
}
