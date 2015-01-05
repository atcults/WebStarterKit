using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Common.Mail.Common;

namespace Common.Mail.Imap.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class CapabilityResult : ImapCommandResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<String> Features { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="text"></param>
        public CapabilityResult(String tag, String text)
            : base(tag, text)
        {
            const string s = "* Capability ";
            if (text.StartsWith(s, StringComparison.OrdinalIgnoreCase) == false)
            { throw new MailClientException(); }

            string line;
            using (var sr = new StringReader(text))
            {
                line = sr.ReadLine();
            }
            var index = s.Length;
            if (line != null)
            {
                var ss = line.Substring(index, line.Length - index);
                var l = ss.Split(' ').Where(el => String.IsNullOrEmpty(el) != true).ToList();
                Features = new ReadOnlyCollection<string>(l);
            }
        }
    }
}
