using System;
using System.Text;

namespace Common.Net.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class EncodingExtentions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String GetString(this Encoding encoding, Byte[] bytes)
        {
            return encoding.GetString(bytes, 0, bytes.Length);
        }
    }
}
