using MeteoApp.Core.Models;

namespace MeteoApp.Core.Interfaces
{
    public interface ISearchLocationSuggestionService
    {
        Task<IEnumerable<LocationSuggestion>> GetLocationSuggestionsAsync(string query);
    }
}
