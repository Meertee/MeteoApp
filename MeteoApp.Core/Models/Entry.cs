
namespace MeteoApp.Core.Models
{
    public class Entry
    {
        public int Id { get; set; }
        // Se nel tuo XAML c'è un IsVisible="{Binding Done}", aggiungi anche la proprietà Done:
        public bool Done { get; set; }
    }
}
