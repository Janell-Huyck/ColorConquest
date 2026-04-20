using ColorConquest.Core;
using Xunit;

namespace ColorConquest.Tests.Core;

public class BoardSizeTests
{
    [Theory]
    [InlineData(BoardSize.Easy, 3, 3)]
    [InlineData(BoardSize.Medium, 5, 5)]
    [InlineData(BoardSize.Hard, 9, 9)]
    public void GetDimensions_MatchesBoardSize(BoardSize size, int expectedRows, int expectedCols)
    {
        var (rows, cols) = BoardSizeExtensions.GetDimensions(size);

        Assert.Equal(expectedRows, rows);
        Assert.Equal(expectedCols, cols);
    }

    [Theory]
    [InlineData(3, 3, 8)]
    [InlineData(5, 5, 15)]
    [InlineData(9, 9, 48)]
    public void ScrambleMoveCount_ScalesWithBoardArea(int rows, int cols, int expected)
    {
        Assert.Equal(expected, BoardSizeExtensions.ScrambleMoveCount(rows, cols));
    }

    [Fact]
    public void ScrambleMoveCount_IsAtLeastEight()
    {
        Assert.Equal(8, BoardSizeExtensions.ScrambleMoveCount(1, 1));
    }
}
