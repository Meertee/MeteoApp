using System.Net.Http.Json;
using MeteoApp.Core.Models;

namespace MeteoApp.Core.Services
{
    public class WeatherService(string apiKey)
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _apiKey = apiKey;
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public async Task<float?> GetTemperatureAsync(double lat, double lon)
        {
            try
            {
                string url = $"{BaseUrl}?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";

                var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(url);
                return response?.Main.Temp;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"---> WEATHER ERROR: {ex.Message}");
                return null;
            }
        }

        public async Task RefreshEntryWeatherAsync(WeatherLocation entry, DatabaseService dbService)
        {
            var temp = await GetTemperatureAsync(entry.Latitude, entry.Longitude);

            if (temp.HasValue)
            {
                entry.CurrentTemp = temp.Value;
                await dbService.SaveEntryAsync(entry);
            }
        }
    }
}