using System;
using System.Xml.Linq;

namespace Common.Net.Extensions
{
    /// <summary>
    /// This class Defines xml parser extension
    /// </summary>
    public static class XmlParserExtensions
    {
        /// <summary>
        /// Casting elment to string.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String CastElementToString(this XElement element, String key)
        {
            if (element.Element(key) == null) { return ""; }
            var xElement = element.Element(key);
            if (xElement != null) return xElement.Value;
            return null;
        }

        /// <summary>
        /// Casting element to boolean.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Boolean? CastElementToBoolean(this XElement element, String key)
        {
            Boolean x ;
            if (element.Element(key) == null) { return null; }
            var xElement = element.Element(key);
            if (xElement != null && Boolean.TryParse(xElement.Value, out x))
            {
                return x;
            }
            return null;
        }

        /// <summary>
        /// Casting element to int32.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Int32? CastElementToInt32(this XElement element, String key)
        {
            Int32 x ;
            if (element.Element(key) == null) { return null; }
            var xElement = element.Element(key);
            if (xElement != null && Int32.TryParse(xElement.Value, out x))
            {
                return x;
            }
            return null;
        }

        /// <summary>
        /// Casting element to int64.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Int64? CastElementToInt64(this XElement element, String key)
        {
            Int64 x; 
            if (element.Element(key) == null) { return null; }
            var xElement = element.Element(key);
            if (xElement != null && Int64.TryParse(xElement.Value, out x))
            {
                return x;
            }
            return null;
        }

        /// <summary>
        /// Casting element to string
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String CastAttributeToString(this XElement element, String key)
        {
            if (element.Attribute(key) == null) { return ""; }
            return element.Attribute(key).Value;
        }

        /// <summary>
        /// Casting attribute to boolean.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Boolean? CastAttributeToBoolean(this XElement element, String key)
        {
            Boolean x ;
            if (element.Attribute(key) == null) { return null; }
            if (Boolean.TryParse(element.Attribute(key).Value, out x))
            {
                return x;
            }
            return null;
        }

        /// <summary>
        /// Casting attribute to int32.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Int32? CastAttributeToInt32(this XElement element, String key)
        {
            Int32 x;
            if (element.Attribute(key) == null) { return null; }
            if (Int32.TryParse(element.Attribute(key).Value, out x))
            {
                return x;
            }
            return null;
        }

        /// <summary>
        /// Casting attribute to int64.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Int64? CastAttributeToInt64(this XElement element, String key)
        {
            Int64 x;
            if (element.Attribute(key) == null) { return null; }
            if (Int64.TryParse(element.Attribute(key).Value, out x))
            {
                return x;
            }
            return null;
        }
    }
}
