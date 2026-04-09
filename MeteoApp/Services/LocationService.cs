using System;
using System.Linq;
using System.Threading.Tasks;
using MeteoApp.Core.Services;
using Microsoft.Maui.Devices.Sensors;


using ModelEntry = MeteoApp.Core.Models.Entry;

namespace MeteoApp.Services
{
    public class LocationService : ILocationService
    {
        public async Task<ModelEntry> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    string cityName = placemark?.Locality ?? "Posizione Attuale";

                    return new ModelEntry
                    {
                        Id = 0,
                        CityName = $"{cityName} (GPS)",
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        IsCurrentLocation = true
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore GPS: {ex.Message}");
            }

            return null;
        }
    }
}