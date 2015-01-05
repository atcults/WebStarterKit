using System;

namespace Common.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Enum<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Parse(string value)
        {
            return Parse(value, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T Parse(string value, bool ignoreCase)
        {
            var tp = CheckEnumType();
            return (T)Enum.Parse(tp, value, ignoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="returnedValue"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out T returnedValue)
        {
            return TryParse(value, true, out returnedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="returnedValue"></param>
        /// <returns></returns>
        public static bool TryParse(string value, bool ignoreCase, out T returnedValue)
        {
            var tp = CheckEnumType();
            try
            {
                returnedValue = (T)Enum.Parse(tp, value, ignoreCase);
                return true;
            }
            catch
            {
                returnedValue = default(T);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static T[] GetValues()
        {
            var tp = CheckEnumType();
            return (T[])Enum.GetValues(tp);
        }

        private static Type CheckEnumType()
        {
            var tp = typeof(T);
            if (tp.IsEnum == false) { throw new ArgumentException("T must be Enum type."); }
            return tp;
        }
    }
}
