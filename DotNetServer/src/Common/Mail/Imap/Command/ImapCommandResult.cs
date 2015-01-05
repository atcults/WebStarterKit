using System;
using System.Text.RegularExpressions;

namespace Common.Mail.Imap.Command
{
    /// <summary>Represents result of pop3 command class.
    /// </summary>
    public class ImapCommandResult
    {
        private readonly String _text = "";
        private readonly ImapCommandResultStatus _status = ImapCommandResultStatus.None;
        /// <summary>
		/// 
		/// </summary>
        public String Text
        {
            get { return _text; }
        }

		/// <summary>
		/// 
		/// </summary>
        public ImapCommandResultStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="text"></param>
        public ImapCommandResult(String tag, String text)
        {
            _text = text;
            var rx = new Regex(@"^" + tag + " (OK|NO|BAD) .*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var m = rx.Match(text);
            var response = m.Groups[1].Value;
            if (String.Equals(response, "OK", StringComparison.OrdinalIgnoreCase) )
            {
                _status = ImapCommandResultStatus.Ok;
            }
            else if (String.Equals(response, "NO", StringComparison.OrdinalIgnoreCase) )
            {
                _status = ImapCommandResultStatus.No;
            }
            else if (String.Equals(response, "BAD", StringComparison.OrdinalIgnoreCase) )
            {
                _status = ImapCommandResultStatus.Bad;
            }
            else
            {
                _status = ImapCommandResultStatus.None;
            }
            _text = text;
        }
    }
}
