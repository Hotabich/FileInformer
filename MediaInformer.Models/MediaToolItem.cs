namespace MediaInformer.Models
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class MediaToolItem
    {
        [DataMember]
        public bool IsEmpty { get; set; }

        [DataMember]
        public bool IsFavorite { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FilePath { get; set; }
    }
}
