using MeteoApp.Core.Interfaces;

namespace MeteoApp.Services
{
    internal class NotificationService : INotificationPermissionService

    {
        public async Task<bool> CheckAndRequestPermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            return status == PermissionStatus.Granted;
        }
    }
}
