using MeteoApp.Core.Interfaces;

namespace MeteoApp.Services
{
    public class LocationService : ICurrentLocationService, ILocationPermissionService
    {
        public async Task<bool> CheckAndRequestPermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            return status == PermissionStatus.Granted;
        }

        public async Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    return (location.Latitude, location.Longitude);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to get location: {ex.Message}");
            }
            return null;
        }
    }
}
