using ColorConquest.Core.ViewModels;
using ColorConquest.Core.Services;
using ColorConquest.Core;
using Xunit;

namespace ColorConquest.Tests.ViewModels;

public class SettingsViewModelTests
{

    private static SettingsViewModel CreateViewModelWithMemoryPrefs()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        return new SettingsViewModel(theme, board, display, tile);
    }

    [Fact]
    public void Initializes_FromPreferences()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        Assert.NotNull(vm.AvailableColors);
        Assert.NotNull(vm.SelectedPrimaryColor);
        Assert.NotNull(vm.SelectedSecondaryColor);
    }

    [Fact]
    public void ChangingIsDarkTheme_UpdatesPreference()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        var initial = vm.IsDarkTheme;
        vm.IsDarkTheme = !initial;
        Assert.Equal(!initial, vm.IsDarkTheme);
    }

    [Fact]
    public void ChangingShowMoveCount_UpdatesPreference()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        var initial = vm.ShowMoveCount;
        vm.ShowMoveCount = !initial;
        Assert.Equal(!initial, vm.ShowMoveCount);
    }

    [Fact]
    public void ChangingShowGameTimer_UpdatesPreference()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        var initial = vm.ShowGameTimer;
        vm.ShowGameTimer = !initial;
        Assert.Equal(!initial, vm.ShowGameTimer);
    }

    [Fact]
    public void ChangingSelectedBoardSize_UpdatesPreference()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        var initial = vm.SelectedBoardSize;
        var newSize = initial == BoardSize.Easy ? BoardSize.Medium : BoardSize.Easy;
        vm.SelectedBoardSize = newSize;
        Assert.Equal(newSize, vm.SelectedBoardSize);
    }
    [Fact]
    public void SetPrimaryColorCommand_UpdatesPrimaryColor()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        var first = vm.AvailableColors[0];
        var second = vm.AvailableColors.Count > 1 ? vm.AvailableColors[1] : first;
        vm.SetPrimaryColorCommand.Execute(second);
        Assert.Equal(second, vm.SelectedPrimaryColor);
        vm.SetPrimaryColorCommand.Execute(first);
        Assert.Equal(first, vm.SelectedPrimaryColor);
    }

    [Fact]
    public void SetSecondaryColorCommand_UpdatesSecondaryColor()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        var first = vm.AvailableColors[0];
        var second = vm.AvailableColors.Count > 1 ? vm.AvailableColors[1] : first;
        vm.SetSecondaryColorCommand.Execute(second);
        Assert.Equal(second, vm.SelectedSecondaryColor);
        vm.SetSecondaryColorCommand.Execute(first);
        Assert.Equal(first, vm.SelectedSecondaryColor);
    }

    [Fact]
    public void SetPrimaryColorCommand_InvalidColor_DoesNotThrow()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        var invalid = new ColorConquest.Core.Services.TileColorOption("invalid", "Invalid", "#000000");
        var ex = Record.Exception(() => vm.SetPrimaryColorCommand.Execute(invalid));
        Assert.Null(ex); // Should not throw
    }

    [Fact]
    public void SetSecondaryColorCommand_InvalidColor_DoesNotThrow()
    {
        var vm = CreateViewModelWithMemoryPrefs();
        var invalid = new ColorConquest.Core.Services.TileColorOption("invalid", "Invalid", "#000000");
        var ex = Record.Exception(() => vm.SetSecondaryColorCommand.Execute(invalid));
        Assert.Null(ex); // Should not throw
    }

    // Optionally, add more tests for preference eventing, two-way sync, etc.
}
