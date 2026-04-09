using MeteoApp.Core.Models;
using MeteoApp.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MeteoApp.Core.ViewModels
{
    public class MeteoListViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;

        private ObservableCollection<Entry> _entries = new ObservableCollection<Entry>();
        public ObservableCollection<Entry> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                OnPropertyChanged();
            }
        }

        public MeteoListViewModel(ILocationService locationService)
        {
            _locationService = locationService;
            // Avvia il caricamento asincrono senza bloccare il costruttore
            _ = LoadCurrentLocationAsync();
        }

        private async Task LoadCurrentLocationAsync()
        {
            try
            {
                var currentEntry = await _locationService.GetCurrentLocationAsync();
                if (currentEntry != null)
                {
                   
                    Entries.Insert(0, currentEntry);
                }
            }

            catch (HttpRequestException ex)
            {
                // connectivity problem
            }
            catch (Exception ex)
            {
              
                System.Diagnostics.Debug.WriteLine($"Errore caricando la posizione: {ex.Message}");
            }
        }
    }
}