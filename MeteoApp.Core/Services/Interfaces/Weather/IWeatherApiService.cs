using MeteoApp.Core.Models;

namespace MeteoApp.Core.Interfaces
{
    public interface IWeatherApiService
    {
        Task<WeatherLocation?> GetWeatherLocationAsync(double  latitude, double longitude);
    }
}
