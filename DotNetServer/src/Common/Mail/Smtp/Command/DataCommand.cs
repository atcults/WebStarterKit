using System;

namespace Common.Mail.Smtp.Command
{
    /// Represent data command.
    /// <summary>
    /// Represent data command.
    /// </summary>
    public class DataCommand : SmtpCommand
    {
		/// <summary>
		/// Get command name
		/// </summary>
        public override String Name
        {
            get { return "Data"; }
        }

		/// <summary>
		/// Get Command name as string
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return Name;
        }
    }
}
