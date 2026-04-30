using MeteoApp.Core.Services;
using MeteoApp.Core.Services.MeteoApp.Core.Services;
using MeteoApp.Core.ViewModels;
using MeteoApp.Services;
using MeteoApp.Views;
using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;

[assembly: SupportedOSPlatform("ios14.0")]
[assembly: SupportedOSPlatform("maccatalyst14.0")]
[assembly: SupportedOSPlatform("android23.0")]
namespace MeteoApp
{

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            
            string weatherApiKey = "API_KEY";
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "MeteoSecure.db3");
            string dbPassword = "LinkPrime1234567";

            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Registrazione Servizi
            builder.Services.AddSingleton<DatabaseService>(new DatabaseService(dbPath, dbPassword));
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IGpsService, GpsService>();
            builder.Services.AddSingleton<IGoogleMapsApiService, GoogleMapsApiService>();
            builder.Services.AddSingleton<ILocationService, LocationService>();
            builder.Services.AddTransient<MeteoMapPage>();
            builder.Services.AddTransient<MeteoMapViewModel>();
            builder.Services.AddTransient<ISearchHandler, CitySearchHandler>();
            builder.Services.AddSingleton<IMapService, MauiMapService>();
            builder.Services.AddSingleton<WeatherService>(new WeatherService(weatherApiKey));


            // Registrazione ViewModels e Views
            builder.Services.AddTransient<MeteoListViewModel>();
            builder.Services.AddTransient<MeteoListPage>();

            builder.Services.AddTransient<MeteoItemViewModel>();
            builder.Services.AddTransient<MeteoItemPage>();

#if ANDROID || IOS
            builder.RegisterFirebaseServices();
#endif

#if ANDROID
            Task.Run(async () =>
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Tiramisu)
                {
                    await Permissions.RequestAsync<Permissions.PostNotifications>();
                }
            });
#endif

            return builder.Build();
        }
    }
}
