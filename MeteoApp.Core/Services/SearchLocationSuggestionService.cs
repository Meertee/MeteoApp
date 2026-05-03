using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MeteoApp.Core.Services
{
    public class SearchLocationSuggestionService : ISearchLocationSuggestionService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "AIzaSyDI83HHPHTFzm-7NITgpqVUJv8xsAG9FwU"; 

        public SearchLocationSuggestionService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<LocationSuggestion>> GetLocationSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
                return [];

            string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={Uri.EscapeDataString(query)}&types=(cities)&key={ApiKey}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<GoogleGeocodingResponse>(url);

                if (response != null && response.Status == "OK")
                {
                    return response.Results.Select(r => new LocationSuggestion
                    {
                        Name = r.FormattedAddress,
                        Description = "Città / Località"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Geocoding Error: {ex.Message}");
            }

            return [];
        }
        public class GoogleGeocodingResponse
        {
            [JsonPropertyName("predictions")]
            public List<GeocodingResult> Results { get; set; } = new();

            [JsonPropertyName("status")]
            public string Status { get; set; } = string.Empty;
        }

        public class GeocodingResult
        {
            [JsonPropertyName("description")]
            public string FormattedAddress { get; set; } = string.Empty;
        }
    }
}