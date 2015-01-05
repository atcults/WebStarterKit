using System;

namespace Common.Mail.Smtp.Command
{
    /// Represent help command.
    /// <summary>
    /// Represent help command.
    /// </summary>
    public class HelpCommand : SmtpCommand
    {
        private String _commandName = "";
		/// <summary>
		/// 
		/// </summary>
        public override String Name
        {
            get { return "Help"; }
        }

		/// <summary>
		/// 
		/// </summary>
        public String CommandName
        {
            get { return _commandName; }
            set { _commandName = value; }
        }

		/// <summary>
		/// 
		/// </summary>
        public HelpCommand()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="commandName"></param>
        public HelpCommand(String commandName)
        {
            _commandName = commandName;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0} {1}", Name, CommandName);
        }
    }
}
