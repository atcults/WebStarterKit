using System;
using System.IO;

namespace Common.Net.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Byte[] ToByteArray(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        internal static void CopyTo(this Stream source, Stream target)
        {
            var streamLength = new Byte[source.Length];
            source.Read(streamLength, 0, streamLength.Length);
            target.Write(streamLength, 0, streamLength.Length);
        }
    }
}
