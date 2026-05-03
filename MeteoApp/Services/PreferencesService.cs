using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Services.Interfaces.Preferences;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace MeteoApp.Services
{
    public class PreferencesService : ISettingsService
    {
        public bool UseFahrenheit
        {
            get => Preferences.Default.Get(nameof(UseFahrenheit), false);
            set => Preferences.Default.Set(nameof(UseFahrenheit), value);
        }

        public string GetTemperatureUnitString()
        {
            return UseFahrenheit ? "°F" : "°C";
        }
    }
}
