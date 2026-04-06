namespace ColorConquest.Views
{
    public partial class AboutPage : ContentPage
    {
        private static readonly Uri SolveGuideUri = new("https://youtu.be/LnYCcUc4FIo?si=f1WIOlg7Z-9hUjY9");

        public AboutPage()
        {
            InitializeComponent();
        }

        private async void OnSolveGuideTapped(object? sender, EventArgs e)
        {
            await Launcher.Default.OpenAsync(SolveGuideUri);
        }
    }
}
