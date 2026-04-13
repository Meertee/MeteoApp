using MeteoApp.Core.Models;
using MeteoApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MeteoApp.Core.ViewModels
{
    public class CitySearchHandler(ILocationService locationService) : BaseViewModel, ISearchHandler
    {
        private readonly ILocationService _locationService = locationService;
        private bool _isSuggestionsVisible;

        public ObservableCollection<GooglePlacePrediction> Suggestions { get; } = [];

        public bool IsSuggestionsVisible
        {
            get => _isSuggestionsVisible;
            set { _isSuggestionsVisible = value; OnPropertyChanged(); }
        }

        public async Task LoadSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 1)
            {
                Clear();
                return;
            }

            var results = await _locationService.GetSuggestionsAsync(query);

            Suggestions.Clear();
            foreach (var pred in results) Suggestions.Add(pred);
            IsSuggestionsVisible = Suggestions.Count > 0;
        }

        public void Clear()
        {
            Suggestions.Clear();
            IsSuggestionsVisible = false;
        }
    }
}
