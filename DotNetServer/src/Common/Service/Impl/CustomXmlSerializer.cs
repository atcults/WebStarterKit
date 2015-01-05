using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Common.Base;

namespace Common.Service.Impl
{
    public class CustomXmlSerializer : ISerialization
    {
        public string Serialize(object obj)
        {
            return Serialize(obj, Encoding.UTF8);
        }

        public string Serialize(object obj, Encoding encoding)
        {
            var serializer = new XmlSerializer(obj.GetType());
            string xml;
            
            using (var ms = new MemoryStream())
            {
                using (var xtw = new XmlTextWriter(ms, encoding))
                {
                    xtw.Formatting = Formatting.Indented;
                    serializer.Serialize(xtw, obj);
                    //rewind
                    ms.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(ms, encoding))
                    {
                        xml = reader.ReadToEnd();
                        xtw.Close();
                        reader.Close();
                    }
                }
            }

            Logger.Log(LogType.Info, this, xml);
            return xml;
        }

        public T Deserialize<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            Logger.Log(LogType.Info, this, xml);

            if (!String.IsNullOrEmpty(xml))
            {
                using (var sr = new StringReader(xml))
                {
                    return (T)serializer.Deserialize(sr);
                }
            }
            return default(T);
        }
    }
}