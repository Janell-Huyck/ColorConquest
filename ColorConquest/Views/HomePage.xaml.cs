namespace ColorConquest.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly ColorConquest.Core.ViewModels.HomeViewModel _viewModel;
        private readonly ColorConquest.Core.ViewModels.HomeAnimationViewModel _animationViewModel;
        public HomePage()
        {
            InitializeComponent();
            _viewModel = new ColorConquest.Core.ViewModels.HomeViewModel();
            _animationViewModel = new ColorConquest.Core.ViewModels.HomeAnimationViewModel();
            _viewModel.StartGameRequested += OnStartGameRequested;
            BindingContext = new HomePageContext(_viewModel, _animationViewModel);
            // Start the demo animation automatically when the page loads
            _ = _animationViewModel.PlayDemoAsync();
        }

        private async Task OnStartGameRequested()
        {
            await Shell.Current.GoToAsync("//GamePage");
        }
    }
}
