using Xunit;
using ColorConquest.Core.Services;

namespace ColorConquest.Tests.Core.Services
{
    public class PreferencesServicesTests
    {
        private InMemoryPreferences prefs;
        private ThemePreferences themePrefs;
        private GameBoardPreferences boardPrefs;
        private GameDisplayPreferences displayPrefs;
        private TileColorPreferences colorPrefs;

        public PreferencesServicesTests()
        {
            prefs = new InMemoryPreferences();
            themePrefs = new ThemePreferences(prefs);
            boardPrefs = new GameBoardPreferences(prefs);
            displayPrefs = new GameDisplayPreferences(prefs);
            colorPrefs = new TileColorPreferences(prefs);
        }

        private void ResetAll()
        {
            themePrefs.Reset();
            boardPrefs.Reset();
            displayPrefs.Reset();
            colorPrefs.Reset();
        }

        [Fact]
        public void ThemePreferences_Persists_And_Resets()
        {
            ResetAll();
            themePrefs.SaveTheme(UserTheme.Dark);
            Assert.Equal(UserTheme.Dark, themePrefs.GetSavedTheme());
            themePrefs.Reset();
            Assert.Equal(UserTheme.Light, themePrefs.GetSavedTheme());
            ResetAll();
        }

        [Fact]
        public void GameBoardPreferences_Persists_And_Resets()
        {
            ResetAll();
            boardPrefs.SetBoardSize(ColorConquest.Core.BoardSize.Hard);
            Assert.Equal(ColorConquest.Core.BoardSize.Hard, boardPrefs.GetBoardSize());
            boardPrefs.Reset();
            Assert.Equal(ColorConquest.Core.BoardSize.Medium, boardPrefs.GetBoardSize());
            ResetAll();
        }

        [Fact]
        public void GameDisplayPreferences_Persists_And_Resets()
        {
            ResetAll();
            displayPrefs.SetShowMoveCount(false);
            displayPrefs.SetShowGameTimer(false);
            Assert.False(displayPrefs.GetShowMoveCount());
            Assert.False(displayPrefs.GetShowGameTimer());
            displayPrefs.Reset();
            Assert.True(displayPrefs.GetShowMoveCount());
            Assert.True(displayPrefs.GetShowGameTimer());
            ResetAll();
        }

        [Fact]
        public void TileColorPreferences_Persists_And_Resets()
        {
            ResetAll();
            var allColors = colorPrefs.GetAvailableColors();
            Assert.True(allColors.Count >= 2, "At least two colors must be defined for this test.");
            var newPrimary = allColors.Count > 2 ? allColors[2] : allColors[0];
            var newSecondary = allColors.Count > 3 ? allColors[3] : allColors[1];
            colorPrefs.SetPrimaryColorKey(newPrimary.Key);
            colorPrefs.SetSecondaryColorKey(newSecondary.Key);
            Assert.Equal(newPrimary.Key, colorPrefs.GetPrimaryColor().Key);
            Assert.Equal(newSecondary.Key, colorPrefs.GetSecondaryColor().Key);
            colorPrefs.Reset();
            Assert.Equal(allColors[0].Key, colorPrefs.GetPrimaryColor().Key);
            Assert.Equal(allColors[1].Key, colorPrefs.GetSecondaryColor().Key);
            ResetAll();
        }

        [Fact]
        public void TileColorPreferences_Handles_Empty_Colors_List()
        {
            // Arrange: create a subclass with empty Colors
            var emptyPrefs = new TileColorPreferences_Empty(prefs);
            // Act & Assert
            Assert.Equal(string.Empty, emptyPrefs.GetPrimaryColorKey());
            Assert.Equal(string.Empty, emptyPrefs.GetSecondaryColorKey());
        }

        [Fact]
        public void TileColorPreferences_Handles_One_Color()
        {
            var onePrefs = new TileColorPreferences_One(prefs);
            Assert.Equal("only", onePrefs.GetPrimaryColorKey());
            Assert.Equal("only", onePrefs.GetSecondaryColorKey());
        }

        private class TileColorPreferences_Empty : TileColorPreferences
        {
            public TileColorPreferences_Empty(ColorConquest.Core.Services.IPreferences p) : base(p) { }
            protected override IReadOnlyList<TileColorOption> Colors => new List<TileColorOption>();
        }
        private class TileColorPreferences_One : TileColorPreferences
        {
            public TileColorPreferences_One(ColorConquest.Core.Services.IPreferences p) : base(p) { }
            protected override IReadOnlyList<TileColorOption> Colors => new List<TileColorOption> { new TileColorOption("only", "Only", "#000000") };
        }
    }
}
