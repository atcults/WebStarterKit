using System;

namespace Core.ReadWrite.Attribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityNameAttribute : System.Attribute
    {
        public EntityNameAttribute(string entityName)
        {
            Value = entityName;
        }

        public string Value { get; private set; }
    }
}