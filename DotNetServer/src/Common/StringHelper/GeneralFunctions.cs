using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.StringHelper
{
    public class GeneralFunctions
    {
        /// <summary>
        /// Strips the HTML.
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <param name="htmlPlaceHolder">The HTML place holder.</param>
        /// <returns></returns>
        public static string StripHtml(string htmlString, string htmlPlaceHolder)
        {
            const string pattern = @"<(.|\n)*?>";
            var sOut = Regex.Replace(htmlString, pattern, htmlPlaceHolder);
            sOut = sOut.Replace("&nbsp;", "");
            sOut = sOut.Replace("&amp;", "&");
            sOut = sOut.Replace("&gt;", ">");
            sOut = sOut.Replace("&lt;", "<");
            return sOut;
        }

        /// <summary>
        /// Parses the camel cased string to proper.
        /// </summary>
        /// <param name="sIn">The s in.</param>
        /// <returns></returns>
        public static string ParseCamelToProper(string sIn)
        {
            var letters = sIn.ToCharArray();
            var sOut = "";
            foreach (var c in letters)
            {
                if (c.ToString(CultureInfo.InvariantCulture) != c.ToString(CultureInfo.InvariantCulture).ToLower())
                {
                    //it's uppercase, add a space
                    sOut += " " + c;
                }
                else
                {
                    sOut += c.ToString(CultureInfo.InvariantCulture);
                }
            }
            return sOut;
        }

        /// <summary>
        /// Generates a random string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var randomString = string.Empty;
            if (length > 0)
            {
                var rnd = new Random();
                for (var i = 0; i < length; i++)
                {
                    randomString += chars[rnd.Next(chars.Length)];
                }
            }
            return randomString;
        }

        /// <summary>
        /// Generates a 4 by 4 masked string.
        /// </summary>
        /// <returns></returns>
        public static string Generate4By4MaskedString()
        {
            const string chars = "1234567890";
            var randomString = string.Empty;
            var rnd = new Random();
            for (var i = 1; i <= 16; i++)
            {
                randomString += chars[rnd.Next(chars.Length)];
                if (i < 16 && i % 4 == 0)
                {
                    randomString += "-";
                }
            }
            return randomString;
        }

        public static string ConvertToString(UInt16[] data)
        {
            var s = string.Empty;
            int count;
            for (count = 0; count < data.Length && data[count] != 0; count++)
                s += (char) data[count];
            return s;
        }

        public static string ConvertToString(byte[] data)
        {
            var s = string.Empty;
            int count;
            for (count = 0; count < data.Length && data[count] != 0; count++)
                s += (char) data[count];
            return s;
        }

        public static byte[] StringToByteArray(string str)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        public string Num2Str(string value)
        {
            string[] connectors = {"Rupies", "And", "Paisa", "Only"};

            var dValue = Convert.ToDouble(value);
            var iFirst = (int) dValue;
            var strValue = IntegerToWords(iFirst) + " " + connectors[0];
            var iSecond = (int) ((dValue - iFirst)*100);
            if (iSecond != 0)
            {
                strValue += " " + connectors[1] + " " + IntegerToWords(iSecond) + " " + connectors[2];
            }
            strValue += " " + connectors[3];
            return strValue;
        }

        private static string IntegerToWords(int iValue)
        {
            string[] groupPrefix = {"", "Hundread", "Thousand", "Lack", "Crore"};

            var result = string.Empty;

            if (iValue == 0)
            {
                result = "Zero";
            }

            var index = 0;
            while (iValue > 0)
            {
                switch (index)
                {
                    case 0:
                        {
                            var newiValue = iValue/100;
                            var itValue = iValue - newiValue*100;
                            iValue = newiValue;
                            if (itValue != 0)
                            {
                                result = TwoDigitIntegerToWords(itValue) + " " + result;
                            }
                        }
                        break;
                    case 1:
                        {
                            var newiValue = iValue/10;
                            var itValue = iValue - newiValue*10;
                            iValue = newiValue;
                            if (itValue != 0)
                            {
                                result = TwoDigitIntegerToWords(itValue) + " " + groupPrefix[index] + " " + result;
                            }
                        }
                        break;
                    default:
                        {
                            var newiValue = iValue/100;
                            var itValue = iValue - newiValue*100;
                            iValue = newiValue;
                            if (itValue != 0)
                            {
                                result = TwoDigitIntegerToWords(itValue) + " " + groupPrefix[index] + " " + result;
                            }
                        }
                        break;
                }
                index++;
            }
            return result;
        }

        private static string TwoDigitIntegerToWords(int iValue)
        {
            string[] digit = {
                                 "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                                 "Eveven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen",
                                 "Eighteen", "Nineteen"
                             };
            string[] tens = {"", "", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"};
            var result = string.Empty;

            if (iValue == 0)
            {
                result = "Zero";
            }
            else if (iValue < 20)
            {
                result += digit[iValue];
            }
            else
            {
                result += tens[iValue/10] + " " + digit[iValue - (iValue/10)*10];
            }
            return result;
        }

        public static object[] BuildObjectArray(params object[] ob)
        {
            return ob;
        }

        public static double GetTotalFromTableColumn(DataTable dt, string member)
        {
            return dt.Rows.Cast<DataRow>().Sum(dr => Convert.ToDouble(dr[member]));
        }
    }
}