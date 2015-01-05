using System;

namespace Common.Mail.Smtp.Command
{
    /// Represent an abstraction of smtp command.
    /// <summary>
    /// Represent an abstraction of smtp command.
    /// </summary>
    public abstract class SmtpCommand
    {
		/// <summary>
		/// 
		/// </summary>
        public abstract String Name { get;}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public abstract String GetCommandString();
    }
}
