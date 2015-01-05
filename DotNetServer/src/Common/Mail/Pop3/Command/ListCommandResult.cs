using System;
using System.Text.RegularExpressions;

namespace Common.Mail.Pop3.Command
{
    /// <summary>Represents result of list command.
    /// </summary>
    public class ListCommandResult
    {
        private struct RegexList
        {
            public static readonly Regex MessageIndex = new Regex(@"^([0-9]+)[\s|\t]+.*$");
            public static readonly Regex Size = new Regex(@"^[0-9]+[\s|\t]+([0-9]+).*$");
        }
        private readonly Int64 _mailIndex;
        private readonly Int32 _size;
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
        public Int32 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public ListCommandResult(String text)
        {
            _mailIndex = GetMessageIndex(text);
            _size = GetSize(text);
        }

        /// <summary>Analyze response single line and get mail index.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static Int64 GetMessageIndex(String line)
        {
            return Int64.Parse(RegexList.MessageIndex.Replace(line.Replace("\r\n", ""), "$1"));
        }

        /// <summary>Analyze response single line and get mail size.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static Int32 GetSize(String line)
        {
            return Int32.Parse(RegexList.Size.Replace(line.Replace("\r\n", ""), "$1"));
        }
    }
}
