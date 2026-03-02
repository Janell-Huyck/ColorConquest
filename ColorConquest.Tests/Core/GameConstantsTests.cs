using ColorConquest.Core;
using Xunit;

namespace ColorConquest.Tests.Core;

public class GameConstantsTests
{
    [Fact]
    public void DefaultGridSize_IsFive()
    {
        Assert.Equal(5, GameConstants.DefaultGridSize);
    }
}
