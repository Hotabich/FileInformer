namespace MediaInformer.ViewModels
{
    using DataAccets.Providers;
    using Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class RecentViewModel : BaseViewModel
    {
        public RecentViewModel()
        {
            this.Initialize();
        }

        protected override async void Initialize()
        {
            this.BusyCount++;
            var result = await RecentProvider.Instance.GetRecentFilesAsync();
            this.Files = new ObservableCollection<MediaToolItem>(result);
            this.Files.CollectionChanged += this.OnCollectionChanget;
            RaisePropertyChanged(() => this.IsItemsFileEmpty);
            this.BusyCount--;
        }

        protected async override void DeleteExecute()
        {
            this.BusyCount++;
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
            await RecentProvider.Instance.ClearRecentAsync();
            this.BusyCount--;
        }
    }
}
