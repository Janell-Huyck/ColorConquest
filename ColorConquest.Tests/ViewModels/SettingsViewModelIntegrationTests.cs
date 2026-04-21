using ColorConquest.Core.Services;
using ColorConquest.Core;
using ColorConquest.Core.ViewModels;
using Xunit;

namespace ColorConquest.Tests.ViewModels;

public class SettingsViewModelIntegrationTests
{
    [Fact]
    public void Changing_SelectedBoardSize_UpdatesPreferenceAndProperty()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        board.Reset();
        board.SetBoardSize(BoardSize.Easy);
        var themeVm = new ThemeViewModel(theme);
        var vm = new SettingsViewModel(theme, board, display, tile, themeVm);
        var newSize = BoardSize.Hard;
        var original = vm.SelectedBoardSize;
        Assert.NotEqual(newSize, original); // Should be Easy
        vm.SelectedBoardSize = newSize;
        Assert.Equal(newSize, vm.SelectedBoardSize);
        Assert.Equal(newSize, board.GetBoardSize());
    }

    // Theme is now managed by ThemeViewModel. See ThemeViewModelTests for theme logic tests.

    [Fact]
    public void Changing_ShowMoveCount_UpdatesPreferenceAndProperty()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        var themeVm = new ThemeViewModel(theme);
        var vm = new SettingsViewModel(theme, board, display, tile, themeVm);
        var newValue = !vm.ShowMoveCount;
        vm.ShowMoveCount = newValue;
        Assert.Equal(newValue, vm.ShowMoveCount);
        Assert.Equal(newValue, display.GetShowMoveCount());
    }

    [Fact]
    public void Changing_ShowGameTimer_UpdatesPreferenceAndProperty()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        var themeVm = new ThemeViewModel(theme);
        var vm = new SettingsViewModel(theme, board, display, tile, themeVm);
        var newValue = !vm.ShowGameTimer;
        vm.ShowGameTimer = newValue;
        Assert.Equal(newValue, vm.ShowGameTimer);
        Assert.Equal(newValue, display.GetShowGameTimer());
    }
}
