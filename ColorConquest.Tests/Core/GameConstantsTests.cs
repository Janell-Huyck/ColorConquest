using ColorConquest.Core;
using Xunit;

namespace ColorConquest.Tests.Core;

public class GameConstantsTests
{
    [Fact]
    public void DefaultRowCount_IsFive()
    {
        Assert.Equal(5, GameConstants.DefaultRowCount);
    }

    [Fact]
    public void DefaultColumnCount_IsFive()
    {
        Assert.Equal(5, GameConstants.DefaultColumnCount);
    }
}
