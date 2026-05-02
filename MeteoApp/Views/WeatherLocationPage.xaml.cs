using MeteoApp.Core.ViewModels;

namespace MeteoApp.Views;

public partial class WeatherLocationPage : ContentPage, IQueryAttributable
{
    public WeatherLocationPage(WeatherLocationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        System.Diagnostics.Debug.WriteLine($"[DEBUG] ApplyQueryAttributes called, keys={string.Join(",", query.Keys)}");
        if (query.TryGetValue("LocationId", out var idParam) && int.TryParse(idParam?.ToString(), out int id))
        {
            System.Diagnostics.Debug.WriteLine($"[DEBUG] Parsed LocationId={id}");
            if (BindingContext is WeatherLocationViewModel viewModel)
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Setting LocationId on VM");
                viewModel.LocationId = id; // triggers OnLocationIdChanged → LoadLocationAsync
            }
        }
    }
}