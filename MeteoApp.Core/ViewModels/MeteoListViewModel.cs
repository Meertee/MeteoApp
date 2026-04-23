using MeteoApp.Core.Models;
using MeteoApp.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MeteoApp.Core.ViewModels
{
    public class MeteoListViewModel : BaseViewModel
    {
        private readonly ILocationService _locationService;
        private readonly DatabaseService _dbService;

        private ObservableCollection<WeatherLocation> _entries = [];
        public ObservableCollection<WeatherLocation> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                OnPropertyChanged();
            }
        }

        public MeteoListViewModel(ILocationService locationService, DatabaseService dbService)
        {
            //Aggiunge la prima riga
            _locationService = locationService;
            _dbService = dbService;

            WeatherLocation placeholder = new()
            {
                Id = 0,
                CityName = "📍 Tocca qui per la tua posizione",
                IsCurrentLocation = true
            };

            Entries.Add(placeholder);
        }

        public async Task FetchLocationOnClickAsync()
        {
            WeatherLocation? placeholderEntry = Entries.FirstOrDefault(e => e.IsCurrentLocation);
            if (placeholderEntry!=null)
            {
                int index = Entries.IndexOf(placeholderEntry);
                Entries[index] = new WeatherLocation
                {
                    Id = 0,
                    CityName = "⏳ Ricerca del GPS in corso...",
                    IsCurrentLocation = true
                };
                try
                {
                   
                    WeatherLocation currentEntry = await _locationService.GetCurrentLocationAsync();

                    if (currentEntry != null)
                    {
                        
                        Entries[index] = currentEntry;
                    }
                    else
                    {
                       
                        Entries[index] = new WeatherLocation { Id = 0, CityName = "❌ Posizione non trovata (Riprova)", IsCurrentLocation = true };
                    }
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Errore caricando la posizione: {ex.Message}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Errore caricando la posizione: {ex.Message}");
                    
                    Entries[index] = new WeatherLocation { Id = 0, CityName = "❌ Errore GPS (Tocca per riprovare)", IsCurrentLocation = true };
                }
            }     
        }

        public async Task LoadEntriesAsync()
        {
           
            List<WeatherLocation> savedEntries = await _dbService.GetEntriesAsync();
            WeatherLocation? gpsEntry = Entries.FirstOrDefault(e => e.IsCurrentLocation);

            Entries.Clear();

            if (gpsEntry != null)
            {
                Entries.Add(gpsEntry);
            }

            foreach (WeatherLocation entry in savedEntries)
            {
                if (!entry.IsCurrentLocation)
                {
                    Entries.Add(entry);
                }
            }
        }
    }
}