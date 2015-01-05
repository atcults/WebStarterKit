using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Core.ReadWrite
{
    /// <summary>
    ///     Data Record extension methods.
    ///     This class shorten the length of sentex to make code beautiful.
    /// </summary>
    public static class DataRecordExtention
    {
        public static Guid ReadUid(this IDataRecord dataRecord, string columnName)
        {
            return dataRecord.GetGuid(dataRecord.GetOrdinal(columnName));
        }

        public static Guid? ReadNullSafeUid(this IDataRecord dataRecord, string columnName)
        {
            return dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName))
                ? (Guid?) null
                : dataRecord.GetGuid(dataRecord.GetOrdinal(columnName));
        }

        public static string ReadNullSafeString(this IDataRecord dataRecord, string columnName)
        {
            return dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName))
                ? null
                : dataRecord.GetString(dataRecord.GetOrdinal(columnName));
        }

        public static int ReadInt(this IDataRecord dataRecord, string columnName)
        {
            return dataRecord.GetInt32(dataRecord.GetOrdinal(columnName));
        }

        public static int? ReadNullSafeInt(this IDataRecord dataRecord, string columnName)
        {
            return dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName))
                ? (int?) null
                : dataRecord.GetInt32(dataRecord.GetOrdinal(columnName));
        }

        public static bool ReadNullSafeBool(this IDataRecord dataRecord, string columnName)
        {
            return !dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName)) &&
                   dataRecord.GetBoolean(dataRecord.GetOrdinal(columnName));
        }

        public static byte? ReadNullSafeByte(this IDataRecord dataRecord, string columnName)
        {
            return dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName))
               ? (byte?)null
               : dataRecord.GetByte(dataRecord.GetOrdinal(columnName));
        }

        public static DateTime? ReadNullSafeDateTime(this IDataRecord dataRecord, string columnName)
        {
            return dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName))
                ? (DateTime?) null
                : dataRecord.GetDateTime(dataRecord.GetOrdinal(columnName));
        }

        public static decimal? ReadNullSafeDecimal(this IDataRecord dataRecord, string columnName)
        {
            return dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName))
                ? (decimal?) null
                : dataRecord.GetDecimal(dataRecord.GetOrdinal(columnName));
        }

        public static byte[] ReadBytes(this IDataRecord dataRecord, string columnName)
        {
            var bytes = dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName))
                ? null
                : dataRecord.GetValue(dataRecord.GetOrdinal(columnName));

            if (bytes == null) return null;
            var binaryFormatter = new BinaryFormatter();
            var serializationStream = new MemoryStream();
            binaryFormatter.Serialize(serializationStream, bytes);
            return serializationStream.ToArray();
        }

        public static string ReadBytesAsBase64(this IDataRecord dataRecord, string columnName)
        {
            var bytes = dataRecord.IsDBNull(dataRecord.GetOrdinal(columnName))
                ? null
                : dataRecord.GetValue(dataRecord.GetOrdinal(columnName));

            if (bytes == null) return null;
            var binaryFormatter = new BinaryFormatter();
            var serializationStream = new MemoryStream();
            binaryFormatter.Serialize(serializationStream, bytes);
            serializationStream.ToArray();

            var fileBytesToString = Encoding.ASCII.GetString(serializationStream.ToArray());
            var fileBase64String =
                fileBytesToString.Substring(fileBytesToString.IndexOf("data", StringComparison.Ordinal));
            return fileBase64String.Remove(fileBase64String.Length - 1);
        }
    }
}