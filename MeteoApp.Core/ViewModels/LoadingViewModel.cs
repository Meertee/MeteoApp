using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MeteoApp.Core.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged { }
    public abstract class LoadingViewModel : BaseViewModel
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }
    }
}
