namespace ColorConquest.Views
{
    public partial class AboutPage : ContentPage
    {
        private readonly ColorConquest.Core.ViewModels.AboutViewModel _viewModel;
        public AboutPage()
        {
            InitializeComponent();
            _viewModel = new ColorConquest.Core.ViewModels.AboutViewModel();
            _viewModel.OpenSolveGuideRequested += OnOpenSolveGuideRequested;
            BindingContext = _viewModel;
        }

        private async Task OnOpenSolveGuideRequested()
        {
            await Launcher.Default.OpenAsync(ColorConquest.Core.ViewModels.AboutViewModel.SolveGuideUri);
        }
    }
}
