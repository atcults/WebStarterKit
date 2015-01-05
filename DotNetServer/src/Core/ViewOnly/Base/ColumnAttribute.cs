using System;

namespace Core.ViewOnly.Base
{
    /// <summary>
    ///     For explicit poco properties, marks the property as a column and optionally
    ///     supplies the DB column name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : System.Attribute
    {
        public ColumnAttribute(string name, bool ignore = false, bool noSearch = false, bool forceToUtc = false)
        {
            Name = name;
            Ignore = ignore;
            NoSearch = noSearch;
            ForceToUtc = forceToUtc;
        }

        public string Name { get; set; }

        public bool Ignore { get; set; }

        public bool NoSearch { get; set; }

        public bool ForceToUtc { get; set; }
    }
}