using ColorConquest.Services;

namespace ColorConquest
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            ThemeChrome.ApplyToApplication();
        }
    }
}
