using System;
using System.Globalization;

namespace Common.Net.Core
{
    /// <summary>
    /// This class represent extension of ascii char code
    /// </summary>
    public static class AsciiCharCodeExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="charCode"></param>
        /// <returns></returns>
        public static Byte GetNumber(this AsciiCharCode charCode)
        {
            return (Byte)charCode;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="charCode"></param>
        /// <returns></returns>
        public static Char GetChar(this AsciiCharCode charCode)
        {
            return (Char)charCode.GetNumber();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charCode"></param>
        /// <returns></returns>
        public static String GetString(this AsciiCharCode charCode)
        {
            return GetChar(charCode).ToString(CultureInfo.InvariantCulture);
        }
    }
}