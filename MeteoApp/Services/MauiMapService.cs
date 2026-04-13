using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Services
{
    public class MauiMapService : IMapService
    {
        public void CenterAndPin(Microsoft.Maui.Controls.Maps.Map map, Location location, string label)
        {
            map.Pins.Clear();
            var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(2));
            map.MoveToRegion(mapSpan);
            map.Pins.Add(new Pin { Label = label, Location = location, Type = PinType.Place });
        }

        public void ShowLoadingPin(Microsoft.Maui.Controls.Maps.Map map, Location location)
        {
            map.Pins.Clear();
            map.Pins.Add(new Pin { Label = "Ricerca nome...", Location = location });
        }
    }
}
