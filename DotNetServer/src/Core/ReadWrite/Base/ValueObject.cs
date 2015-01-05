using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.ReadWrite.Base
{
    public interface IAmValueObject
    {
    }

    [Serializable]
    public abstract class ValueObject<T> : IAmValueObject, IEquatable<T> where T : class
    {
        public virtual bool Equals(T other)
        {
            if (other == null)
                return false;

            var t = GetType();

            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var value1 = field.GetValue(other);
                var value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else if (value2 == null)
                {
                    return false;
                }
                else if ((typeof (DateTime).IsAssignableFrom(field.FieldType)) ||
                         ((typeof (DateTime?).IsAssignableFrom(field.FieldType))))
                {
                    var dateString1 = ((DateTime) value1).ToLongDateString();
                    var dateString2 = ((DateTime) value2).ToLongDateString();
                    if (!dateString1.Equals(dateString2))
                    {
                        return false;
                    }
                }
                else if (!value1.Equals(value2))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as T;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            var fields = GetFields();

            const int startValue = 17;
            const int multiplier = 59;

            return fields.Select(field => field.GetValue(this))
                .Where(value => value != null)
                .Aggregate(startValue, (current, value) => current*multiplier + value.GetHashCode());
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var t = GetType();

            var fields = new List<FieldInfo>();

            while (t != typeof (object))
            {
                fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

                t = t.BaseType;
            }

            return fields;
        }

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
        {
            return ReferenceEquals(x, null) ? ReferenceEquals(y, null) : x.Equals(y);
        }

        public static bool operator !=(ValueObject<T> x, ValueObject<T> y)
        {
            return !(x == y);
        }
    }
}