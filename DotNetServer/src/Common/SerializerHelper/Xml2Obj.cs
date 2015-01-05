using System.IO;
using System.Xml.Serialization;
using Common.SystemSettings;

namespace Common.SerializerHelper
{
    public static class Xml2Obj<T>
    {
        public static T Load()
        {
            return Load(Globals.ConfigFolder + typeof(T).Name + ".xml");
        }

        public static T Load(string xmlPath)
        {
            if (!File.Exists(xmlPath))
            {
                var config = System.Activator.CreateInstance<T>();
                Save(config);
            }

            var deSerializer = new XmlSerializer(typeof (T));
            TextReader reader = new StreamReader(xmlPath);
            var temp = (T) deSerializer.Deserialize(reader);
            reader.Close();
            return temp;
        }

        public static void Save(string xmlPath, T settings)
        {
            var serializer = new XmlSerializer(typeof (T));
            TextWriter writer = new StreamWriter(xmlPath);
            serializer.Serialize(writer, settings);
            writer.Close();
        }

        public static void Save(T settings)
        {
            Save(Globals.ConfigFolder + typeof (T).Name + ".xml", settings);
        }

        public static bool IsExist()
        {
            return File.Exists(Globals.ConfigFolder + typeof(T).Name + ".xml");
        }
    }
}