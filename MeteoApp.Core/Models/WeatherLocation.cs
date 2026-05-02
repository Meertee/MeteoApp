
using SQLite;

namespace MeteoApp.Core.Models
{
    public class WeatherLocation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string CityName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsCurrentLocation { get; set; } = false;

        // Temperature
        public float CurrentTemp { get; set; }
        public float MinimumTemp { get; set; }
        public float MaximumTemp { get; set; }
        public float FeelsLike { get; set; }

        // Conditions
        public string WeatherDescription { get; set; } = string.Empty;
        public string WeatherMain { get; set; } = string.Empty;
        public string WeatherIcon { get; set; } = string.Empty;

        // Atmosphere
        public float Humidity { get; set; }
        public float Pressure { get; set; }
        public int Visibility { get; set; }
        public int CloudCoverage { get; set; }

        // Wind
        public float WindSpeed { get; set; }
        public float WindDegree { get; set; }
        public float WindGust { get; set; }

        // Precipitation
        public float RainLastHour { get; set; }
        public float SnowLastHour { get; set; }

        // Sun
        public long Sunrise { get; set; }
        public long Sunset { get; set; }

        // Meta
        public long LastUpdated { get; set; }
    }
}
