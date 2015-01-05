using System;
using Common.Mail.Common;

namespace Common.Mail.Pop3.Command
{
    /// <summary>Represents result of pop3 command class.
    /// </summary>
    public class Pop3CommandResult
    {
        private readonly String _text = "";
        private readonly Boolean _ok = true;
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
        public Boolean Ok
        {
            get { return _ok; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
        public Pop3CommandResult(String text)
        {
            _ok = MailParser.IsResponseOk(text);
            _text = text;
        }
    }
}
