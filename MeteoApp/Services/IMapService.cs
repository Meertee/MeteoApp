using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Services
{
    public interface IMapService
    {
        void CenterAndPin(Microsoft.Maui.Controls.Maps.Map map, Location location, string label);
        void ShowLoadingPin(Microsoft.Maui.Controls.Maps.Map map, Location location);
    }
}
