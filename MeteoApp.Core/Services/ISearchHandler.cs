using MeteoApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MeteoApp.Core.Services
{
    public interface ISearchHandler
    {
        ObservableCollection<GooglePlacePrediction> Suggestions { get; }
        bool IsSuggestionsVisible { get; set; }
        Task LoadSuggestionsAsync(string query);
        void Clear();
    }
}
