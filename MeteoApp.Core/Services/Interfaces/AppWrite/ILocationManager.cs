using MeteoApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Services.Interfaces.AppWrite
{
    public interface ILocationManager
    {
        Task SaveLocationAsync(WeatherLocation location);
        Task DeleteLocationAsync(WeatherLocation location);
    }
}
