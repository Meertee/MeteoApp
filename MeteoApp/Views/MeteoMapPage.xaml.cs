
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
    private readonly IMapService _mapService;

    public ModelEntry PassedEntry
    {
        get => _viewModel.CityEntry;
        set => _viewModel.CityEntry = value;
    }

    public MeteoMapPage(MeteoMapViewModel viewModel, IMapService mapService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _mapService = mapService;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

    
        if (_viewModel.CityEntry.Latitude == 0)
        {
            var result = await _viewModel.LoadCurrentLocationAsync();
            if (result.HasValue)
                _mapService.CenterAndPin(myMap, new Location(result.Value.Latitude, result.Value.Longitude), result.Value.CityName);
        }
        else
        {
            _mapService.CenterAndPin(myMap, new Location(_viewModel.CityEntry.Latitude, _viewModel.CityEntry.Longitude), _viewModel.CityEntry.CityName);
        }
    }
    private async void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        _mapService.ShowLoadingPin(myMap, e.Location);
        string cityName = await _viewModel.ProcessMapClickAsync(e.Location.Latitude, e.Location.Longitude);
        _mapService.CenterAndPin(myMap, e.Location, cityName);
    }



  

    private async void OnSuggestionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is GooglePlacePrediction prediction)
        {

            (double Latitude, double Longitude)? coords = await _viewModel.ProcessSuggestionSelectionAsync(prediction);
            if (coords.HasValue)
            {

                _mapService.CenterAndPin(myMap, new Location(coords.Value.Latitude, coords.Value.Longitude), prediction.Description);
            }
        }
        ((CollectionView)sender).SelectedItem = null; 
    }

   

    private async void OnSaveClicked(object sender, EventArgs e)
    {

        await _viewModel.SaveCityAsync();
        await DisplayAlertAsync("Successo", $"{_viewModel.CityEntry.CityName} salvata correttamente!", "OK");
        await Shell.Current.GoToAsync("..");
    }

 


}



