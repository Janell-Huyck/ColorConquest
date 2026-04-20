using System.Threading.Tasks;
using ColorConquest.Core.ViewModels;
using Xunit;

namespace ColorConquest.Tests.Core.ViewModels;

public class AboutViewModelTests
{
    [Fact]
    public async Task OpenSolveGuideCommand_RaisesEvent()
    {
        var vm = new AboutViewModel();
        bool called = false;
        vm.OpenSolveGuideRequested += () => { called = true; return Task.CompletedTask; };
        await vm.OpenSolveGuideCommand.ExecuteAsync(null);
        Assert.True(called);
    }
}
