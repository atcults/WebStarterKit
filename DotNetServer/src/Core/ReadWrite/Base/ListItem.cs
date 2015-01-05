using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.ReadWrite.Base
{
    /// <summary>
    ///     Provides list datastructure datatype to the application. Typically used when each list item has behaviour
    ///     associated with it.
    /// </summary>
    public abstract class ListItem : IComparable
    {
        protected ListItem()
        {
        }

        protected ListItem(int value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        public int Value { get; private set; }

        public string DisplayName { get; private set; }

        public virtual int CompareTo(object other)
        {
            return Value.CompareTo(((ListItem) other).Value);
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static IEnumerable<T> GetAll<T>() where T : ListItem, new()
        {
            var type = typeof (T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return (from info in fields let instance = new T() select info.GetValue(instance)).OfType<T>();
        }

        public static IEnumerable<ListItem> GetAll(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return
                (from info in fields
                    let instance = Activator.CreateInstance(type)
                    select (ListItem) info.GetValue(instance)).ToArray();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as ListItem;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static T FromValue<T>(int value) where T : ListItem, new()
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : ListItem, new()
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : ListItem, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof (T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public static ListItem FromValueOrDefault(Type listItemType, int listItemValue)
        {
            return GetAll(listItemType).SingleOrDefault(e => e.Value == listItemValue);
        }

        public static ListItem FromDisplayNameOrDefault(Type listItemType, string displayName)
        {
            return GetAll(listItemType).SingleOrDefault(e => e.DisplayName == displayName);
        }
    }
}