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
            //Aggiunge la prima riga
            _locationService = locationService;
            
            Entry placeholder = new Entry
            {
                Id = 0,
                CityName = "📍 Tocca qui per la tua posizione",
                IsCurrentLocation = true
            };

            Entries.Add(placeholder);
        }

        public async Task FetchLocationOnClickAsync()
        {
            Entry placeholderEntry = Entries.FirstOrDefault(e => e.IsCurrentLocation);
            if (placeholderEntry!=null)
            {
                int index = Entries.IndexOf(placeholderEntry);
                Entries[index] = new Entry
                {
                    Id = 0,
                    CityName = "⏳ Ricerca del GPS in corso...",
                    IsCurrentLocation = true
                };
                try
                {
                   
                    Entry currentEntry = await _locationService.GetCurrentLocationAsync();

                    if (currentEntry != null)
                    {
                        
                        Entries[index] = currentEntry;
                    }
                    else
                    {
                       
                        Entries[index] = new Entry { Id = 0, CityName = "❌ Posizione non trovata (Riprova)", IsCurrentLocation = true };
                    }
                }
                catch (HttpRequestException ex)
                {
                    // connectivity problem
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Errore caricando la posizione: {ex.Message}");
                    
                    Entries[index] = new Entry { Id = 0, CityName = "❌ Errore GPS (Tocca per riprovare)", IsCurrentLocation = true };
                }
            }

          
            
            
        }
    }
}