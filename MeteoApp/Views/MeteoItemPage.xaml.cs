using MeteoApp.Core.Models;
using MeteoApp.Core.Services;

namespace MeteoApp;

[QueryProperty(nameof(PassedEntry), "WeatherLocation")]
public partial class MeteoItemPage : ContentPage
{
    private readonly WeatherService _weatherService;
    private readonly DatabaseService _dbService;

    private WeatherLocation? _passedEntry;
    public WeatherLocation? PassedEntry
    {
        get => _passedEntry;
        set
        {
            _passedEntry = value;
            BindingContext = _passedEntry;
            OnPropertyChanged();
        }
    }

    public MeteoItemPage(WeatherService weatherService, DatabaseService dbService)
    {
        InitializeComponent();
        _weatherService = weatherService;
        _dbService = dbService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (PassedEntry != null)
        {
            // Fetch latest weather and save to DB
            await _weatherService.RefreshEntryWeatherAsync(PassedEntry, _dbService);

            // Refresh the BindingContext to show the new temperature
            BindingContext = null;
            BindingContext = PassedEntry;
        }
    }
}