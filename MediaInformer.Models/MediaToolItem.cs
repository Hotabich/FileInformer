using System.Runtime.Serialization;

namespace MediaInformer.Models
{

    [DataContract]
    public class MediaToolItem
    {
        [DataMember]
        public double Id { get; set; }

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
