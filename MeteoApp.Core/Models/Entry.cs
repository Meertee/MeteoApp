
namespace MeteoApp.Core.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsCurrentLocation { get; set; }

       
        public bool Done { get; set; }
    }
}
