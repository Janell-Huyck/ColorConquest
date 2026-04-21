
using ColorConquest.Core.Services;
using ColorConquest.Core.ViewModels;
using Xunit;

namespace ColorConquest.Tests.ViewModels;

public class ThemeViewModelTests
{
    [Fact]
    public void ThemeViewModel_Initializes_FromPreferences()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        theme.SaveTheme(UserTheme.Dark);
        var vm = new ThemeViewModel(theme);
        Assert.True(vm.IsDarkTheme);
        theme.SaveTheme(UserTheme.Light);
        vm = new ThemeViewModel(theme);
        Assert.False(vm.IsDarkTheme);
    }

    [Fact]
    public void ThemeViewModel_Toggle_Updates_Preferences()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var vm = new ThemeViewModel(theme);
        vm.IsDarkTheme = true;
        Assert.True(vm.IsDarkTheme);
        Assert.Equal(UserTheme.Dark, theme.GetSavedTheme());
        vm.IsDarkTheme = false;
        Assert.False(vm.IsDarkTheme);
        Assert.Equal(UserTheme.Light, theme.GetSavedTheme());
    }
}