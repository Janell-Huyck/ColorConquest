using ColorConquest.Services;

namespace ColorConquest
{
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services ?? throw new ArgumentNullException(nameof(services));
            UserAppTheme = ThemeChrome.ToAppTheme(AppServices.ThemePreferences.GetSavedTheme());
            MainPage = new AppShell();
            ThemeChrome.ApplyToApplication();
        }
    }
}
