using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace ColorConquest.Core.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [RelayCommand]
    private async Task StartGameAsync()
    {
        // Navigation will be handled by the view via event or service
        if (StartGameRequested is not null)
            await StartGameRequested.Invoke();
    }

    public event Func<Task>? StartGameRequested;
}
