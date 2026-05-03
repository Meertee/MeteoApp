// MeteoApp/Services/NavigationService.cs
using MeteoApp.Core.Interfaces;

namespace MeteoApp.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToAsync(string route)
        {
          
            await Shell.Current.GoToAsync(route);
        }

        public async Task GoBackAsync()
        {
          
            await Shell.Current.GoToAsync("..");
        }
    }
}