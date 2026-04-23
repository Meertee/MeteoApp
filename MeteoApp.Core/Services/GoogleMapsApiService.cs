using MeteoApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MeteoApp.Core.Services
{
    public class GoogleMapsApiService(HttpClient httpClient) : IGoogleMapsApiService
    {
        private const string ApiKey = "API_KEY";



        async Task<string> IGoogleMapsApiService.GetCityNameAsync(double lat, double lon)
        {
            try
            {
                string latStr = lat.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string lonStr = lon.ToString(System.Globalization.CultureInfo.InvariantCulture);
                string url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latStr},{lonStr}&language=it&key={ApiKey}";

                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;

                    if (root.GetProperty("status").GetString() == "OK")
                    {
                        JsonElement results = root.GetProperty("results");
                        if (results.GetArrayLength() > 0)
                        {
                            JsonElement addressComponents = results[0].GetProperty("address_components");
                            foreach (JsonElement component in addressComponents.EnumerateArray())
                            {
                                JsonElement types = component.GetProperty("types");
                                if (types.EnumerateArray().Any(t => t.GetString() == "locality"))
                                {
                                    return component.GetProperty("long_name").GetString() ?? "Città sconosciuta";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Errore chiamata API Google: {ex.Message}"); }
            return "Posizione Attuale";
        }

        public async Task<(double Latitude, double Longitude)?> GetCoordinatesFromAddressAsync(string address)
        {
            try
            {
                string encodedAddress = Uri.EscapeDataString(address);
                string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={ApiKey}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;

                    if (root.GetProperty("status").GetString() == "OK")
                    {

                        JsonElement location = root.GetProperty("results")[0]
                                           .GetProperty("geometry")
                                           .GetProperty("location");

                        double lat = location.GetProperty("lat").GetDouble();
                        double lng = location.GetProperty("lng").GetDouble();

                        return (lat, lng);
                    }
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Errore Geocoding Google: {ex.Message}"); }
            return null;
        }

        public async Task<List<GooglePlacePrediction>> GetSuggestionsAsync(string query)
        {
            List<GooglePlacePrediction> suggestionsList = [];
            try
            {
                string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={query}&types=(cities)&language=it&key={ApiKey}";
                HttpResponseMessage response = await httpClient.GetAsync(url);
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
    }
}
