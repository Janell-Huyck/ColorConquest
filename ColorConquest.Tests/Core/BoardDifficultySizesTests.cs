using ColorConquest.Core;
using Xunit;

namespace ColorConquest.Tests.Core;

public class BoardDifficultySizesTests
{
    [Theory]
    [InlineData(BoardDifficulty.Easy, 3, 3)]
    [InlineData(BoardDifficulty.Medium, 5, 5)]
    [InlineData(BoardDifficulty.Hard, 9, 9)]
    public void GetDimensions_MatchesDifficulty(BoardDifficulty difficulty, int expectedRows, int expectedCols)
    {
        var (rows, cols) = BoardDifficultySizes.GetDimensions(difficulty);

        Assert.Equal(expectedRows, rows);
        Assert.Equal(expectedCols, cols);
    }

    [Theory]
    [InlineData(3, 3, 8)]
    [InlineData(5, 5, 15)]
    [InlineData(9, 9, 48)]
    public void ScrambleMoveCount_ScalesWithBoardArea(int rows, int cols, int expected)
    {
        Assert.Equal(expected, BoardDifficultySizes.ScrambleMoveCount(rows, cols));
    }

    [Fact]
    public void ScrambleMoveCount_IsAtLeastEight()
    {
        Assert.Equal(8, BoardDifficultySizes.ScrambleMoveCount(1, 1));
    }
}
