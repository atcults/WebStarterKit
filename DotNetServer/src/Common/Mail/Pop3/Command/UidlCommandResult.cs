using System;
using System.Text.RegularExpressions;

namespace Common.Mail.Pop3.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class UidlCommandResult
    {
        private struct RegexList
        {
            public static readonly Regex MessageIndex = new Regex(@"^([0-9]+)[\s|\t]+.*$", RegexOptions.None);
            public static readonly Regex Uid = new Regex(@"^[0-9]+[\s|\t]+([\x21-\x7E]+)$", RegexOptions.None);
        }
        private readonly Int64 _mailIndex;
        private readonly String _uid = "";
        /// <summary>
        /// 
        /// </summary>
        public Int64 MailIndex
        {
            get { return _mailIndex; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String Uid
        {
            get { return _uid; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public UidlCommandResult(String text)
        {
            if (text == null)
            { throw new ArgumentNullException("text"); }

            _mailIndex = GetMessageIndex(text);
            _uid = GetUid(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <param name="uid"></param>
        public UidlCommandResult(Int64 mailIndex, String uid)
        {
            _mailIndex = mailIndex;
            _uid = uid;
        }

        /// The receiving line, the string to parse the Index of the email message.
        /// <summary>
        /// The receiving line, the string to parse the Index of the email message.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static Int64 GetMessageIndex(String line)
        {
            return Int64.Parse(RegexList.MessageIndex.Replace(line.Replace("\r\n", ""), "$1"));
        }
        /// The receiving line, the string to parse the UID of the email message.
        /// <summary>
        /// The receiving line, the string to parse the UID of the email message.
        /// </summary>
        /// <param name="line"></param>
        /// <remarks>Characters in the range 0x21 to 0x7E: http://www.ietf.org/rfc/rfc1939.txt</remarks>
        /// <returns></returns>
        private static String GetUid(String line)
        {
            return RegexList.Uid.Replace(line.Replace("\r\n", ""), "$1");
        }
    }
}
