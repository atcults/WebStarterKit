using System;
using System.Text.RegularExpressions;
using Common.Mail.Common;

namespace Common.Mail.Smtp.Command
{
    /// Represent an line of smtp command result responsed from server.
    /// <summary>
    /// Represent an line of smtp command result responsed from server.
    /// </summary>
    public class SmtpCommandResultLine
    {
		private struct RegexList
		{
			public static readonly Regex SmtpResultLine = new Regex(@"(?<StatusCode>[0-9]{3})(?<HasNextLine>[\s-]{0,1})(?<Message>.*)", RegexOptions.IgnoreCase);
		}

        private readonly Int32 _statusCodeNumber;
        private readonly SmtpCommandResultCode _statusCode = SmtpCommandResultCode.None;
		/// <summary>
		/// Get Code number
		/// </summary>
        public Int32 CodeNumber
        {
            get { return _statusCodeNumber; }
        }

		/// <summary>
		/// Get status code
		/// </summary>
        public SmtpCommandResultCode StatusCode
        {
            get { return _statusCode; }
        }

		/// <summary>
		/// 
		/// </summary>
        public Boolean HasNextLine { get; private set; }

		/// <summary>
		/// smtp text message
		/// </summary>
        public String Message { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="line"></param>
        public SmtpCommandResultLine(String line)
        {
            var m = RegexList.SmtpResultLine.Match(line);
            if (Int32.TryParse(m.Groups["StatusCode"].Value, out _statusCodeNumber) == false)
            { throw new MailClientException("Invalid format response." + Environment.NewLine + line); }
            _statusCode = (SmtpCommandResultCode)_statusCodeNumber;
            HasNextLine = m.Groups["HasNextLine"].Value == "-";
            Message = m.Groups["Message"].Value;
        }
    }
}
