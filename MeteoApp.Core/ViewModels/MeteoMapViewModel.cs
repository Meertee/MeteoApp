using MeteoApp.Core.Models;
using MeteoApp.Core.Services;
using System.Collections.ObjectModel;

namespace MeteoApp.Core.ViewModels
{
    public class MeteoMapViewModel(ILocationService locationService, DatabaseService dbService, ISearchHandler searchHandler) : LoadingViewModel
    {

        private readonly ILocationService _locationService = locationService;
        private readonly DatabaseService _dbService = dbService;
        public ISearchHandler SearchManager { get; } = searchHandler;
        private WeatherLocation _cityEntry = new();
        public WeatherLocation CityEntry
        {
            get => _cityEntry;
            set
            {
                _cityEntry = value;
                OnPropertyChanged();
                
            }
        }
        private bool _isSelectingSuggestion = false;
        private string _searchText=string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();


                if (!_isSelectingSuggestion)
                {
                    
                    _ = SearchManager.LoadSuggestionsAsync(value);
                }
            }
        }

        private bool _isSuggestionsVisible;
        public bool IsSuggestionsVisible
        {
            get => _isSuggestionsVisible;
            set
            {
                _isSuggestionsVisible = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GooglePlacePrediction> Suggestions { get; set; } = [];


        public async Task<(double Latitude, double Longitude)?> ProcessSuggestionSelectionAsync(GooglePlacePrediction prediction)
        {
            _isSelectingSuggestion = true;
            SearchText = prediction.Description;
            SearchManager.Clear();
            _isSelectingSuggestion = false;
     

            (double Latitude, double Longitude)? coords = await _locationService.GetCoordinatesForCityAsync(prediction.Description);

            if (coords.HasValue)
            {
                CityEntry.Latitude = coords.Value.Latitude;
                CityEntry.Longitude = coords.Value.Longitude;
                CityEntry.CityName = prediction.Description;
            }
            return coords;
        }

        public async Task<string> ProcessMapClickAsync(double lat, double lon)
        {
            string realCityName = await _locationService.GetCityNameAsync(lat, lon);
            CityEntry.Latitude = lat;
            CityEntry.Longitude = lon;
            CityEntry.CityName = realCityName;
            return realCityName;
            
        }

        public async Task<(double Latitude, double Longitude, string CityName)?> LoadCurrentLocationAsync()
        {
           
            WeatherLocation currentGpsEntry = await _locationService.GetCurrentLocationAsync();

            if (currentGpsEntry != null)
            {
             
                CityEntry.Latitude = currentGpsEntry.Latitude;
                CityEntry.Longitude = currentGpsEntry.Longitude;
                CityEntry.CityName = currentGpsEntry.CityName;

                return (currentGpsEntry.Latitude, currentGpsEntry.Longitude, currentGpsEntry.CityName);
            }

            return null;
        }

        public async Task SaveCityAsync()
        {
            if (CityEntry != null && !string.IsNullOrWhiteSpace(CityEntry.CityName))
            {
                await _dbService.SaveEntryAsync(CityEntry);
            }
        }


    }
}
