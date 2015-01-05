using System.Collections.Generic;

namespace Core.ViewOnly.Base
{
    internal class ExpandoColumn : PocoColumn
    {
        public override void SetValue(object target, object val)
        {
            ((IDictionary<string, object>) target)[ColumnName] = val;
        }

        public override object GetValue(object target)
        {
            object val;
            ((IDictionary<string, object>) target).TryGetValue(ColumnName, out val);
            return val;
        }

        public override object ChangeType(object val)
        {
            return val;
        }
    }
}