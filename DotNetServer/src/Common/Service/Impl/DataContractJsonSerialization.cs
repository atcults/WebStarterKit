using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Common.Service.Impl
{
    public class DataContractJsonSerialization : ISerialization
    {
        public string Serialize(object obj, Encoding encoding)
        {
            //Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(ms, obj);
            var json = ms.ToArray();
            ms.Close();
            return encoding.GetString(json, 0, json.Length);
        }

        public string Serialize(object obj)
        {
            //Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(ms, obj);
            var json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public T Deserialize<T>(string str)
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));
            var ser = new DataContractJsonSerializer(typeof(T));
            var deserializedUser = (T)ser.ReadObject(ms);
            ms.Close();
            return deserializedUser;
        }
    }
}