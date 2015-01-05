using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Common.Net.Core;

namespace Common.Mail.Common
{
    /// Class for parse mail text.
    /// <summary>
    /// Class for parse mail text.
    /// </summary>
    public class MailParser
    {
        private struct RegexList
        {
            public static readonly Regex HexDecoder = new Regex("((\\=([0-9A-F][0-9A-F]))*)", RegexOptions.IgnoreCase);
            public static readonly Regex HexDecoder1 = new Regex("((%([0-9A-F][0-9A-F]))*)", RegexOptions.IgnoreCase);
            public static readonly Regex DecodeByRfc2047 = new Regex(@"[\s]{0,1}[=][\?](?<Encoding>[^?]+)[\?](?<BorQ>[B|b|Q|q])[\?](?<Value>[^?]+)[\?][=][\s]{0,1}");
            public static readonly Regex DecodeByRfc2231 = new Regex(@"(?<Encoding>[^']+)[\'](?<Language>[a-zA-z\-]*)[\'](?<Value>[^\s]+)");
            public static readonly Regex AsciiCharOnly = new Regex("[^\x00-\x7F]");
            public static readonly Regex ThreeLetterTimeZone = new Regex("(\\([^(].*\\))");
            public static readonly Regex TimeZone = new Regex("[+\\-][0-9][0-9][0-9][0-9]");
            public static readonly ICollection<Regex> ContentTypeBoundary = new List<Regex>();
            public static readonly ICollection<Regex> ContentTypeName = new List<Regex>();
            public static readonly ICollection<Regex> ContentDispositionFileName = new List<Regex>();
            public const String Rfc2231FormatText = @"[;\t\s]+{0}\*{1}=(?<Value>[^\n\r;]+)(;|$)";
            public const String Rfc2231FormatText1 = @"[;\t\s]+{0}\*{1}\*=(?<Value>[^\n\r;]+)(;|$)";

            static RegexList()
            {
                InitializeRegexList();
            }
            private static void InitializeRegexList()
            {
                ContentTypeBoundary.Add(new Regex(".*boundary=[\"]*(?<Value>[^\"]*).*", RegexOptions.IgnoreCase));
                ContentTypeName.Add(new Regex(".*name=[\"]*(?<Value>[^\"]*)[;\n\r]", RegexOptions.IgnoreCase));
                ContentTypeName.Add(new Regex(".*name=[\"]*(?<Value>[^\"]*).*", RegexOptions.IgnoreCase));
                ContentTypeName.Add(new Regex(@"[;\t\s]+name\*=(?<Value>[^\n\r]+).*", RegexOptions.IgnoreCase));
                ContentDispositionFileName.Add(new Regex("[;\t\\s]+filename=[\"]*(?<Value>[^\"]*)[;\n\r]", RegexOptions.IgnoreCase));
                ContentDispositionFileName.Add(new Regex("[;\t\\s]+filename=[\"]*(?<Value>[^\"]*).*", RegexOptions.IgnoreCase));
                ContentDispositionFileName.Add(new Regex("[;\t\\s]+filename\\*=[\"]*(?<Value>[^\"\n\r]+).*", RegexOptions.IgnoreCase));
            }
        }
        private static readonly Dictionary<string, Encoding> EncodingList = new Dictionary<string, Encoding>();
        private static TimeSpan _timeZoneOffset = DateTimeOffset.Now.Offset;
        private static String _dateTimeFormatString = "ddd, dd MMM yyyy HH:mm:ss +0000";
        ///This is multi-part message in MIME format to get the value of.
        /// <summary>
        /// This is multi-part message in MIME format to get the value of.
        /// </summary>
        public const String ThisIsMultiPartMessageInMimeFormat = "This is multi-part message in MIME format.";
        /// new line is the value of a string.
        /// <summary>
        /// new line is the value of a string.
        /// </summary>
        public const String NewLine = "\r\n";
        /// Format of the date string is a string that you want to configure.
        /// <summary>
        /// Format of the date string is a string that you want to configure.
        /// </summary>
        public static String DateTimeFormatString
        {
            get { return _dateTimeFormatString; }
        }

        /// The date string UTC to set offset from the get or set the value of.
        /// <summary>
        /// The date string UTC to set offset from the get or set the value of.
        /// This value can be changed by the DateTimeFormatString offset to the value can be changed.
        /// </summary>
        public static TimeSpan TimeZoneOffset
        {
            get { return _timeZoneOffset; }
            set
            {
                _timeZoneOffset = value;
                SetDateTimeFormatString();
            }
        }

        /// Maximum number of characters per line.
        /// <summary>
        /// Maximum number of characters per line.
        /// </summary>
        public const Int32 MaxCharCountPerRow = 76;
        static MailParser()
        {
            SetDateTimeFormatString();
            InitializeEncodingList();
        }

        private static void SetDateTimeFormatString()
        {
            _dateTimeFormatString = String.Format(_timeZoneOffset.TotalMilliseconds >= 0 ? "ddd, dd MMM yyyy HH:mm:ss +{0:00}{1:00}" : "ddd, dd MMM yyyy HH:mm:ss {0:00}{1:00}", _timeZoneOffset.Hours, _timeZoneOffset.Minutes);
        }

        private static void InitializeEncodingList()
        {
            var d = EncodingList;
            d["UTF7"] = Encoding.GetEncoding("UTF-7");
            d["UTF8"] = Encoding.UTF8;
            d["UTF32"] = Encoding.GetEncoding("utf-32");
            d["CP1252"] = Encoding.GetEncoding("windows-1252");
        }

        /// + OK response, including to get whether or not.
        /// <summary>
        /// + OK response, including to get whether or not.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Boolean IsResponseOk(String text)
        {
            return text.StartsWith("+OK", StringComparison.OrdinalIgnoreCase);
        }

        /// The From characters from a text string that can be used as destination email address to retrieve the string.
        /// <summary>
        /// The From characters from a text string that can be used as destination email address to retrieve the string.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static String MailAddress(String from)
        {
            var rg = new Regex("[<]{1}(?<MailAddress>[^>]+)[>]{1}");

            var m = rg.Match(@from);
            if (String.IsNullOrEmpty(m.Value))
            {
                return from;
            }
            return m.Groups["MailAddress"].Value;
        }

        /// Date from the data in the header of the email message that you want to use to generate the date string.
        /// <summary>
        /// Date from the data in the header of the email message that you want to use to generate the date string.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static String DateTimeOffsetString(DateTimeOffset dateTime)
        {
            return dateTime.ToString(DateTimeFormatString, new CultureInfo("en-US"));
        }

        /// This method has the following format.
        /// <summary>
        /// This method has the following format.
        /// Tue, 25 Oct 2011 20:44:24
        /// Tue, 25 Oct 2011 20:44:24 +0900
        /// Tue, 25 Oct 2011 20:44:24 +0900 (JST)
        /// Tue, 25 Oct 2011 20:44:24 F
        /// Tue, 25 Oct 2011 20:44:24 EDT
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(String dateTime)
        {
            DateTimeOffset dtime;
            var dateTimeTimeZone = dateTime;
            var ts = TimeSpan.Zero;

            if (DateTimeOffset.TryParse(dateTime, out dtime)) { return dtime; }

            var m = RegexList.ThreeLetterTimeZone.Match(dateTime);//(CST)
            if (m.Success)
            {
                dateTimeTimeZone = dateTime.Remove(m.Index, m.Length).TrimEnd();//Remove last (CST) string
            }
            // extract date and time
            var index = dateTimeTimeZone.LastIndexOf(" ", StringComparison.Ordinal);
            if (index < 1) throw new FormatException("probably not a date");
            var dateTimePart = dateTimeTimeZone.Substring(0, index - 1);//Tue, 25 Oct 2011 20:44:24
            var timeZonePart = dateTimeTimeZone.Substring(index + 1);//+0600 or GMT (Three letter military timezone)

            if (DateTimeOffset.TryParse(dateTimePart, out dtime) == false) { throw new FormatException(); }

            if (RegexList.TimeZone.IsMatch(timeZonePart))
            {
                var hour = Convert.ToInt32(timeZonePart.Substring(1, 2));
                var minute = Convert.ToInt32(timeZonePart.Substring(3, 2));
                if (timeZonePart.Substring(0, 1) == "-")
                {
                    hour = -hour;
                    minute = -minute;
                }
                ts = new TimeSpan(hour, minute, 0);
                dtime = new DateTimeOffset(dtime.DateTime, ts);
            }
            else
            {
                switch (timeZonePart)
                {
                    case "A": ts = new TimeSpan(1, 0, 0); break;
                    case "B": ts = new TimeSpan(2, 0, 0); break;
                    case "C": ts = new TimeSpan(3, 0, 0); break;
                    case "D": ts = new TimeSpan(4, 0, 0); break;
                    case "E": ts = new TimeSpan(5, 0, 0); break;
                    case "F": ts = new TimeSpan(6, 0, 0); break;
                    case "G": ts = new TimeSpan(7, 0, 0); break;
                    case "H": ts = new TimeSpan(8, 0, 0); break;
                    case "I": ts = new TimeSpan(9, 0, 0); break;
                    case "K": ts = new TimeSpan(10, 0, 0); break;
                    case "L": ts = new TimeSpan(11, 0, 0); break;
                    case "M": ts = new TimeSpan(12, 0, 0); break;
                    case "N": ts = new TimeSpan(-1, 0, 0); break;
                    case "O": ts = new TimeSpan(-2, 0, 0); break;
                    case "P": ts = new TimeSpan(-3, 0, 0); break;
                    case "Q": ts = new TimeSpan(-4, 0, 0); break;
                    case "R": ts = new TimeSpan(-5, 0, 0); break;
                    case "S": ts = new TimeSpan(-6, 0, 0); break;
                    case "T": ts = new TimeSpan(-7, 0, 0); break;
                    case "U": ts = new TimeSpan(-8, 0, 0); break;
                    case "V": ts = new TimeSpan(-9, 0, 0); break;
                    case "W": ts = new TimeSpan(-10, 0, 0); break;
                    case "X": ts = new TimeSpan(-11, 0, 0); break;
                    case "Y": ts = new TimeSpan(-12, 0, 0); break;
                    case "Z":
                    case "UT":
                    case "GMT": break;    // It's UTC
                    case "EST": ts = new TimeSpan(5, 0, 0); break;
                    case "EDT": ts = new TimeSpan(4, 0, 0); break;
                    case "CST": ts = new TimeSpan(6, 0, 0); break;
                    case "CDT": ts = new TimeSpan(5, 0, 0); break;
                    case "MST": ts = new TimeSpan(7, 0, 0); break;
                    case "MDT": ts = new TimeSpan(6, 0, 0); break;
                    case "PST": ts = new TimeSpan(8, 0, 0); break;
                    case "PDT": ts = new TimeSpan(7, 0, 0); break;
                    case "JST": ts = new TimeSpan(9, 0, 0); break;
                    default: throw new FormatException("invalid time zone");
                }
                dtime = new DateTimeOffset(dtime.DateTime, ts);
            }
            return dtime;
        }

        /// From the String to get the value of TransferEncoding.
        /// <summary>
        /// From the String to get the value of TransferEncoding.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static TransferEncoding ToTransferEncoding(String text)
        {
            switch (text.ToLower())
            {
                case "7bit": return TransferEncoding.SevenBit;
                case "base64": return TransferEncoding.Base64;
                case "quoted-printable": return TransferEncoding.QuotedPrintable;
            }
            return TransferEncoding.SevenBit;
        }

        /// From TransferEncoding to retrieve the string.
        /// <summary>
        /// From TransferEncoding to retrieve the string.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String ToTransferEncoding(TransferEncoding encoding)
        {
            switch (encoding)
            {
                case TransferEncoding.SevenBit: return "7bit";
                case TransferEncoding.Base64: return "Base64";
                case TransferEncoding.QuotedPrintable: return "Quoted-Printable";
            }
            return "7bit";
        }

        /// of the mail header encoding string.
        /// <summary>
        /// of the mail header encoding string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encodeType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String EncodeToMailHeaderLine(String text, TransferEncoding encodeType, Encoding encoding)
        {
            return EncodeToMailHeaderLine(text, encodeType, encoding, MaxCharCountPerRow);
        }

        /// of the mail header encoding string.
        /// <summary>
        /// of the mail header encoding string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encodeType"></param>
        /// <param name="encoding"></param>
        /// <param name="maxCharCount"></param>
        /// <returns></returns>
        public static String EncodeToMailHeaderLine(String text, TransferEncoding encodeType, Encoding encoding, Int32 maxCharCount)
        {
            Byte[] bb;
            var sb = new StringBuilder();
            var startIndex = 0;
            Int32 charCountPerRow ;
            Int32 byteCount;

            if (maxCharCount > MaxCharCountPerRow)
            { throw new ArgumentException("maxCharCount must less than MailParser.MaxCharCountPerRow."); }

            if (String.IsNullOrEmpty(text))
            { return ""; }

            if (AsciiCharOnly(text))
            {
                startIndex = 0;
                charCountPerRow = maxCharCount;
                for (var i = 0; i < text.Length; i++)
                {
                    sb.Append(text[i]);
                    if (startIndex == charCountPerRow)
                    {
                        sb.Append(NewLine);
                        startIndex = 0;
                        charCountPerRow = MaxCharCountPerRow;
                        if (i < text.Length - 1)
                        {
                            sb.Append("\t");
                        }
                    }
                    else
                    {
                        startIndex += 1;
                    }
                }
                return sb.ToString();
            }
            if (encodeType == TransferEncoding.Base64)
            {
                charCountPerRow = (Int32)Math.Floor((maxCharCount - (encoding.WebName.Length + 10)) * 0.75);
                for (var i = 0; i < text.Length; i++)
                {
                    byteCount = encoding.GetByteCount(text.Substring(startIndex, (i + 1) - startIndex));
                    if (byteCount > charCountPerRow)
                    {
                        bb = encoding.GetBytes(text.Substring(startIndex, i - startIndex));
                        sb.AppendFormat("=?{0}?B?{1}?={2}\t", encoding.WebName, Convert.ToBase64String(bb), NewLine);
                        startIndex = i;
                        charCountPerRow = (Int32)Math.Floor((MaxCharCountPerRow - (encoding.WebName.Length + 10)) * 0.75);
                    }
                }
                bb = encoding.GetBytes(text.Substring(startIndex));
                sb.AppendFormat("=?{0}?B?{1}?=", encoding.WebName, Convert.ToBase64String(bb));

                return sb.ToString();
            }
            if (encodeType != TransferEncoding.QuotedPrintable)
            {
                return text;
            }
            charCountPerRow = (Int32) Math.Floor((maxCharCount - (Double) (encoding.WebName.Length + 10))/3);
            for (var i = 0; i < text.Length; i++)
            {
                byteCount = encoding.GetByteCount(text.Substring(startIndex, (i + 1) - startIndex));
                if (byteCount > charCountPerRow)
                {
                    bb = encoding.GetBytes(text.Substring(startIndex, i - startIndex));
                    sb.AppendFormat("=?{0}?Q?{1}?={2}\t", encoding.WebName,
                                    ToQuotedPrintableOnHeader(encoding.GetString(bb)), NewLine);
                    startIndex = i;
                    charCountPerRow =
                        (Int32) Math.Floor((MaxCharCountPerRow - (encoding.WebName.Length + 10))*0.75);
                }
            }
            bb = encoding.GetBytes(text.Substring(startIndex));
            sb.AppendFormat("=?{0}?Q?{1}?=", encoding.WebName, ToQuotedPrintable(encoding.GetString(bb)));

            return sb.ToString();
        }

        /// A string of the mail header, 2231 RFC according to the specifications of the encoding.
        /// <summary>
        /// A string of the mail header, 2231 RFC according to the specifications of the encoding.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <param name="maxCharCount"></param>
        /// <returns></returns>
        public static String EncodeToMailHeaderLineByRfc2231(String parameterName, String text, Encoding encoding, Int32 maxCharCount)
        {
            var sb = new StringBuilder();
            var startIndex = 0;
            var rowNo = 0;

            var charCountPerRow = MaxCharCountPerRow - parameterName.Length - 3;
            var bb = encoding.GetBytes(text);
            for (var i = 0; i < bb.Length; i++)
            {
                //0-9
                if (0x30 <= bb[i] && bb[i] <= 0x39)
                {
                    sb.Append((Char)bb[i]);
                }
                else if (0x41 <= bb[i] && bb[i] <= 0x5a)//A-Z
                {
                    sb.Append((Char)bb[i]);
                }
                else if (0x61 <= bb[i] && bb[i] <= 0x7a)//a-z
                {
                    sb.Append((Char)bb[i]);
                }
                else
                {
                    sb.Append("%");
                    sb.Append(bb[i].ToString("X2"));
                }
            }

            if (sb.Length > charCountPerRow)
            {
                var s = sb.ToString();
                sb.Length = 0;
                while (true)
                {
                    if (rowNo > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(parameterName);
                    sb.Append("*");
                    sb.Append(rowNo);
                    sb.Append("*=");
                    if (rowNo == 0)
                    {
                        sb.Append(encoding.WebName);
                        sb.Append("''");
                    }
                    if (startIndex + charCountPerRow < s.Length)
                    {
                        sb.Append(s.Substring(startIndex, charCountPerRow));
                        sb.Append(NewLine);
                    }
                    else
                    {
                        sb.Append(s.Substring(startIndex, s.Length - startIndex));
                        sb.Append(";");
                        break;
                    }
                    rowNo += 1;
                    startIndex += charCountPerRow;
                }
                return sb.ToString();
            }
            return String.Format("{0}*={1}''{2}", parameterName, encoding.WebName, sb);
        }

        /// The mail headers to decode the string.
        /// <summary>
        /// The mail headers to decode the string.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static String DecodeFromMailHeaderLine(String line)
        {
            var rg = RegexList.DecodeByRfc2047;
            Encoding en;
            var startIndex = 0;
            var sb = new StringBuilder();

            if (String.IsNullOrEmpty(line)) { return ""; }

            var m = RegexList.DecodeByRfc2231.Match(line);
            var mc = rg.Matches(line);
            if (m.Success  && mc.Count == 0)
            {
                en = GetEncoding(m.Groups["Encoding"].Value);
                sb.Append(DecodeFromMailHeaderLineByRfc2231(m.Groups["Value"].Value, en));
            }
            else
            {
                for (var i = 0; i < mc.Count; i++)
                {
                    m = mc[i];
                    sb.Append(line.Substring(startIndex, m.Index - startIndex));
                    startIndex = m.Index + m.Length;

                    if (m.Groups.Count < 3)
                    {
                        throw new InvalidDataException();
                    }
                    Byte[] bb;
                    if (m.Groups["BorQ"].Value.ToUpper() == "B")
                    {
                        bb = Convert.FromBase64String(m.Groups["Value"].Value);
                    }
                    else if (m.Groups["BorQ"].Value.ToUpper() == "Q")
                    {
                        bb = FromQuotedPrintableTextOnHeader(m.Groups["Value"].Value);
                    }
                    else
                    {
                        throw new InvalidDataException();
                    }
                    en = GetEncoding(m.Groups["Encoding"].Value);
                    sb.Append(en.GetString(bb));
                }
                sb.Append(line.Substring(startIndex, line.Length - startIndex));
            }
            return sb.ToString();
        }

        /// A string of the mail header, 2231 RFC according to the specifications of the decoding.
        /// <summary>
        /// A string of the mail header, 2231 RFC according to the specifications of the decoding.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String DecodeFromMailHeaderLineByRfc2231(String text, Encoding encoding)
        {
            var currentIndex = 0;
            var bb = new Byte[text.Length];
            var byteArrayIndex = 0;
            var hexChar = "";

            while (true)
            {
                //% check whether FF format
                Boolean isDigitChar;
                if (currentIndex <= text.Length - 3 &&
                    text[currentIndex] == '%')
                {
                    hexChar = text.Substring(currentIndex + 1, 2);
                    isDigitChar = RegexList.HexDecoder1.IsMatch(hexChar);
                }
                else
                {
                    isDigitChar = false;
                }

                if (isDigitChar)
                {
                    bb[byteArrayIndex] = Convert.ToByte(hexChar, 16);
                    currentIndex += 3;
                }
                else
                {
                    bb[byteArrayIndex] = (Byte)text[currentIndex];
                    currentIndex += 1;
                }
                byteArrayIndex += 1;
                if (currentIndex >= text.Length) { break; }
            }
            //Convert byte array into a string.
            var bb2 = new Byte[byteArrayIndex];
            Array.Copy(bb, 0, bb2, 0, byteArrayIndex);
            return encoding.GetString(bb2);
        }

        /// Content-Type's analysis.
        /// <summary>
        /// Parse content-type.
        /// Content-Type's analysis.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="line"></param>
        public static void ParseContentType(ContentType contentType, String line)
        {
            Match m;

            //name=???;
            foreach (var rx in RegexList.ContentTypeName)
            {
                m = rx.Match(line);
                if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
                {
                    contentType.Name = m.Groups["Value"].Value;
                    break;
                }
            }
            if (String.IsNullOrEmpty(contentType.Name))
            {
                contentType.Name = ParseHeaderParameterValue("name", line);
            }

            //boundary
            foreach (var rx in RegexList.ContentTypeBoundary)
            {
                m = rx.Match(line);
                if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
                {
                    contentType.Boundary = m.Groups["Value"].Value;
                    break;
                }
            }
            if (String.IsNullOrEmpty(contentType.Boundary))
            {
                contentType.Boundary = ParseHeaderParameterValue("boundary", line);
            }
        }

        /// Content-Disposition's analysis.
        /// <summary>
        /// Parse content-disposision.
        /// Content-Disposition's analysis.
        /// </summary>
        /// <param name="contentDisposition"></param>
        /// <param name="line"></param>
        public static void ParseContentDisposition(ContentDisposition contentDisposition, String line)
        {
            //filename=???;
            foreach (var rx in RegexList.ContentDispositionFileName)
            {
                var m = rx.Match(line);
                if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
                {
                    contentDisposition.FileName = m.Groups["Value"].Value;
                    return;
                }
            }
            contentDisposition.FileName = ParseHeaderParameterValue("filename", line);
        }

        private static String ParseHeaderParameterValue(String parameterName, String line)
        {
            var rowNo = 0;
            var sb = new StringBuilder();

            var l = new List<String> {RegexList.Rfc2231FormatText, RegexList.Rfc2231FormatText1};

            foreach (var t in l)
            {
                while (true)
                {
                    var rx = new Regex(String.Format(t, parameterName, rowNo), RegexOptions.IgnoreCase);
                    var m = rx.Match(line);
                    if (String.IsNullOrEmpty(m.Groups["Value"].Value))
                    {
                        break;
                    }
                    sb.Append(m.Groups["Value"].Value);
                    rowNo += 1;
                }
            }
            return sb.ToString();
        }

        /// The body of the email message string encoded according to the specifications of the email message.
        /// <summary>
        /// The body of the email message string encoded according to the specifications of the email message.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encodeType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String EncodeToMailBody(String text, TransferEncoding encodeType, Encoding encoding)
        {
            var bb = encoding.GetBytes(text);
            if (encodeType == TransferEncoding.Base64)
            {
                return Convert.ToBase64String(bb);
            }
            if (encodeType == TransferEncoding.QuotedPrintable)
            {
                return ToQuotedPrintable(encoding.GetString(bb));
            }
            return encoding.GetString(bb);
        }

        /// The body of the email message to parse a string, the body of the email message that is decoded to retrieve the string.
        /// <summary>
        /// The body of the email message to parse a string, the body of the email message that is decoded to retrieve the string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encodeType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String DecodeFromMailBody(String text, TransferEncoding encodeType, Encoding encoding)
        {
            Byte[] b ;

            if (encodeType == TransferEncoding.Base64)
            {
                b = Convert.FromBase64String(text);
            }
            else if (encodeType == TransferEncoding.QuotedPrintable)
            {
                b = FromQuotedPrintableText(text);
            }
            else
            {
                b = encoding.GetBytes(text);
            }
            return encoding.GetString(b);
        }

        /// Boundary Generate String.
        /// <summary>
        /// Boundary Generate String.
        /// </summary>
        /// <returns></returns>
        public static string GenerateBoundary()
        {
            var s = String.Format("NextPart_{0}", Guid.NewGuid().ToString("D"));
            return s;
        }

        /// Q-encode, decode the string to encode and to retrieve the string.
        /// <summary>
        /// Q-encode, decode the string to encode and to retrieve the string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToQuotedPrintableOnHeader(String text)
        {
            var sr = new StringReader(text);
            var sb = new StringBuilder();
            Int32 i;

            while ((i = sr.Read()) > 0)
            {
                //If the characters ASCII
                if (32 < i && i < 127)
                {
                    //Space, =,? The _
                    if (i == 32 ||
                        i == 61 ||
                        i == 63 ||
                        i == 95)
                    {
                        sb.Append("=");
                        sb.Append(Convert.ToString(i, 16).ToUpper());
                    }
                    else
                    {
                        sb.Append(Convert.ToChar(i));
                    }
                }
                else
                {
                    sb.Append("=");
                    sb.Append(Convert.ToString(i, 16).ToUpper());
                }
            }
            return sb.ToString();
        }

        /// In QuotedPrintable decoded string to encode and to retrieve the string.
        /// <summary>
        /// In QuotedPrintable decoded string to encode and to retrieve the string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToQuotedPrintable(String text)
        {
            var sr = new StringReader(text);
            var sb = new StringBuilder();
            Int32 i;

            while ((i = sr.Read()) > 0)
            {
                //If =
                if (i == 61)
                {
                    sb.Append("=");
                    sb.Append(Convert.ToString(i, 16).ToUpper());
                }
                //ASCII characters, carriage returns, line feeds, horizontal tab, if the space
                else if ((32 < i && i < 127) ||
                    i == AsciiCharCode.CarriageReturn.GetNumber() ||
                    i == AsciiCharCode.LineFeed.GetNumber() ||
                    i == AsciiCharCode.HorizontalTabulation.GetNumber() ||
                    i == AsciiCharCode.Space.GetNumber())
                {
                    sb.Append(Convert.ToChar(i));
                }
                else
                {
                    sb.Append("=");
                    sb.Append(Convert.ToString(i, 16).ToUpper());
                }
            }
            return sb.ToString();
        }

        /// QuotedPrintable encoded string to decode and to retrieve the string.
        /// <summary>
        /// QuotedPrintable encoded string to decode and to retrieve the string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Byte[] FromQuotedPrintableTextOnHeader(String text)
        {
            if (text == null)
            { throw new ArgumentNullException(); }

            var ms = new MemoryStream();

            using (var sr = new StringReader(text))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    // is the last character on the line, the line is of the = to indicate that you want to continue.
                    if (line.EndsWith("="))
                    {
                        // Remove =
                        line = line.Substring(0, line.Length - 1);
                    }
                    var i = 0;
                    while (i < line.Length)
                    {
                        // character at current position is " = "
                        if (line.Substring(i, 1) == "=")
                        {
                            // To obtain hexadecimal string
                            var charLen = i == (line.Length - 2) ? 1 : 2;
                            var target = line.Substring(i + 1, charLen);
                            ms.WriteByte(Convert.ToByte(target, 16));
                            i += 3;
                        }
                        // Space represented by "_"
                        else if (line.Substring(i, 1) == "_")
                        {

                            ms.WriteByte(Convert.ToByte(' '));
                            i = i + 1;
                        }
                        // character at current position is " = "If you are not in
                        else
                        {
                            ms.WriteByte(Convert.ToByte(line[i]));
                            i = i + 1;
                        }
                    }
                }
            }
            return ms.ToArray();
        }

        /// QuotedPrintable encoded string to decode and to retrieve the string.
        /// <summary>
        /// QuotedPrintable encoded string to decode and to retrieve the string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Byte[] FromQuotedPrintableText(String text)
        {
            if (text == null)
            { throw new ArgumentNullException(); }

            var ms = new MemoryStream();

            using (var sr = new StringReader(text))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    // is the last character on the line, the line is of the = to indicate that you want to continue.
                    Boolean addNewLine;
                    if (line.EndsWith("="))
                    {
                        // Remove =
                        line = line.Substring(0, line.Length - 1);
                        addNewLine = false;
                    }
                    else
                    {
                        addNewLine = true;
                    }
                    var i = 0;
                    while (i < line.Length)
                    {
                        // character at current position is " = "
                        if (line.Substring(i, 1) == "=")
                        {
                            // To obtain hexadecimal string
                            var charLen = i == (line.Length - 2) ? 1 : 2;
                            var target = line.Substring(i + 1, charLen);
                            ms.WriteByte(Convert.ToByte(target, 16));
                            i += 3;
                        }
                        // character at current position is " = "If you are not in
                        else
                        {
                            ms.WriteByte(Convert.ToByte(line[i]));
                            i = i + 1;
                        }
                    }
                    //Add a line break
                    if (addNewLine)
                    {
                        ms.WriteByte(AsciiCharCode.CarriageReturn.GetNumber());
                        ms.WriteByte(AsciiCharCode.LineFeed.GetNumber());
                    }
                }
            }
            return ms.ToArray();
        }

        /// String Base 64 string.
        /// <summary>
        /// String Base 64 string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToBase64String(String text)
        {
            var b = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(b, 0, b.Length);
        }

        /// String Base 64 string.
        /// <summary>
        /// String Base 64 string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String ToBase64String(Byte[] bytes)
        {
            return Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
        }

        /// String Base 64 string.
        /// <summary>
        /// String Base 64 string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String FromBase64String(String text)
        {
            var b = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(b);
        }

        /// Cram-MD 5 strings in the conversion.
        /// <summary>
        /// Cram-MD 5 strings in the conversion.
        /// </summary>
        /// <param name="challenge"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static String ToCramMd5String(String challenge, String userName, String password)
        {
            // The user name and calculate HMAC-MD Base5 hash value 64 encoding the return as a response
            var bb = Encoding.UTF8.GetBytes(String.Format("{0} {1}", userName, Cryptography.ToCramMd5String(challenge, password)));
            return Convert.ToBase64String(bb);
        }

        /// Specify the ASCII character that is a string that consists of only gets a value indicating whether or not.
        /// <summary>
        /// Specify the ASCII character that is a string that consists of only gets a value indicating whether or not.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Boolean AsciiCharOnly(String text)
        {
            return !RegexList.AsciiCharOnly.IsMatch(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(String encoding)
        {
            return GetEncoding(encoding, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(String encoding, Encoding defaultEncoding)
        {
            var d = EncodingList;
            if (d.ContainsKey(encoding.ToUpper()))
            {
                return d[encoding.ToUpper()];
            }
            try
            {
                return Encoding.GetEncoding(encoding);
            }
            catch { return defaultEncoding; }
        }
    }
}
