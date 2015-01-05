using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Common.Base;

namespace Common.SerializerHelper
{
    public class BinarySerializer
    {
        public byte[] SerializeList<T>(List<T> list)
        {
            var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(ms, list);
            ms.Position = 0;
            var serializedList = new byte[ms.Length];
            ms.Read(serializedList, 0, (int)ms.Length);
            ms.Close();
            return serializedList;
        }

        public List<T> DeserializeList<T>(byte[] data)
        {
            try
            {
                var ms = new MemoryStream();
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                var bf = new BinaryFormatter();
                var list = bf.Deserialize(ms) as List<T>;
                return list;
            }
            catch (SerializationException ex)
            {
                Logger.Log(LogType.Error, typeof(BinarySerializer), "BinarySerializer-DeserializeList", ex);
                return null;
            }
        }
    }
}