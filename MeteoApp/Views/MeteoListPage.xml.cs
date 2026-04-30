using MeteoApp.Core.Models;       
using MeteoApp.Core.ViewModels;
using ModelEntry = MeteoApp.Core.Models.WeatherLocation;

namespace MeteoApp.Views;


public partial class MeteoListPage : ContentPage
{
    public Dictionary<string, Type> Routes { get; private set; } = [];

    protected override async void OnAppearing()
    {
        base.OnAppearing();

       
        MeteoListViewModel viewModel = (MeteoListViewModel)BindingContext;

        if (viewModel != null)
        {
           
            await viewModel.LoadEntriesAsync();
        }
#if ANDROID
        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Tiramisu)
        {
            var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            System.Diagnostics.Debug.WriteLine($"Notification permission: {status}");
        }

        await Plugin.Firebase.CloudMessaging.CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await Plugin.Firebase.CloudMessaging.CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        System.Diagnostics.Debug.WriteLine($"FCM Token: {token}");
#endif
    }

    public MeteoListPage(MeteoListViewModel viewModel)
    {
        InitializeComponent();
        RegisterRoutes();

        
        BindingContext = viewModel;
    }

    private void RegisterRoutes()
    {
        Routes.Add("entrydetails", typeof(MeteoItemPage));
        Routes.Add("mappage", typeof(MeteoMapPage));

        foreach (KeyValuePair<string, Type> item in Routes)
            Routing.RegisterRoute(item.Key, item.Value);
    }

    private async void OnListSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is WeatherLocation selectedEntry)
        {
            var navigationParameter = new Dictionary<string, object>
        {
            { "WeatherLocation", selectedEntry }
        };

            await Shell.Current.GoToAsync(nameof(MeteoItemPage), navigationParameter);

            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void OnItemAdded(object sender, EventArgs e)
    {
        _ = MeteoListPage.ShowPrompt();
    }

    private static async Task ShowPrompt()
    {
        ModelEntry newEntry = new()
        {
            Id = 0,
            CityName = "Nuova Località",
            Latitude = 0, 
            Longitude = 0,
            IsCurrentLocation = false,
            Done = false
        };

       
        Dictionary<string, object> navigationParameter = new()
        {
        { "WeatherLocation", newEntry }
    };

       
        await Shell.Current.GoToAsync("mappage", navigationParameter);
    }
}