using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace ColorConquest.Core.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    public static readonly Uri SolveGuideUri = new("https://youtu.be/LnYCcUc4FIo?si=f1WIOlg7Z-9hUjY9");

    [RelayCommand]
    private async Task OpenSolveGuideAsync()
    {
        if (OpenSolveGuideRequested is not null)
            await OpenSolveGuideRequested.Invoke();
    }

    public event Func<Task>? OpenSolveGuideRequested;
}
