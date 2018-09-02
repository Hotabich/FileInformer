
namespace MediaInformer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight.Command;
    using MediaInformer.DataAccets.Providers;
    using MediaInformer.DI;
    using MediaInformer.Models;
    using MediaInformer.Storage.Interfaces;
    using Windows.UI.Xaml.Navigation;
    public class InfoPageViewModel : BaseViewModel
    {
        private readonly IStorageProvider storageProvider = Factory.CommonFactory.GetInstance<IStorageProvider>();
        public InfoPageViewModel()
        {
            this.InitializeCommand = new RelayCommand(this.InitializeExecute);
            this.SaveInfoCommand = new RelayCommand(this.SaveInfoExecute, this.SaveInfoCanExecute);
            this.AddToFavoriteCommand = new RelayCommand(this.AddToFavoriteExecute, this.CanAddToFavoriteExecute);
            this.DeleteFromFavoriteCommand = new RelayCommand(this.DeleteFromFavoriteExecute);
        }

        public string Info { get; set; }

        public bool IsFavorite { get; set; }

        public bool CanAddToFavoriteExecute
        {
            get { return this.CurrentItem != null; }
        }

        private bool SaveInfoCanExecute => String.IsNullOrEmpty(this.Info);

        public RelayCommand InitializeCommand { get; private set; }

        public RelayCommand SaveInfoCommand { get; private set; }

        public RelayCommand AddToFavoriteCommand { get; private set; }

        public RelayCommand DeleteFromFavoriteCommand { get; private set; }




        protected override void ShareExecute()
        {
            ShareProvider.Instance.Share();
        }

        private async void InitializeExecute()
        {
            this.BusyCount++;
            var parameter = NavigationProvider.NavigationParameter as MediaToolItem;
            if (parameter != null)
            {
                var response = await ServiceConnectionProvider.GetFileInfo(parameter.FilePath);
                this.Info = response.Result;
                this.CurrentItem = parameter;
            }
            await this.IsFavoriteFile();
            this.RaisePropertyChanged(() => this.Info);
            this.BusyCount--;
        }

        private async void SaveInfoExecute()
        {
            this.BusyCount++;

            await this.storageProvider.SaveFileAsync(this.Info);

            this.BusyCount--;
        }

        private async void AddToFavoriteExecute()
        {
            this.CurrentItem.IsFavorite = true;
            await FavoriteProvider.Instance.AddToFavoriteAsync(this.CurrentItem);
        }

        private async void DeleteFromFavoriteExecute()
        {
            this.CurrentItem.IsFavorite = false;
            await FavoriteProvider.Instance.DeleteFromFavoriteAsync(this.CurrentItem);
        }

        private async Task IsFavoriteFile()
        {
            this.IsFavorite = await FavoriteProvider.Instance.IsFavoriteFileAsync(this.CurrentItem.Id);
        }
    }
}
