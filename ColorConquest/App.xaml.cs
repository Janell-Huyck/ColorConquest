using ColorConquest.Services;

namespace ColorConquest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            UserAppTheme = ThemePreferences.GetSavedTheme();

            MainPage = new AppShell();
            ThemeChrome.ApplyToApplication();
        }
    }
}
