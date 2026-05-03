using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using static MeteoApp.Core.Services.WeatherService;

namespace MeteoApp.Core.Services
{
    public class WeatherService : IWeatherApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "a8cd7e74358a3e4179b66a36f1f419fe";

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherLocation?> GetWeatherLocationAsync(double latitude, double longitude)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={ApiKey}&units=metric";
            try
            {
                var response = await _httpClient.GetFromJsonAsync<OpenWeatherResponse>(url);
                if (response != null)
                {
                    return new WeatherLocation
                    {
                        CityName = response.Name,
                        Country = response.Sys.Country,
                        Latitude = latitude,
                        Longitude = longitude,

                        CurrentTemp = response.Main.Temp,
                        MinimumTemp = response.Main.TempMin,
                        MaximumTemp = response.Main.TempMax,
                        FeelsLike = response.Main.FeelsLike,

                        WeatherDescription = response.Weather.FirstOrDefault()?.Description ?? "Unknown",
                        WeatherMain = response.Weather.FirstOrDefault()?.Main ?? "Unknown",
                        WeatherIcon = response.Weather.FirstOrDefault()?.Icon ?? string.Empty,

                        Humidity = response.Main.Humidity,
                        Pressure = response.Main.Pressure,
                        Visibility = response.Visibility,
                        CloudCoverage = response.Clouds.All,

                        WindSpeed = response.Wind.Speed,
                        WindDegree = response.Wind.Deg,
                        WindGust = response.Wind.Gust,

                        RainLastHour = response.Rain?.OneHour ?? 0,
                        SnowLastHour = response.Snow?.OneHour ?? 0,

                        Sunrise = response.Sys.Sunrise,
                        Sunset = response.Sys.Sunset,
                        LastUpdated = response.DateTime,
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"API Request failed: {ex.Message}");
            }
            return null;
        }
        public class OpenWeatherResponse
        {
            public string Name { get; set; } = string.Empty;
            public MainData Main { get; set; } = new();
            public WeatherData[] Weather { get; set; } = Array.Empty<WeatherData>();
            public WindData Wind { get; set; } = new();
            public CloudData Clouds { get; set; } = new();
            public RainData? Rain { get; set; }
            public SnowData? Snow { get; set; }
            public SysData Sys { get; set; } = new();

            [JsonPropertyName("visibility")]
            public int Visibility { get; set; }

            [JsonPropertyName("dt")]
            public long DateTime { get; set; }
        }

        public class MainData
        {
            public float Temp { get; set; }
            public float Humidity { get; set; }
            public float Pressure { get; set; }

            [JsonPropertyName("feels_like")]
            public float FeelsLike { get; set; }

            [JsonPropertyName("temp_min")]
            public float TempMin { get; set; }

            [JsonPropertyName("temp_max")]
            public float TempMax { get; set; }
        }

        public class WindData
        {
            public float Speed { get; set; }
            public float Deg { get; set; }
            public float Gust { get; set; }
        }

        public class CloudData
        {
            public int All { get; set; } // cloudiness %
        }

        public class RainData
        {
            [JsonPropertyName("1h")]
            public float OneHour { get; set; }
        }

        public class SnowData
        {
            [JsonPropertyName("1h")]
            public float OneHour { get; set; }
        }

        public class SysData
        {
            public long Sunrise { get; set; }
            public long Sunset { get; set; }
            public string Country { get; set; } = string.Empty;
        }

        public class WeatherData
        {
            public int Id { get; set; }
            public string Main { get; set; } = string.Empty; // e.g. "Rain", "Clear"
            public string Description { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
        }
    }
}
