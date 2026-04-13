using MeteoApp.Core.Models;
using MeteoApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MeteoApp.Core.ViewModels
{
    public class MeteoMapViewModel : BaseViewModel
    {

        private readonly ILocationService _locationService;
        private readonly DatabaseService _dbService;
        private Entry _cityEntry;
        public Entry CityEntry
        {
            get => _cityEntry;
            set
            {
                _cityEntry = value;
                OnPropertyChanged();
            }
        }
        private bool _isSelectingSuggestion = false;
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();


                if (!_isSelectingSuggestion)
                {
                    _ = LoadSuggestionsAsync(value);
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

        public ObservableCollection<GooglePlacePrediction> Suggestions { get; set; } = new ObservableCollection<GooglePlacePrediction>();
        public MeteoMapViewModel(ILocationService locationService, DatabaseService dbService)
        {
            _locationService = locationService;
            _dbService = dbService;
            
        }

        private async Task LoadSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 1)
            {
                IsSuggestionsVisible = false;
                Suggestions.Clear();
                return;
            }

            List<GooglePlacePrediction> results = await _locationService.GetSuggestionsAsync(query);

            Suggestions.Clear();
            foreach (GooglePlacePrediction pred in results)
            {
                Suggestions.Add(pred);
            }

            IsSuggestionsVisible = Suggestions.Count > 0;
        }


        public async Task<(double Latitude, double Longitude)?> ProcessSuggestionSelectionAsync(GooglePlacePrediction prediction)
        {
            _isSelectingSuggestion = true;
            SearchText = prediction.Description;
            //Pulizia
            IsSuggestionsVisible = false;
            Suggestions.Clear();
            _isSelectingSuggestion = false;

            //tupla 
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
            string realCityName = await _locationService.GetCityNameFromGoogleAsync(lat, lon);
            CityEntry.Latitude = lat;
            CityEntry.Longitude = lon;
            CityEntry.CityName = realCityName;
            return realCityName;
            
        }

        public async Task<(double Latitude, double Longitude, string CityName)?> LoadCurrentLocationAsync()
        {
           
            Entry currentGpsEntry = await _locationService.GetCurrentLocationAsync();

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
