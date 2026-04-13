using MeteoApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Services
{
    namespace MeteoApp.Core.Services
    {
        public class LocationService(IGpsService gpsService, IGoogleMapsApiService googleApi) : ILocationService
        {
            public async Task<Entry> GetCurrentLocationAsync()
            {
               
                var coords = await gpsService.GetDeviceCoordinatesAsync();

          
                double lat = coords?.Latitude ?? 41.9028;
                double lon = coords?.Longitude ?? 12.4964;

           
                string cityName = await googleApi.GetCityNameAsync(lat, lon);

                return new Entry
                {
                    CityName = cityName,
                    Latitude = lat,
                    Longitude = lon,
                    IsCurrentLocation = true
                };
            }

            public Task<List<GooglePlacePrediction>> GetSuggestionsAsync(string query)
                => googleApi.GetSuggestionsAsync(query);

            public Task<(double Latitude, double Longitude)?> GetCoordinatesForCityAsync(string cityName)
                => googleApi.GetCoordinatesFromAddressAsync(cityName);

            public Task<string> GetCityNameAsync(double latitude, double longitude) => googleApi.GetCityNameAsync(latitude, longitude);
            
        }
    }
}
