using MeteoApp.Core.Models;
using System.Threading.Tasks;
namespace MeteoApp.Core.Services

{
    public interface ILocationService
    {
        Task<Entry> GetCurrentLocationAsync();
        Task<string> GetCityNameFromGoogleAsync(double latitude, double longitude);

        Task<List<GooglePlacePrediction>> GetSuggestionsAsync(string query);
        Task<(double Latitude, double Longitude)?> GetCoordinatesForCityAsync(string cityName);


    }
}
