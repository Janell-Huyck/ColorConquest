namespace ColorConquest
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Shell chrome is applied in App after MainPage is assigned; ctor runs before that assignment.
        }
    }
}
