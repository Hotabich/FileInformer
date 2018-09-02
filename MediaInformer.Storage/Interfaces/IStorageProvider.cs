namespace MediaInformer.Storage.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.Storage;

    public interface IStorageProvider
    {
        T ReadFromSettings<T>(string key);
        void WriteToSettings<T>(string key, T value);
        bool DeleteFromSetting(string key);
        Task SaveFileAsync(string data, string fileName = null);
        Task SaveFileAsync(IEnumerable<string> data, string fileName = null);
        Task<IReadOnlyList<StorageFile>> GetFilesAsync();
        Task WriteToFileAsync<T>(string fileName, T value);
        Task<T> ReadFromFileAsync<T>(string fileName);

    }
}
