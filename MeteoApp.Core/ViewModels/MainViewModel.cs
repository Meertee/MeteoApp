using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;
using MeteoApp.Core.Services.Interfaces.AppWrite;
using MeteoApp.Core.Services.Interfaces.Preferences;
using System.Collections.ObjectModel;

namespace MeteoApp.Core.ViewModels
{
    public partial class MainViewModel(
        INavigationService navigationService,
        ILocationManager locationManager,
        IDatabaseService dbService,
        ILocationPermissionService locationPermissionService,
        ICurrentLocationService currentLocationService,
        IWeatherApiService weatherApiService,
        ISettingsService settingsService,
        INotificationPermissionService notificationPermissionService) : ObservableObject
    {
        private bool _isInitialized = false;
        public ObservableCollection<WeatherLocation> Locations { get; } = [];

     

        [ObservableProperty]
        private string _temperatureUnit = string.Empty;

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
            // 2. Ogni volta che carichi la lista, aggiorniamo il simbolo leggendo le impostazioni
            TemperatureUnit = settingsService.GetTemperatureUnitString();

            List<WeatherLocation> savedLocations = await dbService.GetAllLocationsAsync();
            Locations.Clear();
            foreach (WeatherLocation location in savedLocations)
            {
                Locations.Add(location);
            }
        }

        [RelayCommand]
        public async Task GoToSearchAsync()
        {
           
            await navigationService.NavigateToAsync("SearchLocationPage");
        }

        [RelayCommand]
        public async Task GoToSettingsAsync()
        {
           
            await navigationService.NavigateToAsync("SettingsPage");
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