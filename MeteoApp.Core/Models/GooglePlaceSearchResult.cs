namespace MeteoApp.Core.Models
{
    public class GooglePlaceSearchResult
    {
        public List<GooglePlacePrediction> Predictions { get; set; } = [];
        public string Status { get; set; } = string.Empty;
    }
}
