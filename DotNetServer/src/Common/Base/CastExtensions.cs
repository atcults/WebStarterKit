using System;
using System.Globalization;

namespace Common.Base
{
    /// <summary>
    /// It is a class that provides a method for Extension of type conversion.
    /// </summary>
    public static class CastExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SByte? ToSByte(this Object value)
        {
            return Cast.ToSByte(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static SByte ToSByte(this Object value, SByte defaultValue)
        {
            return Cast.ToSByte(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static SByte ToSByte(this Object value, SByte defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToSByte(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16? ToInt16(this Object value)
        {
            return Cast.ToInt16(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this Object value, Int16 defaultValue)
        {
            return Cast.ToInt16(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this Object value, Int16 defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToInt16(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32? ToInt32(this Object value)
        {
            return Cast.ToInt32(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this Object value, Int32 defaultValue)
        {
            return Cast.ToInt32(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this Object value, Int32 defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToInt32(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64? ToInt64(this Object value)
        {
            return Cast.ToInt64(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this Object value, Int64 defaultValue)
        {
            return Cast.ToInt64(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this Object value, Int64 defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToInt64(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Byte? ToByte(this Object value)
        {
            return Cast.ToByte(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Byte ToByte(this Object value, Byte defaultValue)
        {
            return Cast.ToByte(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static Byte ToByte(this Object value, Byte defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToByte(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt16? ToUInt16(this Object value)
        {
            return Cast.ToUInt16(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(this Object value, UInt16 defaultValue)
        {
            return Cast.ToUInt16(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static UInt16 ToUInt16(this Object value, UInt16 defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToUInt16(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32? ToUInt32(this Object value)
        {
            return Cast.ToUInt32(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this Object value, UInt32 defaultValue)
        {
            return Cast.ToUInt32(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this Object value, UInt32 defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToUInt32(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt64? ToUInt64(this Object value)
        {
            return Cast.ToUInt64(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this Object value, UInt64 defaultValue)
        {
            return Cast.ToUInt64(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this Object value, UInt64 defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToUInt64(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Single? ToSingle(this Object value)
        {
            return Cast.ToSingle(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Single ToSingle(this Object value, Single defaultValue)
        {
            return Cast.ToSingle(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static Single ToSingle(this Object value, Single defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToSingle(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double? ToDouble(this Object value)
        {
            return Cast.ToDouble(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Double ToDouble(this Object value, Double defaultValue)
        {
            return Cast.ToDouble(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static Double ToDouble(this Object value, Double defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToDouble(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal? ToDecimal(this Object value)
        {
            return Cast.ToDecimal(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this Object value, Decimal defaultValue)
        {
            return Cast.ToDecimal(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="numberStyle"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this Object value, Decimal defaultValue, NumberStyles numberStyle, IFormatProvider formatProvider)
        {
            return Cast.ToDecimal(value, defaultValue, numberStyle, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean? ToBoolean(this Object value)
        {
            return Cast.ToBoolean(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Boolean ToBoolean(this Object value, Boolean defaultValue)
        {
            return Cast.ToBoolean(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid? ToGuid(this Object value)
        {
            return Cast.ToGuid(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Guid ToGuid(this Object value, Guid defaultValue)
        {
            return Cast.ToGuid(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TimeSpan? ToTimeSpan(this Object value)
        {
            return Cast.ToTimeSpan(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this Object value, TimeSpan defaultValue)
        {
            return Cast.ToTimeSpan(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this Object value)
        {
            return Cast.ToDateTime(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Object value, DateTime defaultValue)
        {
            return Cast.ToDateTime(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="formatProvider"></param>
        /// <param name="dateTimeStyle"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Object value, DateTime defaultValue, IFormatProvider formatProvider, DateTimeStyles dateTimeStyle)
        {
            return Cast.ToDateTime(value, defaultValue, formatProvider, dateTimeStyle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTimeOffset? ToDateTimeOffset(this Object value)
        {
            return Cast.ToDateTimeOffset(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(this Object value, DateTimeOffset defaultValue)
        {
            return Cast.ToDateTimeOffset(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="formatProvider"></param>
        /// <param name="dateTimeStyle"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(this Object value, DateTimeOffset defaultValue, IFormatProvider formatProvider, DateTimeStyles dateTimeStyle)
        {
            return Cast.ToDateTimeOffset(value, defaultValue, formatProvider, dateTimeStyle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToStringOrNull(this Object value)
        {
            return Cast.ToStringOrNull(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToStringOrNull(this String value)
        {
            return Cast.ToStringOrNull(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="inIsCastEmptyToNull"></param>
        /// <returns></returns>
        public static String ToStringOrNull(this String value, Boolean inIsCastEmptyToNull)
        {
            return Cast.ToStringOrNull(value, inIsCastEmptyToNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToStringOrEmpty(this Object value)
        {
            return Cast.ToStringOrEmpty(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Object ToDbValue(this Object value)
        {
            return Cast.ToDbValue(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? ToEnum<T>(this Object value) where T : struct
        {
            return Cast.ToEnum<T>(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this Object value, T defaultValue) where T : struct
        {
            return Cast.ToEnum(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="inIgnoreCase"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this Object value, T defaultValue, Boolean inIgnoreCase) where T : struct
        {
            return Cast.ToEnum(value, defaultValue, inIgnoreCase);
        }

        /// <summary>
        /// If the value is less than the minimum value, I will return the minimum value. I will return the value if it is greater.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Value to validate</param>
        /// <param name="minValue">Minimum value</param>
        /// <returns></returns>
        public static T EnsureMinValue<T>(this T value, T minValue) where T : struct
        {
            return Operator<T>.LessThan(value, minValue) ? minValue : value;
        }

        /// <summary>
        /// Value is greater than a maximum value, the maximum value is returned. If it is smaller returns the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Value to validate</param>
        /// <param name="maxValue">The carvings are worth seeing</param>
        /// <returns></returns>
        public static T EnsureMaxValue<T>(this T value, T maxValue) where T : struct
        {
            return Operator<T>.GreaterThan(value, maxValue) ? maxValue : value;
        }

        /// <summary>
        /// Value is less than the minimum value, the minimum value is returned. Value is greater than a maximum value, the maximum value is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Value to validate</param>
        /// <param name="minValue">The carvings are worth seeing.</param>
        /// <param name="maxValue">The carvings are worth seeing.</param>
        /// <returns></returns>
        public static T EnsureRange<T>(this T value, T minValue, T maxValue) where T : struct
        {
            if (Operator.GreaterThan(minValue, maxValue))
            { throw new ArgumentException("maxValue must be greater than minValue"); }
            return value.EnsureMinValue(minValue).EnsureMaxValue(maxValue);
        }
    }
}
