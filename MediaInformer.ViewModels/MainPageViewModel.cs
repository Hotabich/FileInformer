namespace MediaInformer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using MediaInformer.Storage.Interfaces;
    using MediaInformer.Storage.Models;
    using MediaInformer.DI;
    using Windows.Storage;
    using MediaInformer.Models;
    using Windows.Storage.Pickers;
    using System.Collections.Generic;
    using System.Linq;
    using Windows.UI.Xaml.Controls;
    using MediaInformer.DataAccets.Providers;

    public class MainPageViewModel : BaseViewModel
    {
        private readonly IStorageProvider storageProvider = Factory.CommonFactory.GetInstance<IStorageProvider>();

        public MainPageViewModel()
        {
            this.files.Add(new MediaToolItem { IsEmpty = true });
            this.AddFilesCommand = new RelayCommand(this.AddFilesExecuteAsync);
        }

        public RelayCommand AddFilesCommand { get; private set; }

        public override bool IsItemsFileEmpty
        {
            get
            {
                var result = Files.Count > 1;
                if (!result)
                {
                    this.IsSelectionMode = result;
                }
                return result;
            }
        }

        protected override async void ItemClickExecute(MediaToolItem item)
        {
            if (item != null && item.IsEmpty)
            {
                this.AddFilesExecuteAsync();
            }
            else
            {
                await RecentProvider.Instance.AddToRecentAsync(item);
                this.NavigateToInfo(item);
            }
        }

        private async void AddFilesExecuteAsync()
        {
            this.BusyCount++;

            var files = await this.storageProvider.GetFilesAsync();

            if (files != null && files.Count > 0)
            {
                foreach (StorageFile file in files)
                {
                    this.Files.Add(new MediaToolItem
                    {
                        IsEmpty = false,
                        FilePath = file.Path,
                        FileName = file.Name
                    });
                }
            }

            this.BusyCount--;
        }
    }
}
