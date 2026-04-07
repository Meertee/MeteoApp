using MeteoApp.Core.Models;

using System.Collections.ObjectModel;



namespace MeteoApp.Core.ViewModels
{
    public class MeteoListViewModel : BaseViewModel
    {
        ObservableCollection<Entry> _entries;

        public ObservableCollection<Entry> Entries
        {
            get { return _entries; }
            set
            {
                _entries = value;
                OnPropertyChanged();
            }
        }

        public MeteoListViewModel()
        {
            Entries = new ObservableCollection<Entry>();

            for (var i = 0; i < 2; i++)
            {
                var e = new Entry
                {
                    Id = i
                };

                Entries.Add(e);
            }
        }
    }
}
