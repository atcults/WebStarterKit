using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Common.SerializerHelper
{
    /// <summary>
    /// Summary description for XmlSerializer
    /// </summary>
    public class DataContract2Obj<T>
    {
        public static T Load(string xmlPath)
        {
            using (var fs = new FileStream(xmlPath, FileMode.Open))
            {
                var reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                var ser = new DataContractSerializer(typeof (T));
                var obj = (T) ser.ReadObject(reader, true);
                reader.Close();
                fs.Close();
                return obj;
            }
        }

        public static void Save(string xmlPath, T obj)
        {
            using (var fs = new FileStream(xmlPath, FileMode.Create))
            {
                var ser = new DataContractSerializer(typeof (T));
                ser.WriteObject(fs, obj);
                fs.Close();
            }
        }
    }
}