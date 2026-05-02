using MeteoApp.Core.Models;

namespace MeteoApp.Core.Interfaces
{
    public interface IDatabaseService
    {
        Task InitializeAsync();
        Task<List<WeatherLocation>> GetAllLocationsAsync();
        Task<WeatherLocation> GetCurrentLocationAsync();
        Task<WeatherLocation> GetLocationAsync(int id);
        Task<int> SaveLocationAsync(WeatherLocation location);
        Task<int> DeleteLocationAsync(WeatherLocation location);
    }
}
