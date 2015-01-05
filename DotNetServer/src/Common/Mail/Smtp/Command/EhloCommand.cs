using System;
using System.Collections.Generic;

namespace Common.Mail.Smtp.Command
{
    /// Represent ehlo command.
    /// <summary>
    /// Represent ehlo command.
    /// </summary>
    public class EhloCommand : SmtpCommand
    {
        private String _domain = "";
		/// <summary>
		/// Define ehlo as name
		/// </summary>
        public override String Name
        {
            get { return "Ehlo"; }
        }

		/// <summary>
		/// Define domain name
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
        public EhloCommand(String domain)
        {
            _domain = domain;
        }

		/// <summary>
		/// Get string of command
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0} {1}", Name, Domain);
        }

		/// <summary>
		/// 
		/// </summary>
        public class Result
        {
            private String _keyword = "";
            private readonly List<String> _parameters = new List<string>();
			/// <summary>
			/// 
			/// </summary>
            public String Keyword
            {
                get { return _keyword; }
                set { _keyword = value; }
            }
			/// <summary>
			/// 
			/// </summary>
            public List<String> Parameters
            {
                get { return _parameters; }
            }
        }
    }
}
