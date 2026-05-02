using MeteoApp.Views;

namespace MeteoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(WeatherLocationPage), typeof(WeatherLocationPage));
            Routing.RegisterRoute(nameof(SearchLocationPage), typeof(SearchLocationPage));
        }
    }
}