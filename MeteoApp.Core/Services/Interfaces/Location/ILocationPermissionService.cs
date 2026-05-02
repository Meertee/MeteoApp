namespace MeteoApp.Core.Interfaces
{
    public interface ILocationPermissionService
    {
        Task<bool> CheckAndRequestPermissionAsync();
    }
}
