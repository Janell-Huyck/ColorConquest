namespace ColorConquest.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly ColorConquest.Core.ViewModels.HomeViewModel _viewModel;
        public HomePage()
        {
            InitializeComponent();
            _viewModel = new ColorConquest.Core.ViewModels.HomeViewModel();
            _viewModel.StartGameRequested += OnStartGameRequested;
            BindingContext = _viewModel;
        }

        private async Task OnStartGameRequested()
        {
            await Shell.Current.GoToAsync("//GamePage");
        }
    }
}
