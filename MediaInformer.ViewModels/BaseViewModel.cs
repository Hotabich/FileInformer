namespace MediaInformer.ViewModels
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using MediaInformer.DataAccets.Providers;
    using MediaInformer.DI;
    using MediaInformer.Models;
    using MediaInformer.Models.Enums;
    using MediaInformer.Models.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using Windows.UI.Xaml.Controls;

    public class BaseViewModel : ViewModelBase
    {
        private readonly INavigationProvider navigationProvider = Factory.CommonFactory.GetInstance<INavigationProvider>();

        private int busyCount;
        protected bool isSelestionMode;
        protected MediaToolItem selectedItem;
        protected ObservableCollection<MediaToolItem> files;
        protected ObservableCollection<MediaToolItem> selectedFiles;

        public BaseViewModel()
        {
            this.files = new ObservableCollection<MediaToolItem>();
            this.selectedFiles = new ObservableCollection<MediaToolItem>();
            this.files.CollectionChanged += this.OnCollectionChanget;
            this.selectedFiles.CollectionChanged += this.OnSelectedItemsCollectionChanget;
            this.ShareCommand = new RelayCommand(this.ShareExecute);
            this.DeleteCommand = new RelayCommand(this.DeleteExecute, IsSelectedItemEmpty);
            this.ItemClickCommand = new RelayCommand<MediaToolItem>(this.ItemClickExecute);
            this.SaveItemCommand = new RelayCommand(this.SaveItemExecute, IsSelectedItemEmpty);
            this.SelectAllCommand = new RelayCommand(this.SelectAllExecute, this.IsItemsFileEmpty);
            this.DeselectAllCommand = new RelayCommand(this.DeselectAllExecute, this.IsItemsFileEmpty);
            this.ShowSettingsPopupCommand = new RelayCommand(ShowSettingsPopupExecute);
        }

        public RelayCommand<MediaToolItem> ItemClickCommand { get; protected set; }

        public RelayCommand SelectAllCommand { get; protected set; }

        public RelayCommand DeselectAllCommand { get; protected set; }

        public RelayCommand SaveItemCommand { get; protected set; }

        public RelayCommand ShareCommand { get; protected set; }

        public RelayCommand DeleteCommand { get; protected set; }

        public RelayCommand ShowSettingsPopupCommand { get; private set; }


        public bool IsSelectedItemEmpty => this.SelectedItems.Count > 0;

        public bool IsBusy
        {
            get
            {
                return this.BusyCount != 0;
            }
        }

        public bool IsSelectionMode
        {
            get
            {
                return this.isSelestionMode;
            }
            set
            {
                if (isSelestionMode != value)
                {
                    this.isSelestionMode = value;
                    this.SelectionModeExecute();
                    this.RaisePropertyChanged();
                }
            }
        }

        public virtual bool IsItemsFileEmpty
        {
            get
            {
                var result = Files.Count > 0;
                if (!result)
                {
                    this.IsSelectionMode = result;
                }
                return result;
            }
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

        public ObservableCollection<MediaToolItem> SelectedItems
        {
            get { return this.selectedFiles; }
            set
            {
                if (this.selectedFiles != value)
                {
                    this.selectedFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public MediaToolItem CurrentItem
        {
            get { return this.selectedItem; }
            set
            {
                if (this.selectedItem != value)
                {
                    this.selectedItem = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ListViewSelectionMode SelectionMode { get; set; }

        protected INavigationProvider NavigationProvider
        {
            get
            {
                return this.navigationProvider;
            }
        }

        protected int BusyCount
        {
            get
            {
                return this.busyCount;
            }

            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Busy Count can't be negative value");
                }
                if (this.busyCount != value)
                {
                    this.busyCount = value;
                    this.RaisePropertyChanged(() => this.IsBusy);
                }
            }
        }



        protected virtual void ShareExecute() { }

        protected virtual void Initialize() { }

        protected virtual async void ItemClickExecute(MediaToolItem item)
        {
            if (item != null && !item.IsEmpty)
            {
                await RecentProvider.Instance.AddToRecentAsync(item);
                this.NavigateToInfo(item);
            }
        }

        protected virtual void DeleteExecute()
        {
            var deletedItem = new List<MediaToolItem>();

            foreach (MediaToolItem item in SelectedItems)
            {
                deletedItem.Add((MediaToolItem)item);
            }

            foreach (var item in deletedItem)
            {
                if (this.SelectedItems.Contains(item))
                {
                    this.SelectedItems.Remove(item);
                    this.Files.Remove(item);
                }
            }
        }

        protected void NavigateToInfo(MediaToolItem filePath)
        {
            if (filePath != null)
            {
                NavigationProvider.Navigate(NavigationSource.InfoPage, filePath);
            }
        }

        protected void SelectionModeExecute()
        {

            this.SelectionMode = isSelestionMode ? ListViewSelectionMode.Multiple : ListViewSelectionMode.None;
            this.RaisePropertyChanged(() => SelectionMode);
        }

        protected void DeselectAllExecute()
        {
            foreach (var item in this.Files)
            {
                if (this.SelectedItems.Contains(item))
                {
                    this.SelectedItems.Remove(item);
                }
            }
        }

        protected void SelectAllExecute()
        {
            foreach (var item in this.Files)
            {
                if (!this.SelectedItems.Contains(item) && !item.IsEmpty)
                {
                    this.SelectedItems.Add(item);
                }
            }
        }

        protected void SaveItemExecute()
        {

        }

        protected void ShowSettingsPopupExecute()
        {
            PopupProvider.Instance.ShowPopupContent(PopupNavigationSource.SettingsPopup);
        }

        protected virtual void OnCollectionChanget(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(() => IsItemsFileEmpty);
            this.RaisePropertyChanged(() => IsSelectedItemEmpty);
        }

        protected virtual void OnSelectedItemsCollectionChanget(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(() => IsSelectedItemEmpty);
        }

    }
}
