namespace MediaInformer.DataAccets.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MediaInformer.DI;
    using System.Linq;
    using MediaInformer.Storage.Interfaces;
    using Models;

    public class RecentProvider
    {
        private const string RecentFileKey = "RecentFiles";
        private const double DefaultID = 0;

        private List<MediaToolItem> recentItems = new List<MediaToolItem>();
        private static Lazy<RecentProvider> instance = new Lazy<RecentProvider>(() => new RecentProvider(), false);
        private readonly IStorageProvider storageProvider = Factory.CommonFactory.GetInstance<IStorageProvider>();

        public static RecentProvider Instance
        {
            get { return instance.Value; }
        }

        public async Task<List<MediaToolItem>> GetRecentFilesAsync()
        {
            var result = await this.storageProvider.ReadFromFileAsync<List<MediaToolItem>>(RecentFileKey);
            if (result != null)
            {
                this.recentItems = result;
            }
            return this.recentItems;
        }

        public async Task AddToRecentAsync(MediaToolItem item)
        {
            var items = await this.GetRecentFilesAsync();
            var maxId = DefaultID;
            if (items.Count > 0)
            {
                maxId = items.Max(x => x.Id);
            }
            item.Id = maxId;
            this.recentItems.Add(item);
            await this.WriteToFileAsync();
        }

        public async Task DeleteFromRecentAsync(MediaToolItem item)
        {
            var items = await this.GetRecentFilesAsync();
            if (item != null && items.Count > 0)
            {
                items.Remove(item);
            }
            this.recentItems = items;
            await this.WriteToFileAsync();
        }

        public async Task ClearRecentAsync()
        {
            this.recentItems.Clear();
            await this.WriteToFileAsync();
        }



        private async Task WriteToFileAsync()
        {
            await storageProvider.WriteToFileAsync<List<MediaToolItem>>(RecentFileKey, this.recentItems);
        }
    }
}
