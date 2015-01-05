using System.Text;

namespace Common.Service
{
    public interface ISerialization
    {
        string Serialize(object obj, Encoding encoding);
        string Serialize(object obj);
        T Deserialize<T>(string str);
    }
}