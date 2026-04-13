using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Models
{
    public class GooglePlacePrediction
    {
        public string Description { get; set; } // Es: "Milano, MI, Italia"
        public string Place_Id { get; set; }    // Id univoco di Google
    }
}
