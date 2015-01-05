using System;

namespace Core.ViewOnly.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewNameAttribute : System.Attribute
    {
        public ViewNameAttribute(string tableName)
        {
            Value = tableName;
        }

        public string Value { get; private set; }
    }
}