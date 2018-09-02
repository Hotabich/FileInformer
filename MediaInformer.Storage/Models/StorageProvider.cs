
namespace MediaInformer.Storage.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using System;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using System.Text;
    using Enum;
    using System.IO;

    public class StorageProvider : IStorageProvider
    {
        private const string StorageSettingsFileName = "__ApplicationSettings";
        private const string SaveFolderPickerExtensionChar = "*";
        private const string FileSavePickerExtensionChars = ".txt";
        private const string FileSavePickerExtensionName = "Text file";
        private const string DefaultFileName = "File_Info";
        private const string TimeFormat = "{0:dd-MM-yyyy_HH-mm-ss}";

        private bool isApplicationClosed;
        private FileSavePicker fileSavePicker;
        private FileSystemPath fileSystemPath;
        private object dictionaryLock = new object();
        private readonly object writesPendingLock = new object();
        private List<Task> notCompletedWrites = new List<Task>();
        private Dictionary<string, object> fileAccessLocks = new Dictionary<string, object>();
        private TaskCompletionSource<bool> applicationReactivatedTaskSource = new TaskCompletionSource<bool>();



        public StorageProvider()
        {
            this.InitializeFileSavePicker();
            this.Serializer = new Serializer();
            this.fileSystemPath = new FileSystemPath();
        }

        public ISerializer Serializer { get; set; }

        public T ReadFromSettings<T>(string key)
        {
            T result = default(T);
            try
            {
                var fileAccessLock = GetFileAccessLockObject(StorageSettingsFileName);
                lock (fileAccessLock)
                {
                    var settings = ApplicationData.Current.LocalSettings;
                    object resultObj = null;
                    if (settings.Values.TryGetValue(key, out resultObj))
                    {
                        result = (T)resultObj;
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return result;
        }

        public void WriteToSettings<T>(string key, T value)
        {
            var fileAccessLock = GetFileAccessLockObject(StorageSettingsFileName);
            lock (fileAccessLock)
            {
                try
                {
                    var settings = ApplicationData.Current.LocalSettings;
                    settings.Values[key] = value;
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                }
            }
        }

        public bool DeleteFromSetting(string key)
        {
            var result = false;
            var fileAccessLock = GetFileAccessLockObject(StorageSettingsFileName);
            lock (fileAccessLock)
            {
                try
                {
                    var settings = ApplicationData.Current.LocalSettings;
                    result = settings.Values.Remove(key);
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                }
            }

            return result;
        }

        public Task<T> ReadFromFileAsync<T>(string fileName)
        {
            return this.ReadFromFileAsync<T>(fileName, this.Serializer);
        }

        public Task<T> ReadFromFileAsync<T>(string fileName, ISerializer serializer)
        {
            return Task.Run(async () =>
            {
                T result = default(T);
                try
                {
                    var filePath = fileSystemPath.GetFolderPath(fileName, StorageType.Local);
                    using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
                    {
                        if (stream != null)
                        {
                            result = serializer.Deserialize<T>(stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                }

                return result;
            });
        }

        public Task WriteToFileAsync<T>(string fileName, T value)
        {
            var filePath = fileSystemPath.GetFolderPath(fileName, StorageType.Local);
            return this.WriteToFileAsync<T>(filePath, value, this.Serializer);
        }

        public async Task WriteToFileAsync<T>(string fileName, T value, ISerializer serializer)
        {
            try
            {
                var isCompleted = false;
                while (!isCompleted)
                {
                    if (!this.isApplicationClosed)
                    {
                        var task = this.PerformWriteToFileAsync<T>(fileName, value, serializer);
                        lock (this.writesPendingLock)
                        {
                            this.notCompletedWrites.Add(task);
                        }

                        await task.ContinueWith((endedTask) =>
                        {
                            // check exception
                            // if we have exception - rerun
                            isCompleted = endedTask.Exception == null;

                            // remove from dictionary
                            lock (this.writesPendingLock)
                            {
                                this.notCompletedWrites.Remove(task);
                            }
                        });
                    }

                    if (!isCompleted)
                    {
                        await this.applicationReactivatedTaskSource.Task;
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        public Task PerformWriteToFileAsync<T>(string fileName, T value, ISerializer serializer)
        {
            return Task.Run(async () =>
            {
                if (!String.IsNullOrEmpty(fileName))
                {
                    try
                    {
                        var data = serializer.Serialize(value);
                        File.WriteAllText(fileName, data);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        public async Task<IReadOnlyList<StorageFile>> GetFilesAsync()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add("*");

            return await openPicker.PickMultipleFilesAsync();
        }

        public async Task SaveFileAsync(string data, string fileName = null)
        {

            StorageFile file = await this.GetStorageFileAsync(fileName);
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, data);
            }
        }

        public async Task SaveFileAsync(IEnumerable<string> data, string fileName = null)
        {
            StorageFile file = await this.GetStorageFileAsync(fileName);
            if (file != null)
            {
                await FileIO.WriteLinesAsync(file, data);
            }
        }

        private void InitializeFileSavePicker()
        {
            fileSavePicker = new FileSavePicker();
            var fileTypeList = new List<string>() { FileSavePickerExtensionChars };
            fileSavePicker.FileTypeChoices.Add(FileSavePickerExtensionName, fileTypeList);
            this.fileSavePicker.SuggestedStartLocation = PickerLocationId.Desktop;

        }

        private async Task<StorageFile> GetStorageFileAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = this.GetFileName();
            }
            this.fileSavePicker.SuggestedFileName = fileName;
            return await fileSavePicker.PickSaveFileAsync();
        }

        private string GetFileName()
        {
            StringBuilder stringBuilder = new StringBuilder(DefaultFileName);
            stringBuilder.Append(string.Format(TimeFormat, DateTime.Now));
            return stringBuilder.ToString();
        }

        private object GetFileAccessLockObject(string filePath)
        {
            if (!this.fileAccessLocks.ContainsKey(filePath))
            {
                lock (this.dictionaryLock)
                {
                    if (!this.fileAccessLocks.ContainsKey(filePath))
                    {
                        this.fileAccessLocks[filePath] = new object();
                    }
                }
            }

            return this.fileAccessLocks[filePath];
        }

        private void HandleException(Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception.Message);
        }
    }
}
