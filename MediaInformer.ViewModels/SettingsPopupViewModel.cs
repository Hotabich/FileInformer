using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediaInformer.DataAccets.Providers;
using System;
using System.Globalization;

namespace MediaInformer.ViewModels
{
    public class SettingsPopupViewModel : ViewModelBase
    {
        private bool isWindowHello;
        public SettingsPopupViewModel()
        {
            this.CloseSettingsCommand = new RelayCommand(CloseSettingsExecute);
        }

        public RelayCommand CloseSettingsCommand { get; private set; }
        public CultureInfo[] Language => CultureProvider.SupportedLanguage;

        public CultureInfo CurrentLanguage
        {
            get
            {
                return new CultureInfo(CultureProvider.Instance.CurrentCulture);
            }

            set
            {
                if (CultureProvider.Instance.CurrentCulture != value.Name)
                {
                    CultureProvider.Instance.CurrentCulture = value.Name;
                    CultureProvider.Instance.ApplyCulture();
                }
            }
        }

        public bool IsWindowHello
        {
            get
            {
                return this.isWindowHello;
            }
            set
            {
                this.SetWindowsHello();
            }
        }

        private void CloseSettingsExecute()
        {
            PopupProvider.Instance.CloseAnyPopup();
        }

        private async void SetWindowsHello()
        {
            this.isWindowHello = await WindowsHelloProvider.Instance.CheckWindowsHelloAsync();
            this.RaisePropertyChanged(() => IsWindowHello);
        }
    }
}
