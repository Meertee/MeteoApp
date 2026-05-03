using MeteoApp.Core.ViewModels;

namespace MeteoApp.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
       
            BindingContext = viewModel;
        }
    }
}