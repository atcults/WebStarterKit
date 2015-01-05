using System;

namespace Common.Mail.Smtp.Command
{
    /// Represent rcpt command.
    /// <summary>
    /// Represent rcpt command.
    /// </summary>
    public class RcptCommand : SmtpCommand
    {
        private String _forwardPath = "";
		/// <summary>
		/// 
		/// </summary>
        public override String Name
        {
            get { return "Rcpt To:"; }
        }

		/// <summary>
		/// 
		/// </summary>
        public String ForwardPath
        {
            get { return _forwardPath; }
            set { _forwardPath = value; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inForwardPath"></param>
        public RcptCommand(String inForwardPath)
        {
            _forwardPath = inForwardPath;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0}{1}", Name,ForwardPath);
        }
    }
}
