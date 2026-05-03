using Appwrite;
using Appwrite.Services;
using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;
using MeteoApp.Core.Services.Interfaces.AppWrite;
using System.Text.Json;

namespace MeteoApp.Core.Services
{
    public class AppwriteService : IAppwriteService
    {
        private readonly Client _client;
        private readonly Databases _databases;

        //AppWritw
        //Appwrite Funziona ma si devono mettere le impostazioni giuste tolte per il commit 
        private const string Endpoint = "https://fra.cloud.appwrite.io/v1";
        private const string ProjectId = "";
        private const string DatabaseId = "";
        private const string CollectionId = "";

        public AppwriteService()
        {
            _client = new Client()
                .SetEndpoint(Endpoint)
                .SetProject(ProjectId);

            _databases = new Databases(_client);
        }

        public async Task SaveLocationAsync(WeatherLocation location)
        {
            try
            {
        
                var data = new Dictionary<string, object>
                {
                  
                
                    { "CityName", location.CityName },
                    { "Country", location.Country },
                    { "Latitude", location.Latitude },
                    { "Longitude", location.Longitude },
                    { "IsCurrentLocation", location.IsCurrentLocation },
                    
                    // Temperature
                    { "CurrentTemp", location.CurrentTemp },
                    { "MinimumTemp", location.MinimumTemp },
                    { "MaximumTemp", location.MaximumTemp },
                    { "FeelsLike", location.FeelsLike },

                    // Conditions
                    { "WeatherDescription", location.WeatherDescription },
                    { "WeatherMain", location.WeatherMain },
                    { "WeatherIcon", location.WeatherIcon },

                    // Atmosphere
                    { "Humidity", location.Humidity },
                    { "Pressure", location.Pressure },
                    { "Visibility", location.Visibility },
                    { "CloudCoverage", location.CloudCoverage },

                    // Wind
                    { "WindSpeed", location.WindSpeed },
                    { "WindDegree", location.WindDegree },
                    { "WindGust", location.WindGust },

                    // Precipitation
                    { "RainLastHour", location.RainLastHour },
                    { "SnowLastHour", location.SnowLastHour },

                    // Sun & Meta
                    { "Sunrise", location.Sunrise },
                    { "Sunset", location.Sunset },
                    { "LastUpdated", location.LastUpdated }
                };

                string documentId = string.IsNullOrEmpty(location.CityName)
                    ? ID.Unique()
                    : location.CityName.Replace(" ", "_").ToLower();

                try
                {
                    await _databases.GetDocument(DatabaseId, CollectionId, documentId);
                    // Se esiste, aggiorna
                    await _databases.UpdateDocument(DatabaseId, CollectionId, documentId, data);
                }
                catch (AppwriteException ex) when (ex.Code == 404)
                {
                    // Se non esiste (404), crea
                    await _databases.CreateDocument(DatabaseId, CollectionId, documentId, data);
                    System.Diagnostics.Debug.WriteLine($"Errore Appwrite: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore Appwrite: {ex.Message}");
            }
        }

        public async Task DeleteLocationAsync(WeatherLocation location)
        {
            try
            {
                string documentId = location.CityName.Replace(" ", "_").ToLower();
                await _databases.DeleteDocument(DatabaseId, CollectionId, documentId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore cancellazione Appwrite: {ex.Message}");
            }
        }

        public async Task<List<WeatherLocation>> GetAllLocationsAsync()
        {
            return new List<WeatherLocation>();
        }
    }
}