using ColorConquest.Core.ViewModels;
using ColorConquest.Core.Services;
using ColorConquest.Core;
using Xunit;

namespace ColorConquest.Tests.ViewModels;

public class SettingsResetIntegrationTests
{
    [Fact]
    public void ResetAllPreferences_ResetsAllToDefaults()
    {
        var mem = new InMemoryPreferences();
        var theme = new ThemePreferences(mem);
        var board = new GameBoardPreferences(mem);
        var display = new GameDisplayPreferences(mem);
        var tile = new TileColorPreferences(mem);
        var reset = new PreferencesReset(theme, board, display, tile);

        // Set non-defaults
        theme.SaveTheme(UserTheme.Dark);
        board.SetBoardSize(BoardSize.Hard);
        display.SetShowMoveCount(false);
        display.SetShowGameTimer(false);
        tile.SetPrimaryColorKey("red");
        tile.SetSecondaryColorKey("pink");

        // Act
        reset.ResetAll();

        // Assert
        var themeVal = theme.GetSavedTheme();
        var boardVal = board.GetBoardSize();
        System.Console.WriteLine($"After reset: Theme={themeVal}, Board={boardVal}");
        Assert.Equal(UserTheme.Light, themeVal);
        Assert.Equal(BoardSize.Medium, boardVal);
        var moveCount = display.GetShowMoveCount();
        var gameTimer = display.GetShowGameTimer();
        System.Console.WriteLine($"After reset: ShowMoveCount={moveCount}, ShowGameTimer={gameTimer}");
        Assert.True(moveCount);
        Assert.True(gameTimer);
        var primary = tile.GetPrimaryColorKey();
        var secondary = tile.GetSecondaryColorKey();
        Assert.Equal("blue", primary);
        Assert.Equal("red", secondary);
    }
}
