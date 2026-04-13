using MeteoApp.Core.Models;       
using Microsoft.Maui.Controls;
using MeteoApp.Core.ViewModels;
using ModelEntry = MeteoApp.Core.Models.Entry;

namespace MeteoApp.Views;

public partial class MeteoListPage : ContentPage
{
    public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

    public MeteoListPage(MeteoListViewModel viewModel)
    {
        InitializeComponent();
        RegisterRoutes();

        // Istanzia il ViewModel che ora risiede in MeteoApp.Core
        BindingContext = viewModel;
    }

    private void RegisterRoutes()
    {
        Routes.Add("entrydetails", typeof(MeteoItemPage));
        Routes.Add("mappage", typeof(MeteoMapPage));

        foreach (var item in Routes)
            Routing.RegisterRoute(item.Key, item.Value);
    }

    private async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            ModelEntry selectedEntry = (ModelEntry)e.SelectedItem;

            //Deseleziona
            ListView list = (ListView)sender;
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
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "Entry", selectedEntry }
            };

                await Shell.Current.GoToAsync("entrydetails", navigationParameter);
            }

        }
    }

    private void OnItemAdded(object sender, EventArgs e)
    {
        _ = ShowPrompt();
    }

    private async Task ShowPrompt()
    {
        ModelEntry newEntry = new ModelEntry
        {
            Id = 0,
            CityName = "Nuova Località",
            Latitude = 41.9028, 
            Longitude = 12.4964,
            IsCurrentLocation = false,
            Done = false
        };

       
        Dictionary<string, object> navigationParameter = new Dictionary<string, object>
    {
        { "Entry", newEntry }
    };

       
        await Shell.Current.GoToAsync("mappage", navigationParameter);
    }
}