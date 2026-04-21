using ColorConquest.Core.ViewModels;
using ColorConquest.Core.Services;
using Xunit;
using System.Linq;

namespace ColorConquest.Tests.Core.ViewModels
{
    public class SettingsViewModelColorPaletteTests
    {
        private SettingsViewModel CreateVm()
        {
            var mem = new InMemoryPreferences();
            var theme = new ThemePreferences(mem);
            var board = new GameBoardPreferences(mem);
            var display = new GameDisplayPreferences(mem);
            var tile = new TileColorPreferences(mem);
            var themeVm = new ThemeViewModel(theme);
            return new SettingsViewModel(theme, board, display, tile, themeVm);
        }

        [Fact]
        public void ShowColorPalette_OpensModalAndSetsTarget()
        {
            var vm = CreateVm();
            Assert.False(vm.IsColorPaletteVisible);
            Assert.Null(vm.ColorPaletteTarget);
            vm.ShowColorPaletteCommand.Execute("Primary");
            Assert.True(vm.IsColorPaletteVisible);
            Assert.Equal("Primary", vm.ColorPaletteTarget);
        }

        [Fact]
        public void HideColorPalette_ClosesModalAndClearsTarget()
        {
            var vm = CreateVm();
            vm.ShowColorPaletteCommand.Execute("Secondary");
            vm.HideColorPaletteCommand.Execute(null);
            Assert.False(vm.IsColorPaletteVisible);
            Assert.Null(vm.ColorPaletteTarget);
        }

        [Fact]
        public void SetPrimaryColor_ClosesModalAndUpdatesColor()
        {
            var vm = CreateVm();
            var color = vm.AvailableColors.First();
            vm.ShowColorPaletteCommand.Execute("Primary");
            vm.SetPrimaryColorCommand.Execute(color);
            Assert.False(vm.IsColorPaletteVisible);
            Assert.Null(vm.ColorPaletteTarget);
            Assert.Equal(color, vm.SelectedPrimaryColor);
        }

        [Fact]
        public void SetSecondaryColor_ClosesModalAndUpdatesColor()
        {
            var vm = CreateVm();
            var color = vm.AvailableColors.Last();
            vm.ShowColorPaletteCommand.Execute("Secondary");
            vm.SetSecondaryColorCommand.Execute(color);
            Assert.False(vm.IsColorPaletteVisible);
            Assert.Null(vm.ColorPaletteTarget);
            Assert.Equal(color, vm.SelectedSecondaryColor);
        }
    }
}
