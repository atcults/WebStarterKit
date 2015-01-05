using System;
using System.Reflection;

namespace Core.ViewOnly.Base
{
    internal class PocoColumn
    {
        public string ColumnName;
        public bool ForceToUtc;
        public bool NoSearch;
        public PropertyInfo PropertyInfo;

        public virtual void SetValue(object target, object val)
        {
            PropertyInfo.SetValue(target, val, null);
        }

        public virtual object GetValue(object target)
        {
            return PropertyInfo.GetValue(target, null);
        }

        public virtual object ChangeType(object val)
        {
            return Convert.ChangeType(val, PropertyInfo.PropertyType);
        }
    }
}