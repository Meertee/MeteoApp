using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Core.Platforms.iOS;

namespace MeteoApp
{
    public static class FirebaseInitializer
    {
        public static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) =>
                {
                    CrossFirebase.Initialize();
                    return false;
                }));
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            return builder;
        }
    }
}