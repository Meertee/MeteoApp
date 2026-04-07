using MeteoApp.Core.Models;       
using Microsoft.Maui.Controls;
using MeteoApp.Core.ViewModels;
using ModelEntry = MeteoApp.Core.Models.Entry;

namespace MeteoApp.Views;

public partial class MeteoListPage : Shell
{
    public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

    public MeteoListPage()
    {
        InitializeComponent();
        RegisterRoutes();

        // Istanzia il ViewModel che ora risiede in MeteoApp.Core
        BindingContext = new MeteoListViewModel();
    }

    private void RegisterRoutes()
    {
        Routes.Add("entrydetails", typeof(MeteoItemPage));

        foreach (var item in Routes)
            Routing.RegisterRoute(item.Key, item.Value);
    }

    private void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            ModelEntry entry = e.SelectedItem as ModelEntry;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Entry", entry }
            };

            Shell.Current.GoToAsync($"entrydetails", navigationParameter);
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