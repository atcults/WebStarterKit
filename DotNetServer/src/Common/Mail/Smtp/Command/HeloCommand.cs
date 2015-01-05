using System;

namespace Common.Mail.Smtp.Command
{
    /// Represent helo command.
    /// <summary>
    /// Represent helo command.
    /// </summary>
    public class HeloCommand : SmtpCommand
    {
        private String _domain = "";
		/// <summary>
		/// Name of helo command
		/// </summary>
        public override String Name
        {
            get { return "Helo"; }
        }

		/// <summary>
		/// Name of domain
		/// </summary>
        public String Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="domain"></param>
        public HeloCommand(String domain)
        {
            _domain = domain;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0} {1}", Name, Domain);
        }
    }
}
