using System;
using System.Xml.Serialization;

namespace Common.Extensions
{
    public static class XmlEnumExtension
    {
        public static string ReadXmlEnumAttribute(this Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
            var attribs = (XmlEnumAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (XmlEnumAttribute), true);
            return attribs.Length > 0 ? attribs[0].Name : value.ToString();
        }

        public static T ParseXmlEnumAttribute<T>(this string str)
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                var attribs = (XmlEnumAttribute[])item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(XmlEnumAttribute), true);
                if(attribs.Length > 0 && attribs[0].Name.Equals(str)) return item;
            }
            return (T)Enum.Parse(typeof(T), str, true);
        }
    }
}