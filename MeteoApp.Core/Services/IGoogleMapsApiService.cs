using MeteoApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Services
{
    public interface IGoogleMapsApiService
    {
        public Task<string> GetCityNameAsync(double lat, double lon);
        public Task<List<GooglePlacePrediction>> GetSuggestionsAsync(string query);
        public Task<(double Latitude, double Longitude)?> GetCoordinatesFromAddressAsync(string address);

    }
}
