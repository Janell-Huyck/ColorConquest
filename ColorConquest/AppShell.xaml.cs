namespace ColorConquest
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Shell chrome is applied in App after MainPage is assigned; ctor runs before that assignment.
            if (Application.Current != null)
                Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
            ApplyNavBarTheme(Application.Current?.UserAppTheme ?? AppTheme.Unspecified);
        }

        private void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            ApplyNavBarTheme(e.RequestedTheme);
        }

        private void ApplyNavBarTheme(AppTheme theme)
        {
            var navBarBg = theme == AppTheme.Dark ? Color.FromArgb("#1f1f1f") : Colors.White;
            var navBarText = theme == AppTheme.Dark ? Colors.White : Colors.Black;
            Shell.SetNavBarHasShadow(this, false);
            Shell.SetBackgroundColor(this, navBarBg);
            Shell.SetForegroundColor(this, navBarText);
        }
    }
}
