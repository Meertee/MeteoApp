using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Services.Interfaces.Preferences
{
    public interface ISettingsService
    {
        
        bool UseFahrenheit { get; set; }

        string GetTemperatureUnitString();
    }
}
