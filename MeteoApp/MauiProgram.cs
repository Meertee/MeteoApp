using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Services;
using MeteoApp.Core.Services.Interfaces.AppWrite;
using MeteoApp.Core.Services.Interfaces.Preferences;
using MeteoApp.Core.ViewModels;
using MeteoApp.Services;
using MeteoApp.Views;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;
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
#pragma warning disable CA1416 // Validate platform compatibility
            builder.Services.AddMauiBlazorWebView();
#pragma warning restore CA1416 // Validate platform compatibility
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            // Registrazione Servizi
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<ICurrentLocationService, LocationService>();
            builder.Services.AddSingleton<ILocationPermissionService, LocationService>();
            builder.Services.AddSingleton<IWeatherApiService, WeatherService>();
            builder.Services.AddSingleton<IWeatherStateService,WeatherStateService>();
            builder.Services.AddSingleton<ISearchLocationSuggestionService, SearchLocationSuggestionService>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<INotificationPermissionService, NotificationService>();
            builder.Services.AddSingleton<IAppwriteService, AppwriteService>();
            builder.Services.AddSingleton<ILocationManager, LocationManager>();
            builder.Services.AddSingleton<ISettingsService, PreferencesService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            // Registrazione ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<WeatherLocationViewModel>();
            builder.Services.AddTransient<SearchLocationViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();

            // Registrazione Views
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<WeatherLocationPage>();
            builder.Services.AddTransient<SearchLocationPage>();
            builder.Services.AddTransient<SettingsPage>();

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
