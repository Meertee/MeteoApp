namespace MeteoApp;

using Microsoft.Maui.Controls;
using ModelEntry = MeteoApp.Core.Models.Entry;

[QueryProperty(nameof(Entry), "Entry")]
public partial class MeteoItemPage : ContentPage
{
    private ModelEntry _passedEntry;
    public ModelEntry PassedEntry
    {
        get => _passedEntry;
        set
        {
            _passedEntry = value;
            OnPropertyChanged();

         
            BindingContext = _passedEntry;
        }
    }

    public MeteoItemPage()
    {
        InitializeComponent();
        
    }

    
}