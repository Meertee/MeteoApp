using MeteoApp.Core.Models;
using MeteoApp.Core.ViewModels;
using MeteoApp.Extensions;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace MeteoApp.Views;

public partial class SearchLocationPage : ContentPage
{
    private double _selectedLatitude;
    private double _selectedLongitude;
    private bool _isSelectingFromList = false;

    public SearchLocationPage(SearchLocationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _selectedLatitude = 45.4642;
        _selectedLongitude = 9.1899;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCurrentLocationAsync();
    }

    private async Task LoadCurrentLocationAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            if (status == PermissionStatus.Granted)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    UpdatePosition(location.Latitude, location.Longitude, "Your position");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Location Error: {ex.Message}");
        }
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        UpdatePosition(e.Location.Latitude, e.Location.Longitude, "Punto selezionato");
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(locationSearchBar.Text)) return;

        try
        {
            var locations = await Geocoding.Default.GetLocationsAsync(locationSearchBar.Text);
            var loc = locations?.FirstOrDefault();
            if (loc != null)
            {
                UpdatePosition(loc.Latitude, loc.Longitude, locationSearchBar.Text);
            }
            else
            {
                await DisplayAlertAsync("Errore", "Località non trovata.", "OK");
            }
        }
        catch (Exception)
        {
            await DisplayAlertAsync("Errore", "Impossibile completare la ricerca.", "OK");
        }
    }

    private async void OnSuggestionSelected(object sender, SelectionChangedEventArgs e)
    {
        var suggestion = e.CurrentSelection.FirstOrDefault() as LocationSuggestion;
        if (suggestion == null) return;
        _isSelectingFromList = true;
        locationSearchBar.Text = suggestion.Name;
        var viewModel = (SearchLocationViewModel)BindingContext;
        viewModel.IsSuggestionsVisible = false;
        try
        {
            var locations = await Geocoding.Default.GetLocationsAsync(suggestion.Name);
            var loc = locations?.FirstOrDefault();
            if (loc != null)
            {
                UpdatePosition(loc.Latitude, loc.Longitude, suggestion.Name);
            }
        }
        catch (Exception)
        {
            await DisplayAlertAsync("Errore", "Impossibile caricare la posizione.", "OK");
        }
        ((CollectionView)sender).SelectedItem = null;
    }

    private void UpdatePosition(double lat, double lon, string label)
    {
        _selectedLatitude = lat;
        _selectedLongitude = lon;

        var location = new Location(lat, lon);

        myMap.CenterAndPin(location, label);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is SearchLocationViewModel viewModel)
        {
            await viewModel.SaveLocationAsync(new Tuple<double, double>(_selectedLatitude, _selectedLongitude));
            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = (SearchLocationViewModel)BindingContext;
        if (_isSelectingFromList)
        {
            _isSelectingFromList = false;
            return;
        }
        if (e.NewTextValue?.Length >= 3)
        {
            await viewModel.LoadSuggestionsAsync(e.NewTextValue);
        }
        else
        {
            viewModel.Suggestions.Clear();
            viewModel.IsSuggestionsVisible = false;
        }
    }
}