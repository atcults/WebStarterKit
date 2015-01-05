using System;
using Core.ViewOnly.Attribute;

namespace Core.ViewOnly.Base
{
    /// <summary>
    ///     Use by IMapper to override table bindings for an object
    /// </summary>
    public class ViewInfo
    {
        /// <summary>
        ///     The database view name
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        ///     Creates and populates a TableInfo from the attributes of a POCO
        /// </summary>
        /// <param name="t">The POCO type</param>
        /// <returns>A TableInfo instance</returns>
        public static ViewInfo FromPoco(Type t)
        {
            var viewInfo = new ViewInfo();

            // Get the table name
            var a = t.GetCustomAttributes(typeof (ViewNameAttribute), true);
            viewInfo.ViewName = a.Length == 0 ? t.Name : ((ViewNameAttribute) a[0]).Value;

            return viewInfo;
        }
    }
}