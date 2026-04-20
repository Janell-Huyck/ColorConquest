using System.Threading.Tasks;
using ColorConquest.Core.ViewModels;
using Xunit;

namespace ColorConquest.Tests.Core.ViewModels;

public class HomeViewModelTests
{
    [Fact]
    public async Task StartGameCommand_RaisesEvent()
    {
        var vm = new HomeViewModel();
        bool called = false;
        vm.StartGameRequested += () => { called = true; return Task.CompletedTask; };
        await vm.StartGameCommand.ExecuteAsync(null);
        Assert.True(called);
    }
}
