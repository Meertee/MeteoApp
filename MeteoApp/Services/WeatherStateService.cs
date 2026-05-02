using MeteoApp.Core.Models;
using MeteoApp.Core.Interfaces;

namespace MeteoApp.Services
{
    public class WeatherStateService : IWeatherStateService
    {
        public WeatherLocation? CurrentLocation { get; private set; }
        public event Action? OnLocationChanged;

        public void SetLocation(WeatherLocation location)
        {
            CurrentLocation = location;
            OnLocationChanged?.Invoke();
        }
    }
}