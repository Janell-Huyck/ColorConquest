using ColorConquest.Core.Services;
using ColorConquest.Core;
using ColorConquest.Core.ViewModels;
using ColorConquest.Core;
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
        var vm = new SettingsViewModel(theme, board, display, tile);
        var newSize = BoardSize.Hard;
        var original = vm.SelectedBoardSize;
        Assert.NotEqual(newSize, original); // Should be Easy
        vm.SelectedBoardSize = newSize;
        Assert.Equal(newSize, vm.SelectedBoardSize);
        Assert.Equal(newSize, board.GetBoardSize());
    }

    [Fact]
    public void Changing_IsDarkTheme_UpdatesPreferenceAndProperty()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        theme.Reset();
        theme.SaveTheme(UserTheme.Light);
        var beforeSet = theme.GetSavedTheme();
        var vm = new SettingsViewModel(theme, board, display, tile);
        var newValue = !vm.IsDarkTheme;
        vm.IsDarkTheme = newValue;
        var pref = theme.GetSavedTheme();
        var vmValue = vm.IsDarkTheme;
        Assert.Equal(newValue, vm.IsDarkTheme);
        Assert.True(vm.IsDarkTheme, $"Expected IsDarkTheme true, got {vm.IsDarkTheme}, BeforeSet: {beforeSet}, Pref: {pref}, VM: {vmValue}");
        Assert.True(pref == UserTheme.Dark, $"Expected ThemePreferences.Dark, got {pref}");
        Assert.Equal(newValue ? UserTheme.Dark : UserTheme.Light, pref);
    }

    [Fact]
    public void Changing_ShowMoveCount_UpdatesPreferenceAndProperty()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        var vm = new SettingsViewModel(theme, board, display, tile);
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
        var vm = new SettingsViewModel(theme, board, display, tile);
        var newValue = !vm.ShowGameTimer;
        vm.ShowGameTimer = newValue;
        Assert.Equal(newValue, vm.ShowGameTimer);
        Assert.Equal(newValue, display.GetShowGameTimer());
    }
}
