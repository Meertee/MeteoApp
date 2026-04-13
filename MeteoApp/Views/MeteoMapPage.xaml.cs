
using MeteoApp.Core.Models;
using MeteoApp.Core.Services;
using MeteoApp.Core.ViewModels;
using MeteoApp.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Runtime.Versioning;
using ModelEntry = MeteoApp.Core.Models.Entry;

namespace MeteoApp.Views;
// il pachetto di prima
[QueryProperty(nameof(PassedEntry), "Entry")]
         
public partial class MeteoMapPage : ContentPage
{
    private readonly MeteoMapViewModel _viewModel;

    public ModelEntry PassedEntry
    {
        get => _viewModel.CityEntry;
        set
        {
            _viewModel.CityEntry = value;
            CenterMapOnEntry();
        }
    }

    public MeteoMapPage(MeteoMapViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void CenterMapOnEntry()
    {
        if (_viewModel.CityEntry != null && _viewModel.CityEntry.Latitude != 0)
        {
            Location location = new (_viewModel.CityEntry.Latitude, _viewModel.CityEntry.Longitude);
            MoveMapToLocation(location, _viewModel.CityEntry.CityName);
        }
    }

    private void MoveMapToLocation(Location location, string label)
    {
        myMap.Pins.Clear();
        MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(2));
        myMap.MoveToRegion(mapSpan);

        Pin pin = new() { Label = label, Location = location, Type = PinType.Place };
        myMap.Pins.Add(pin);
    }

    private async void OnSuggestionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is GooglePlacePrediction prediction)
        {

            (double Latitude, double Longitude)? coords = await _viewModel.ProcessSuggestionSelectionAsync(prediction);
            if (coords.HasValue)
            {

                Location location = new(coords.Value.Latitude, coords.Value.Longitude);


                MoveMapToLocation(location, prediction.Description);
            }
        }
        ((CollectionView)sender).SelectedItem = null; 
    }

    private async void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        myMap.Pins.Clear();
        Pin loadingPin = new () { Label = "Ricerca nome...", Location = e.Location };
        myMap.Pins.Add(loadingPin);

        // Delega la logica API al ViewModel
        string realCityName = await _viewModel.ProcessMapClickAsync(e.Location.Latitude, e.Location.Longitude);
        myMap.Pins.Clear();
        Pin finalPin = new()
        {
            Label = realCityName,
            Address = $"Lat: {e.Location.Latitude:F3}, Lon: {e.Location.Longitude:F3}",
            Location = e.Location,
            Type = PinType.Place
        };

        myMap.Pins.Add(finalPin);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {

        await _viewModel.SaveCityAsync();
        await DisplayAlertAsync("Successo", $"{_viewModel.CityEntry.CityName} salvata correttamente!", "OK");
        await Shell.Current.GoToAsync("..");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.CityEntry.Latitude == 0)
        {
           
            (double Latitude, double Longitude, string CityName)? result = await _viewModel.LoadCurrentLocationAsync();

            if (result.HasValue)
            {
               
                Location location = new (result.Value.Latitude, result.Value.Longitude);
                MoveMapToLocation(location, result.Value.CityName);
            }
        }
        else
        {
            
            CenterMapOnEntry();
        }
    }


}



