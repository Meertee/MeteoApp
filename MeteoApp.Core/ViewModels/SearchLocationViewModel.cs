using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;
using MeteoApp.Core.Services.Interfaces.AppWrite;
using System.Collections.ObjectModel;

namespace MeteoApp.Core.ViewModels
{
    public partial class SearchLocationViewModel(
        ILocationManager locationManager,
        IDatabaseService dbService,
        IWeatherApiService weatherApiService,
        ISearchLocationSuggestionService searchSuggestionsService) : ObservableObject
    {

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isSuggestionsVisible;

        public ObservableCollection<LocationSuggestion> Suggestions { get; } = new();

        public async Task LoadSuggestionsAsync(string query)
        {
            var results = await searchSuggestionsService.GetLocationSuggestionsAsync(query);

            Suggestions.Clear();

            if (results != null && results.Any())
            {
                foreach (var suggestion in results)
                {
                    Suggestions.Add(suggestion);
                }
                IsSuggestionsVisible = true;
            }
            else
            {
                IsSuggestionsVisible = false;
            }
        }

        [RelayCommand]
        public async Task SaveLocationAsync(Tuple<double, double> coordinates)
        {
            double latitude = coordinates.Item1;
            double longitude = coordinates.Item2;

            WeatherLocation? location = await weatherApiService.GetWeatherLocationAsync(latitude, longitude);

            if (location != null)
            {
                var savedLocations = await dbService.GetAllLocationsAsync();

                var existing = savedLocations.FirstOrDefault(l =>
                    string.Equals(l.CityName?.Trim(), location.CityName?.Trim(), StringComparison.OrdinalIgnoreCase));
                
                location.IsCurrentLocation = existing?.IsCurrentLocation ?? false;
                await locationManager.SaveLocationAsync(location); ;
            }
        }
    }
}
