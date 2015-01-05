using System;
using System.Globalization;
using System.Linq;

namespace Common.EncodingHelper
{
    public class HexEncoding
    {
        public static int GetByteCount(string hexString)
        {
            var numHexChars = hexString.Count(IsHexDigit);
            // remove all none A-F, 0-9, characters
            // if odd Number of characters, discard last character
            if (numHexChars%2 != 0)
            {
                numHexChars--;
            }
            return numHexChars/2; // 2 characters per byte
        }

        /// <summary>
        /// Creates a byte array from the hexadecimal string. Each two characters are combined
        /// to create one byte. First two hexadecimal characters become first byte in returned array.
        /// Non-hexadecimal characters are ignored. 
        /// </summary>
        /// <param name="hexString">string to convert to byte array</param>
        /// <param name="discarded">Number of characters in string ignored</param>
        /// <returns>byte array, in the same left-to-right order as the hexString</returns>
        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            var newString = "";
            // remove all none A-F, 0-9, characters
            foreach (var c in hexString)
            {
                if (IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }
            // if odd Number of characters, discard last character
            if (newString.Length%2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            var byteLength = newString.Length/2;
            var bytes = new byte[byteLength];
            var j = 0;
            for (var i = 0; i < bytes.Length; i++)
            {
                var hex = new String(new[] {newString[j], newString[j + 1]});
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }

        public static string ToString(byte[] bytes, int length = 0)
        {
            var hexString = "";

			if(length == 0) length = bytes.Length;

            for (var i = 0; i < length; i++)
            {
                hexString += bytes[i].ToString("X2");
            }
            return hexString;
        }

        /// <summary>
        /// Determines if given string is in proper hexadecimal string format
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static bool InHexFormat(string hexString)
        {
            return hexString.All(IsHexDigit);
        }

        /// <summary>
        /// Returns true is c is a hexadecimal digit (A-F, a-f, 0-9)
        /// </summary>
        /// <param name="c">Character to test</param>
        /// <returns>true if hex digit, false if not</returns>
        public static bool IsHexDigit(Char c)
        {
            var numA = Convert.ToInt32('A');
            var num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);
            var numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6)) return true;
            return numChar >= num1 && numChar < (num1 + 10);
        }

        /// <summary>
        /// Converts 1 or 2 character string into equivalant byte value
        /// </summary>
        /// <param name="hex">1 or 2 character string</param>
        /// <returns>byte</returns>
        private static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            var newByte = byte.Parse(hex, NumberStyles.HexNumber);
            return newByte;
        }
    }
}