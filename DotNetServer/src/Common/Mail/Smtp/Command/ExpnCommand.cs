using System;

namespace Common.Mail.Smtp.Command
{
    /// Represent expn command.
    /// <summary>
    /// Represent expn command.
    /// </summary>
    public class ExpnCommand : SmtpCommand
    {
        private String _mailingList = "";
		/// <summary>
		/// Name of command
		/// </summary>
        public override String Name
        {
            get { return "Expn"; }
        }

		/// <summary>
		/// Define mail list
		/// </summary>
        public String MailingList
        {
            get { return _mailingList; }
            set { _mailingList = value; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inMailingList"></param>
        public ExpnCommand(String inMailingList)
        {
            _mailingList = inMailingList;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0} {1}", Name, MailingList);
        }
    }
}
