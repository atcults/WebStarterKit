using System;

namespace Common.Base
{
    ///<summary>
    /// This library provides serialization by using Cast library
    ///</summary>
    public static class SerializeExtensions
    {
        /// <summary>
        /// Gets a string in XML format from the specified object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToXml(this Object value)
        {
            return Cast.ToXml(value);
        }

        /// <summary>
        /// Create an object from a string of XML format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlText"></param>
        /// <returns></returns>
        public static T FromXml<T>(this String xmlText)
        {
            return Cast.FromXml<T>(xmlText);
        }
    }
}
