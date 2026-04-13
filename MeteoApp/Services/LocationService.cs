using MeteoApp.Core.Models;
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

        public async Task<(double Latitude, double Longitude)?> GetCoordinatesForCityAsync(string cityName)
        {
            try
            {
             
                IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(cityName);
                Location? location = Enumerable.FirstOrDefault(locations);

                if (location != null)
                {
                    // Restituiamo solo i numeri puri al Core
                    return (location.Latitude, location.Longitude);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore Geocoding: {ex.Message}");
            }
            return null;
        }


        private static async Task<Location> GetDeviceCoordinatesAsync()
        {
            Location defaultLocation = new(41.9028, 12.4964);
            try
            {
                GeolocationRequest request = new (GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                Location? location = await Geolocation.Default.GetLocationAsync(request);
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
        public async Task<string> GetCityNameFromGoogleAsync(double latitude, double longitude)
        {
            string defaultCityName = "Posizione Attuale";
            try
            {
                string lat = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string lon = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

                string url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lon}&language=it&key={GoogleMapsApiKey}";

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
                                        return component.GetProperty("long_name").GetString() ?? string.Empty;
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

        public async Task<List<GooglePlacePrediction>> GetSuggestionsAsync(string query)
        {
            List<GooglePlacePrediction> suggestionsList = [];
            try
            {
                string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={query}&types=(cities)&language=it&key={GoogleMapsApiKey}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;
                   
                    if (root.GetProperty("status").GetString() == "OK")
                    {
                       
                        JsonElement predictionsArray = root.GetProperty("predictions");

                      
                        foreach (JsonElement item in predictionsArray.EnumerateArray())
                        {
                            
                            GooglePlacePrediction prediction = new()
                            {
                                Description = item.GetProperty("description").GetString() ?? string.Empty,
                                Place_Id = item.GetProperty("place_id").GetString() ?? string.Empty
                            };

                            suggestionsList.Add(prediction);
                        }
                    }


                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Errore chiamando Google Places: {ex.Message}");
            }

            return suggestionsList;

        }
        public async Task<ModelEntry> GetCurrentLocationAsync()
        {

            ModelEntry defaultEntry = new ()
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
                Location location = await LocationService.GetDeviceCoordinatesAsync();
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