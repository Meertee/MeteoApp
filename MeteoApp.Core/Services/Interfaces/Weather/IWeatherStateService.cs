using MeteoApp.Core.Models;

namespace MeteoApp.Core.Interfaces
{
    public interface IWeatherStateService
    {
        WeatherLocation? CurrentLocation { get; }
        event Action? OnLocationChanged;
        void SetLocation(WeatherLocation location);
    }
}
