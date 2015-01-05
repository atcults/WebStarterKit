using System;
using System.Text;

namespace Common.Mail.Smtp.Command
{
    /// Represent smtp command result responsed from server.
    /// <summary>
    /// Represent smtp command result responsed from server.
    /// </summary>
    public class SmtpCommandResult
    {
        private readonly SmtpCommandResultCode _statusCode = SmtpCommandResultCode.None;
        private readonly String _message = "";
		/// <summary>
		/// Statuscode for smtp command result responsed from server.
		/// </summary>
        public SmtpCommandResultCode StatusCode
        {
            get { return _statusCode; }
        }

		/// <summary>
		/// message text for smtp command result responsed from server.
		/// </summary>
        public String Message
        {
            get { return _message; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lines"></param>
        public SmtpCommandResult(SmtpCommandResultLine[] lines)
        {
            var sb = new StringBuilder();
            if (lines.Length == 0)
            { throw new ArgumentException("line must not be zero length."); }

            _statusCode = lines[0].StatusCode;
            for (var i = 0; i < lines.Length; i++)
            {
                sb.Append(lines[i].Message);
                if (i < lines.Length - 1)
                {
                    sb.AppendLine();
                }
            }
           _message = sb.ToString();
        }
    }
}
