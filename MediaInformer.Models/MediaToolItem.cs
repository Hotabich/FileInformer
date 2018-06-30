namespace MediaInformer.Models
{
    using Windows.Storage;
    public class MediaToolItem
    {
        public int Id { get; set; }

        public bool IsEmpty { get; set; }

        public StorageFile File { get; set; }
    }
}
