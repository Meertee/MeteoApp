using Microsoft.Extensions.Logging;
using MeteoApp.Core.Services;
using MeteoApp.Core.ViewModels;
using MeteoApp.Services;
using MeteoApp.Views;
using System.IO;

namespace MeteoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

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
            builder.Services.AddSingleton<ILocationService, LocationService>();
            builder.Services.AddTransient<MeteoMapPage>();
            builder.Services.AddTransient<MeteoMapViewModel>();


            // Registrazione ViewModels e Views
            builder.Services.AddTransient<MeteoListViewModel>();
            builder.Services.AddTransient<MeteoListPage>();

            return builder.Build();
        }
    }
}
