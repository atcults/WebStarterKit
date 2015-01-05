using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Common.Extensions
{
    internal class ObjectHelper
    {
        // Dictionary to store cached properites
        private static readonly IDictionary<string, PropertyInfo[]> PropertiesCache = new Dictionary<string, PropertyInfo[]>();
        // Help with locking
        private static readonly ReaderWriterLockSlim PropertiesCacheLock = new ReaderWriterLockSlim();
        /// <summary>
        /// Get an array of PropertyInfo for this type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>PropertyInfo[] for this type</returns>
        public static PropertyInfo[] GetCachedProperties<T>()
        {
            if (PropertiesCacheLock.TryEnterUpgradeableReadLock(100))
            {
                PropertyInfo[] props;
                try
                {
                    if (!PropertiesCache.TryGetValue(typeof(T).FullName, out props))
                    {
                        props = typeof(T).GetProperties();
                        if (PropertiesCacheLock.TryEnterWriteLock(100))
                        {
                            try
                            {
                                PropertiesCache.Add(typeof(T).FullName, props);
                            }
                            finally
                            {
                                PropertiesCacheLock.ExitWriteLock();
                            }
                        }
                    }
                }
                finally
                {
                    PropertiesCacheLock.ExitUpgradeableReadLock();
                }
                return props;
            }

            return typeof(T).GetProperties();
        }

        /// <summary>
        /// Return the current row in the reader as an object
        /// </summary>
        /// <param name="reader">The Reader</param>
        /// <param name="objectToReturnType">The type of object to return</param>
        /// <returns>Object</returns>
        public static T GetAs<T>(SqlDataReader reader)
        {
            // Create a new Object
            var newObjectToReturn = Activator.CreateInstance<T>();
            // Get all the properties in our Object
            var props = GetCachedProperties<T>();
            // For each property get the data from the reader to the object
            var columnList = GetColumnList(reader);
            
            foreach (var t in props.Where(t => columnList.Contains(t.Name) && reader[t.Name] != DBNull.Value))
            {
                typeof(T).InvokeMember(t.Name, BindingFlags.SetProperty, null, newObjectToReturn, new[] { reader[t.Name] });
            }
            
            return newObjectToReturn;
        }

        /// <summary>
        /// Return a list from the current reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader</param>
        /// <returns></returns>
        public static List<T> GetAsList<T>(SqlDataReader reader)
        {
            var objetList = new List<T>();
            // Get all the properties in our Object
            var props = GetCachedProperties<T>();
            // For each property get the data from the reader to the object
            var columnList = GetColumnList(reader);
            while (reader.Read())
            {
                // Create a new Object
                var newObjectToReturn = Activator.CreateInstance<T>();
                foreach (var t in props.Where(t => columnList.Contains(t.Name) && reader[t.Name] != DBNull.Value))
                {
                    typeof(T).InvokeMember(t.Name, BindingFlags.SetProperty, null, newObjectToReturn, new Object[] { reader[t.Name] });
                }
                
                objetList.Add(newObjectToReturn);
            }
            return objetList;
        }

        public static SqlParameter[] GetSqlParametersFromPublicProperties(object dataObject)
        {
            var type = dataObject.GetType();
            var props = type.GetProperties();
            var paramArray = new SqlParameter[props.Length];

            for (var i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType.Namespace.Equals("System.Collections.Generic"))
                    continue;
                var fieldValue = type.InvokeMember(props[i].Name, BindingFlags.GetProperty, null, dataObject, null);
                if (fieldValue == null) continue;
                var sqlParameter = new SqlParameter {ParameterName = "@" + props[i].Name, Value = fieldValue};
                paramArray[i] = sqlParameter;
            }

            return paramArray;
        }

        /// <summary>
        /// Get a list of column names from the reader
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <returns></returns>
        public static List<string> GetColumnList(SqlDataReader reader)
        {
            var columnList = new List<string>();
            var readerSchema = reader.GetSchemaTable();
            for (var i = 0; i < readerSchema.Rows.Count; i++)
                columnList.Add(readerSchema.Rows[i]["ColumnName"].ToString());
            return columnList;
        }
    }
}
