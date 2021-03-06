﻿namespace MediaInformer.DataAccets.Providers
{
    using MediaInformer.DI;
    using MediaInformer.Models;
    using MediaInformer.Storage.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FavoriteProvider
    {
        private const string FavoriteFileKey = "FavoriteFiles";
        private const double DefaultID = 0;

        private List<MediaToolItem> favoriteItems = new List<MediaToolItem>();
        private static Lazy<FavoriteProvider> instance = new Lazy<FavoriteProvider>(() => new FavoriteProvider(), false);
        private readonly IStorageProvider storageProvider = Factory.CommonFactory.GetInstance<IStorageProvider>();

        public static FavoriteProvider Instance
        {
            get { return instance.Value; }
        }

        public async Task<List<MediaToolItem>> GetFavoriteFilesAsync()
        {
            var result = await this.storageProvider.ReadFromFileAsync<List<MediaToolItem>>(FavoriteFileKey);
            if (result != null)
            {
                this.favoriteItems = result;
            }
            return this.favoriteItems;
        }

        public async Task AddToFavoriteAsync(MediaToolItem item)
        {
            var items = await this.GetFavoriteFilesAsync();
            var file = items.FirstOrDefault(x => x.FilePath == item.FilePath);
            if (file == null)
            {
                this.favoriteItems.Add(item);
                await this.WriteToFileAsync();
            }
        }

        public async Task DeleteFromFavoriteAsync(MediaToolItem item)
        {
            var items = await this.GetFavoriteFilesAsync();
            if (item != null && items.Count > 0)
            {
                items.Remove(item);
            }
            this.favoriteItems = items;
            await this.WriteToFileAsync();
        }

        public async Task ClearFavoriteAsync()
        {
            this.favoriteItems.Clear();
            await this.WriteToFileAsync();
        }

        public async Task<bool> IsFavoriteFileAsync(string path)
        {
            var result = false;
            var items = await this.GetFavoriteFilesAsync();
            var item = items.FirstOrDefault(x => x.FilePath == path);
            if (item != null)
            {
                result = true;
            }
            return result;
        }

        private async Task WriteToFileAsync()
        {
            await storageProvider.WriteToFileAsync(FavoriteFileKey, this.favoriteItems);
        }
    }
}
