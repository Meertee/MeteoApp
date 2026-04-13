using MeteoApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Services
{
    public class GpsService : IGpsService
    {
        public async Task<(double Latitude, double Longitude)?> GetDeviceCoordinatesAsync()
        {
            try
            {
               
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                    return (location.Latitude, location.Longitude);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore GPS: {ex.Message}");
            }
            return null;
        }
    }
}
