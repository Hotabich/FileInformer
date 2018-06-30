namespace MediaInformer.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows.Input;
    using Windows.Storage;
    using MediaInformer.Models;
    using Windows.Storage.Pickers;
    using System.Collections.Generic;
    using System.Linq;

    public class MainPageViewModel : BaseViewModel
    {
        private ObservableCollection<MediaToolItem> files;

        public MainPageViewModel()
        {
            this.files = new ObservableCollection<MediaToolItem>();
            this.files.Add(new MediaToolItem { IsEmpty = true });
            this.AddFilesCommand = new RelayCommand(this.AddFilesExecuteSync);
            this.files.CollectionChanged += this.OnCollectionChanget;
        }

        public ObservableCollection<MediaToolItem> Files
        {
            get { return this.files; }
            set
            {
                if (this.files != value)
                {
                    this.files = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<MediaToolItem> SelectedItems { get; set; }

        public ICommand AddFilesCommand { get; private set; }
        private async void AddFilesExecuteSync()
        {
            this.BusyCount++;

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");

            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();

            if (files.Count > 0)
            {
                var id = this.GetMaxItemsId();

                foreach (StorageFile file in files)
                {
                    this.Files.Add(new MediaToolItem
                    {
                        Id = id,
                        IsEmpty = false,
                        File = file
                    });
                }
            }

            this.BusyCount--;
        }

        private int GetMaxItemsId()
        {
            var id = 0;
            if (this.Files.Count > 0)
            {
                id = this.Files.Max(x => x.Id);
            }
            return id;
        }

        private void OnCollectionChanget(object sender, NotifyCollectionChangedEventArgs e)
        {
            // throw new NotImplementedException();
        }
    }
}
