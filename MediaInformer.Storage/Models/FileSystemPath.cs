
namespace MediaInformer.Storage.Models
{
    using System;
    using System.Text;
    using Enum;
    using Windows.Storage;

    public class FileSystemPath
    {
        private static readonly char[] FilePathSeparators = new char[2] { '/', '\\' };

        public string GetFolderPath(string key, StorageType systemFolderType)
        {
            var folderPath = String.Empty;

            switch (systemFolderType)
            {
                case StorageType.Local:
                    folderPath = ApplicationData.Current.LocalFolder.Path;
                    break;
                case StorageType.LocalCache:
                    folderPath = ApplicationData.Current.LocalCacheFolder.Path;
                    break;
                case StorageType.Roaming:
                    folderPath = ApplicationData.Current.RoamingFolder.Path;
                    break;
                case StorageType.SharedLocal:
                    folderPath = ApplicationData.Current.SharedLocalFolder.Path;
                    break;
                case StorageType.Temporary:
                    folderPath = ApplicationData.Current.TemporaryFolder.Path;
                    break;
            }
            var path = new StringBuilder(folderPath);
            path.Append(FilePathSeparators[1]);
            path.Append(key);
            return path.ToString();
        }
    }
}
