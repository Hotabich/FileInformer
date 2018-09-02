namespace MediaInformer.Storage.Models
{
    using Interfaces;
    using Newtonsoft.Json;
    using System.IO;
    using System.Runtime.Serialization.Json;

    public class Serializer : ISerializer
    {
        private const int StartPosition = 0;
        public T Deserialize<T>(FileStream stream)
        {
            T result = default(T);
            var deserializer = new DataContractJsonSerializer(typeof(T));
            try
            {
                result = (T)deserializer.ReadObject(stream);
            }
            catch { }

            return result;
        }

        public string Serialize<T>(T data)
        {
            var result = string.Empty;
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, data);
                memoryStream.Position = StartPosition;
                using (var sr = new StreamReader(memoryStream))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }
    }
}
