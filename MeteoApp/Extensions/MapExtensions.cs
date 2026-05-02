using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map; // Add this alias

namespace MeteoApp.Extensions
{
    public static class MapExtensions
    {
        public static void CenterAndPin(this Map map, Location location, string label)
        {
            map.Pins.Clear();
            map.Pins.Add(new Pin { Label = label, Location = location });
            map.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(5)));
        }

        public static void ShowLoadingPin(this Map map, Location location)
        {
            map.Pins.Clear();
            map.Pins.Add(new Pin { Label = "Caricamento...", Location = location });
        }
    }
}