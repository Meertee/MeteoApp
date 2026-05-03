using MeteoApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Services.Interfaces.AppWrite
{
    public interface IAppwriteService
    {
        Task SaveLocationAsync(WeatherLocation location);
        Task DeleteLocationAsync(WeatherLocation location);
        Task<List<WeatherLocation>> GetAllLocationsAsync();
    }
}
