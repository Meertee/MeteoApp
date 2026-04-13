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

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                // Quando il testo cambia, avvia la ricerca (Fire & Forget)
                _ = LoadSuggestionsAsync(value);
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
        public MeteoMapViewModel(ILocationService locationService)
        {
            _locationService = locationService;
            CityEntry = new Entry();
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
            IsSuggestionsVisible = false;
            SearchText = prediction.Description;
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


    }
}
