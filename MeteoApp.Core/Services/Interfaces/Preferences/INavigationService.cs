namespace MeteoApp.Core.Interfaces
{
    public interface INavigationService
    {
     
        Task NavigateToAsync(string route);

        Task GoBackAsync();
    }
}