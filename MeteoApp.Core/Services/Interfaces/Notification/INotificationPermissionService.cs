namespace MeteoApp.Core.Interfaces
{
    public interface INotificationPermissionService
    {
        Task<bool> CheckAndRequestPermissionAsync();
    }
}
