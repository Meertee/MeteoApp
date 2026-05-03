using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;
using MeteoApp.Core.Services.Interfaces.AppWrite;
using System.Collections.ObjectModel;

namespace MeteoApp.Core.ViewModels
{
    public partial class MainViewModel(
        ILocationManager locationManager,
        IDatabaseService dbService,
        ILocationPermissionService locationPermissionService,
        ICurrentLocationService currentLocationService,
        IWeatherApiService weatherApiService,
        INotificationPermissionService notificationPermissionService) : ObservableObject
    {
        private bool _isInitialized = false;
        public ObservableCollection<WeatherLocation> Locations { get; } = [];

        public async Task InitializeAsync()
        {
            if (!_isInitialized)
            {
                await notificationPermissionService.CheckAndRequestPermissionAsync();
                await FetchCurrentLocation();
                _isInitialized = true;
            }

            await LoadLocationsAsync();
        }

        [RelayCommand]
        public async Task LoadLocationsAsync()
        {
            List<WeatherLocation> savedLocations = await dbService.GetAllLocationsAsync();
            Locations.Clear();
            foreach (WeatherLocation location in savedLocations)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Loaded: Id={location.Id}, City={location.CityName}");
                Locations.Add(location);
            }
        }

        private async Task FetchCurrentLocation()
        {
            bool hasPermission = await locationPermissionService.CheckAndRequestPermissionAsync();
            if (hasPermission)
            {
                var coordinates = await currentLocationService.GetCurrentLocationAsync();
                if (coordinates != null)
                {
                    double lat = coordinates.Value.Latitude;
                    double lon = coordinates.Value.Longitude;
                    System.Diagnostics.Debug.WriteLine($"SUCCESS: Fetched coordinates Lat: {lat}, Lon: {lon}");
                    WeatherLocation? currentLocation = await weatherApiService.GetWeatherLocationAsync(lat, lon);
                    if (currentLocation != null)
                    {
                        currentLocation.IsCurrentLocation = true;
                        await locationManager.SaveLocationAsync(currentLocation);
                    }
                }
            }
        }
    }
}