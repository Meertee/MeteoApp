// MeteoApp.Core/Services/LocationManager.cs
using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;
using MeteoApp.Core.Services.Interfaces.AppWrite;

namespace MeteoApp.Core.Services
{
    public class LocationManager : ILocationManager
    {
        private readonly IDatabaseService _localDb;
        private readonly IAppwriteService _cloudDb;

        public LocationManager(IDatabaseService localDb, IAppwriteService cloudDb)
        {
            _localDb = localDb;
            _cloudDb = cloudDb;
        }

        public async Task SaveLocationAsync(WeatherLocation location)
        {
            
            await _localDb.SaveLocationAsync(location);

     
            await _cloudDb.SaveLocationAsync(location);
        }

        public async Task DeleteLocationAsync(WeatherLocation location)
        {
            // 1. Elimina in locale
            await _localDb.DeleteLocationAsync(location);

            // 2. Elimina dal cloud
            await _cloudDb.DeleteLocationAsync(location);
        }
    }
}