using CommunityToolkit.Mvvm.ComponentModel;
using MeteoApp.Core.Models;

namespace MeteoApp.Core.ViewModels
{
    public partial class MeteoItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private WeatherLocation _detailEntry;

        // Create a simple method to set the entry
        public void Initialize(WeatherLocation entry)
        {
            DetailEntry = entry;
        }
    }
}