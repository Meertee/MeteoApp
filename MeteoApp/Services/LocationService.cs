using MeteoApp.Core.Services;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using ModelEntry = MeteoApp.Core.Models.Entry;

namespace MeteoApp.Services
{
    public class LocationService : ILocationService
    {

        private const string GoogleMapsApiKey = "AIzaSyBnllCmyOdSttoFZQbO_2NALMKGtPcUxMo";
        // per fare richieste API a google
        private readonly HttpClient _httpClient;

        public LocationService()
        {
            //Meglio rispetto a usare using
            this._httpClient = new HttpClient();
        }


        private async Task<Location> GetDeviceCoordinatesAsync()
        {
            Location defaultLocation = new Location(41.9028, 12.4964);
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                Location location = await Geolocation.Default.GetLocationAsync(request);
                if (location == null)
                {
                    Console.WriteLine("GPS non ha trovato la posizione, uso il valore di default.");
                    return defaultLocation;
                }
                return location;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore lettura sensore GPS: {ex.Message}");
                return defaultLocation;
            }
        }
        private async Task<string> GetCityNameFromGoogleAsync(double latitude, double longitude)
        {
            string defaultCityName = "Posizione Attuale";
            try
            {
                string lat = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string lon = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

                string url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lon}&key={GoogleMapsApiKey}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(json); // navigabile con component
                    JsonElement root = doc.RootElement; //prende elemento principale

                    if (root.GetProperty("status").GetString() == "OK")
                    {
                        JsonElement results = root.GetProperty("results");
                        if (results.GetArrayLength()>0)
                        {
                            JsonElement addressComponents = results[0].GetProperty("address_components");
                            foreach (JsonElement component in addressComponents.EnumerateArray())
                            {
                                JsonElement types = component.GetProperty("types");
                                foreach (JsonElement type in types.EnumerateArray())
                                {
                                    // Se trova la città, la restituisce subito e ferma la ricerca
                                    if (type.GetString() == "locality")
                                    {
                                        return component.GetProperty("long_name").GetString();
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore chiamata API Google: {ex.Message}");
            }
            return defaultCityName;
        }
        public async Task<ModelEntry> GetCurrentLocationAsync()
        {

            ModelEntry defaultEntry = new ModelEntry
            {
                Id = 0,
                CityName = "(Posizione non disponibile)",
                Latitude = 41.9028,
                Longitude = 12.4964,
                IsCurrentLocation = true,
                Done = false
            };

            try
            {
                Location location = await GetDeviceCoordinatesAsync();
                if (location != null)
                {
                    string cityName = await GetCityNameFromGoogleAsync(location.Latitude, location.Longitude);
                    return new ModelEntry
                    {
                        Id = 0,
                        CityName = $"{cityName}",
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        IsCurrentLocation = true
                        
                    };
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Errore generale: {ex.Message}");
            }
            return defaultEntry;
        }

        
    }
}