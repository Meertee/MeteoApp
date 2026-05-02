using MeteoApp.Core.ViewModels;
using MeteoApp.Core.Models;

namespace MeteoApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (BindingContext is MainViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CRASH DURING APPEARING: {ex.Message}");
        }
    }

    private async void OnListSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is WeatherLocation selectedLocation)
        {
            ((CollectionView)sender).SelectedItem = null;
            await Shell.Current.GoToAsync($"WeatherLocationPage?LocationId={selectedLocation.Id}");
        }
    }

    private async void OnAddLocationClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SearchLocationPage));
    }
}