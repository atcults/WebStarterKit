using System;

namespace Common.Mail.Pop3.Command
{
    /// <summary>Represents and define behavior of pop3 command as abstract class.
	/// </summary>
    public abstract class Pop3Command
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
