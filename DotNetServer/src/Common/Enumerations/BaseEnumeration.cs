using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Common.Base;

namespace Common.Enumerations
{
    public abstract class BaseEnumeration
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }

        public static IEnumerable<BaseEnumeration> GetAll(Type enumerationType)
        {
            var fields = enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            var enumerations = fields.Select(f => f.GetValue(null)).Cast<BaseEnumeration>();

            return enumerations;
        }

        public static object GetDbNullSafe(BaseEnumeration enumeration)
        {
            return enumeration != null ? (object)enumeration.Value : DBNull.Value;
        }
    }

    [Serializable]
    [DebuggerDisplay("{DisplayName} - {Value} - {DisplayOrder}")]
    public abstract class Enumeration<TEnumeration> : BaseEnumeration, IComparable<TEnumeration>, IEquatable<TEnumeration>
        where TEnumeration : Enumeration<TEnumeration>
    {
        private static readonly Lazy<TEnumeration[]> Enumerations = new Lazy<TEnumeration[]>(GetEnumerations);

        protected Enumeration(string value, string displayName, int displayOrder)
        {
            Value = value;
            DisplayName = displayName;
            DisplayOrder = displayOrder;
        }

        public virtual int CompareTo(TEnumeration other)
        {
            return String.Compare(Value, other.Value, StringComparison.Ordinal);
        }

        public override sealed string ToString()
        {
            return DisplayName;
        }

        public virtual int OrderTo(TEnumeration other)
        {
            return DisplayOrder.CompareTo(other.DisplayOrder);
        }

        public static TEnumeration[] GetAll(bool sorted = true)
        {
            var all = Enumerations.Value;
            return sorted ? all.OrderBy(x => x.DisplayOrder).ThenBy(x => x.DisplayName).ToArray() : all;
        }

        private static TEnumeration[] GetEnumerations()
        {
            var enumerationType = typeof(TEnumeration);
            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<TEnumeration>()
                .ToArray();
        }

        public static IdNamePair[] GetAllPaired()
        {
            return GetAll().OrderBy(x => x.DisplayOrder).Select(x => new IdNamePair
            {
                Id = x.Value.Trim(),
                Name = x.DisplayName
            }).ToArray();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }

        public bool Equals(TEnumeration other)
        {
            return other != null && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static TEnumeration FromValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var val = value.Trim();
            return Parse(value, "value", item => item.Value == val);
        }

        public static TEnumeration FromDisplay(string displayName)
        {
            var val = displayName.Trim();
            return string.IsNullOrEmpty(val) ? null : Parse(displayName, "displayName", item => item.DisplayName == val);
        }

        public static TEnumeration FromDisplayOrder(int displayOrder)
        {
            return Parse(displayOrder, "displayOrder", item => item.DisplayOrder == displayOrder);
        }

        public static bool IsValid(string value)
        {
            TEnumeration result;
            return TryParse(item => item.Value == value, out result);
        }

        public static bool IsInvalid(string value)
        {
            return !IsValid(value);
        }

        private static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate)
        {
            TEnumeration result;
            if (TryParse(predicate, out result)) return result;
            var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(TEnumeration));
            Logger.Log(LogType.Error, typeof(BaseEnumeration), message);
            return null;
        }

        private static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            result = GetAll().FirstOrDefault(predicate);
            return result != null;
        }
        
    }
}