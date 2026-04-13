using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Core.Services
{
    public interface IGpsService
    {
        Task<(double Latitude, double Longitude)?> GetDeviceCoordinatesAsync();
    }
}
