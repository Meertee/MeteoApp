using MeteoApp.Core.Models;       
using Microsoft.Maui.Controls;
using MeteoApp.Core.ViewModels;
using ModelEntry = MeteoApp.Core.Models.Entry;

namespace MeteoApp.Views;

public partial class MeteoListPage : Shell
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
               
                Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "Entry", selectedEntry }
            };

                await Shell.Current.GoToAsync($"entrydetails", navigationParameter);
            }
        }
    }

    private void OnItemAdded(object sender, EventArgs e)
    {
        _ = ShowPrompt();
    }

    private async Task ShowPrompt()
    {
        await DisplayAlert("Add City", "To Be Implemented", "OK");
    }
}