using System;
using System.Text;
using Newtonsoft.Json;

namespace Common.Service.Impl
{
    public class JsonSerialization : ISerialization
    {
        public class NativeDataType
        {
            public object Id { get; set; }
        }

        public string Serialize(object obj, Encoding encoding)
        {
            string json;
            if (obj is Guid)
            {
                json = JsonConvert.SerializeObject(new NativeDataType { Id = obj });
            }
            else
            {
                json = JsonConvert.SerializeObject(obj);
            }
            Console.WriteLine(json);
            return json;
        }

        public string Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public T Deserialize<T>(string json)
        {
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
    }
}