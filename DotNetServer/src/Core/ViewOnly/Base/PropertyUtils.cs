using System;
using System.Linq;

namespace Core.ViewOnly.Base
{
    public static class PropertyUtils
    {
        /// --------------------------------------------------------------------
        /// <summary>
        ///     Determine if a property exists in an object
        /// </summary>
        /// <param name="propertyName">Name of the property </param>
        /// <param name="srcObject">the object to inspect</param>
        /// <param name="ignoreCase">ignore case sensitivity</param>
        /// <returns>true if the property exists, false otherwise</returns>
        /// <exception cref="ArgumentNullException">if srcObject is null</exception>
        /// <exception cref="ArgumentException">if propertName is empty or null </exception>
        /// --------------------------------------------------------------------
        public static bool Exists(string propertyName, object srcObject, bool ignoreCase)
        {
            if (srcObject == null)
                throw new ArgumentNullException("srcObject");

            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property name cannot be empty or null.");

            if (!ignoreCase)
            {
                var propInfoSrcObj = srcObject.GetType().GetProperty(propertyName);
                return (propInfoSrcObj != null);
            }

            var propertyInfos = srcObject.GetType().GetProperties();
            propertyName = propertyName.ToLower();
            return propertyInfos.Any(propInfo => propInfo.Name.ToLower().Equals(propertyName));
        }
    }
}