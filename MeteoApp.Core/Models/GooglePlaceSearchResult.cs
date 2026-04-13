using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Models
{
    public class GooglePlaceSearchResult
    {
        public System.Collections.Generic.List<GooglePlacePrediction> Predictions { get; set; }
        public string Status { get; set; }
    }
}
