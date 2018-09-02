using System.IO;

namespace MediaInformer.Storage.Interfaces
{
    public interface ISerializer
    {
        T Deserialize<T>(FileStream stream);

        string Serialize<T>(T data);
    }
}
