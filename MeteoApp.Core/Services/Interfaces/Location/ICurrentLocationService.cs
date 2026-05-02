namespace MeteoApp.Core.Interfaces
{
    public interface ICurrentLocationService
    {
        Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync();
    }
}
