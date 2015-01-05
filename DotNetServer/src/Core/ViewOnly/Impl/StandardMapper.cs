using System;
using System.Reflection;
using Core.ViewOnly.Base;

namespace Core.ViewOnly.Impl
{
    /// <summary>
    ///     StandardMapper is the default implementation of IMapper used by PetaPoco
    /// </summary>
    public class StandardMapper : IMapper
    {
        /// <summary>
        ///     Constructs a TableInfo for a POCO by reading its attribute data
        /// </summary>
        /// <param name="pocoType">The POCO Type</param>
        /// <returns></returns>
        public ViewInfo GetViewInfo(Type pocoType)
        {
            return ViewInfo.FromPoco(pocoType);
        }

        /// <summary>
        ///     Constructs a ColumnInfo for a POCO property by reading its attribute data
        /// </summary>
        /// <param name="pocoProperty"></param>
        /// <returns></returns>
        public ColumnInfo GetColumnInfo(PropertyInfo pocoProperty)
        {
            return ColumnInfo.FromProperty(pocoProperty);
        }

        public Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType)
        {
            return null;
        }

        public Func<object, object> GetToDbConverter(PropertyInfo sourceProperty)
        {
            return null;
        }
    }
}