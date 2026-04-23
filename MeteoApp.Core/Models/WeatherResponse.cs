namespace MeteoApp.Core.Models
{
    public class WeatherResponse
    {
        public MainData Main { get; set; } = new();
    }

    public class MainData
    {
        public float Temp { get; set; }
    }
}
