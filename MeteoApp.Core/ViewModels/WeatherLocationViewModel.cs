using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;

namespace MeteoApp.Core.ViewModels
{
    public partial class WeatherLocationViewModel(
        IDatabaseService dbService,
        IWeatherStateService weatherStateService) : ObservableObject
    {
        [ObservableProperty]
        private int _locationId;

        [ObservableProperty]
        private string _cityName = string.Empty;

        partial void OnLocationIdChanged(int value)
        {
            if (value != 0)
                _ = LoadLocationAsync(value);
        }

        [RelayCommand]
        private async Task LoadLocationAsync(int id)
        {
            WeatherLocation? location = await dbService.GetLocationAsync(id);
            if (location != null)
            {
                CityName = location.CityName;
                weatherStateService.SetLocation(location);
            }
        }
    }
}
