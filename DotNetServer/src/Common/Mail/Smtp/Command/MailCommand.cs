using System;

namespace Common.Mail.Smtp.Command
{
    /// Represent mail command.
    /// <summary>
    /// Represent mail command.
    /// </summary>
    public class MailCommand : SmtpCommand
    {
        private String _reversePath = "";
		/// <summary>
		/// Mail id from which you want to send mail
		/// </summary>
        public override String Name
        {
            get { return "Mail From:"; }
        }

		/// <summary>
		/// 
		/// </summary>
        public String ReversePath
        {
            get { return _reversePath; }
            set { _reversePath = value; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reversePath"></param>
        public MailCommand(String reversePath)
        {
            _reversePath = reversePath;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0}{1}", Name, ReversePath);
        }
    }
}
