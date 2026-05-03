// MeteoApp.Core/ViewModels/SettingsViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;

using MeteoApp.Core.Services.Interfaces.Preferences;

namespace MeteoApp.Core.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

         
            _isFahrenheit = _settingsService.UseFahrenheit;
        }

      
        [ObservableProperty]
        private bool _isFahrenheit;

        partial void OnIsFahrenheitChanged(bool value)
        {
            // Salva la nuova scelta dell'utente!
            _settingsService.UseFahrenheit = value;
            System.Diagnostics.Debug.WriteLine($"Impostazione salvata. Usa Fahrenheit: {value}");
        }
    }
}