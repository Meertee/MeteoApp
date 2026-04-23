namespace MeteoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MeteoItemPage), typeof(MeteoItemPage));
        }
    }
}
