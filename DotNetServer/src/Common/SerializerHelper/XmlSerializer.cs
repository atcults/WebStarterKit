using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common.SerializerHelper
{
    /// <summary>
    /// Summary description for XmlSerializer
    /// </summary>
    public class XmlSerializer<T>
    {
        /// <summary>
        /// XML serializer for class
        /// </summary>
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof (T));

        /// <summary>
        /// Serialize object into XML string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string Serialize(T obj, Encoding encoding)
        {
            string result = null;
            if (!Equals(obj, default(T)))
            {
                using (var ms = new MemoryStream())
                {
                    using (var xtw = new XmlTextWriter(ms, encoding))
                    {
                        xtw.Formatting = Formatting.Indented;
                        _serializer.Serialize(xtw, obj);
                        //rewind
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms, encoding))
                        {
                            result = reader.ReadToEnd();
                            xtw.Close();
                            reader.Close();
                        }
                    }
                }
            }
            return result;
        }

        public string Serialize(T obj)
        {
            string result = null;
            if (!Equals(obj, default(T)))
            {
                using (var ms = new MemoryStream())
                {
                    using (var xtw = new XmlTextWriter(ms, Encoding.UTF8))
                    {
                        xtw.Formatting = Formatting.Indented;
                        _serializer.Serialize(xtw, obj);
                        //rewind
                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                            xtw.Close();
                            reader.Close();
                        }
                    }
                }
            }
            return result;
        }

        public T Deserialize(string xml)
        {
            if (!String.IsNullOrEmpty(xml))
            {
                using (var sr = new StringReader(xml))
                {
                    return (T)_serializer.Deserialize(sr);
                }
            }
            return default(T);
        }
    }
}