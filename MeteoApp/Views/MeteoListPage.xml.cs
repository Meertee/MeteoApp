using MeteoApp.Core.Models;       
using MeteoApp.Core.ViewModels;
using Microsoft.Maui.Controls;
using System.Runtime.Versioning;
using ModelEntry = MeteoApp.Core.Models.Entry;

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
        if (e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is ModelEntry selectedEntry)
        {
            CollectionView list = (CollectionView)sender;
            list.SelectedItem = null;


            // CONTROLLO: È la voce del GPS?
            if (selectedEntry.IsCurrentLocation)
            {
                // prende il modello dal binding recupera il modello collegato alla view
                MeteoListViewModel viewModel = (MeteoListViewModel)BindingContext;

                // Facciamo partire il metodo che abbiamo appena creato!
                if (viewModel != null)
                {
                    await viewModel.FetchLocationOnClickAsync();
                }
            }
            else
            {
                // Se clicco una città già esistente, vado ai DETTAGLI, non alla mappa
                Dictionary<string, object> navigationParameter = new()
                {
                { "Entry", selectedEntry }
            };

                await Shell.Current.GoToAsync("entrydetails", navigationParameter);
            }

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
        { "Entry", newEntry }
    };

       
        await Shell.Current.GoToAsync("mappage", navigationParameter);
    }
}