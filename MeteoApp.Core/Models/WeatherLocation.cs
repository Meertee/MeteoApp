
using SQLite;

namespace MeteoApp.Core.Models
{
    public class WeatherLocation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsCurrentLocation { get; set; }
        public float CurrentTemp {  get; set; }
        public bool Done { get; set; }
    }
}
