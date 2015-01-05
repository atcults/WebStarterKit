using System;
using System.Text.RegularExpressions;

namespace Common.Mail.Pop3.Command
{
    /// <summary>Represents result of stat command.
    /// </summary>
    public class StatCommandResult : Pop3CommandResult
    {
        private struct RegexList
        {
            public static readonly Regex TotalMessageCount = new Regex(@"^.*\+OK[\s|\t]+([0-9]+)[\s|\t]+.*$");
            public static readonly Regex TotalSize = new Regex(@"^.*\+OK[\s|\t]+[0-9]+[\s|\t]+([0-9]+).*$");
        }
        private readonly Int64 _totalMessageCount ;
        private readonly Int64 _totalSize;
        /// <summary>
        /// 
        /// </summary>
        public Int64 TotalMessageCount
        {
            get { return _totalMessageCount; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int64 TotalSize
        {
            get { return _totalSize; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public StatCommandResult(String text)
            : base(text)
        {
            if (Ok)
            {
                _totalMessageCount = GetTotalMessageCount(text);
                _totalSize = GetTotalSize(text);
            }
        }

        /// <summary>Analyze response single line and get total mail count of mailbox.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static Int64 GetTotalMessageCount(String text)
        {
            return Int64.Parse(RegexList.TotalMessageCount.Replace(text.Replace("\r\n", ""), "$1"));
        }

        /// <summary>Analyze response single line and get total mail size of mailbox.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static Int64 GetTotalSize(String text)
        {
            return Int64.Parse(RegexList.TotalSize.Replace(text.Replace("\r\n", ""), "$1"));
        }
    }
}
