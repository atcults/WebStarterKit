using System;

namespace Common.Mail.Smtp.Command
{
    /// Represent vrfy command.
    /// <summary>
    /// Represent vrfy command.
    /// </summary>
    public class VrfyCommand : SmtpCommand
    {
        private String _userName = "";
		/// <summary>
		/// vrfy name
		/// </summary>
        public override String Name
        {
            get { return "Vrfy"; }
        }

		/// <summary>
		/// vrfy username
		/// </summary>
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userName"></param>
        public VrfyCommand(String userName)
        {
            _userName = userName;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0} {1}", Name,UserName);
        }
    }
}
