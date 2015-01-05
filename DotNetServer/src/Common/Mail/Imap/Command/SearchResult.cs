using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Common.Mail.Common;

namespace Common.Mail.Imap.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchResult : ImapCommandResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<Int64> MailIndexList { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="text"></param>
        public SearchResult(String tag, String text)
            : base(tag, text)
        {
            if (Status != ImapCommandResultStatus.Ok)
            {
                MailIndexList = new ReadOnlyCollection<Int64>(new List<Int64>());
                return;
            }
            const string startText = "* Search";
            if (text.StartsWith(startText, StringComparison.OrdinalIgnoreCase) == false)
            { throw new MailClientException(); }

            String line;
            using (var sr = new StringReader(text))
            {
                line = sr.ReadLine();
            }
            var startIndex = startText.Length + 1;
            if (line != null && line.Length < startIndex)
            {
                MailIndexList = new ReadOnlyCollection<Int64>(new List<Int64>());
                return;
            }
            if (line != null)
            {
                var ss = line.Substring(startIndex, line.Length - startIndex);
                var l = new List<Int64>();
                foreach (var el in ss.Split(' '))
                {
                    Int64 mailIndex;
                    if (Int64.TryParse(el, out mailIndex) == false) { continue; }
                    l.Add(mailIndex);
                }
                MailIndexList = new ReadOnlyCollection<Int64>(l);
            }
        }
    }
}
