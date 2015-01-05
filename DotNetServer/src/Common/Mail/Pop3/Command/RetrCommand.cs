using System;

namespace Common.Mail.Pop3.Command
{
    /// <summary>Represents retr command.
    /// </summary>
    public class RetrCommand : Pop3Command
    {
        private Int64 _mailIndex = 1;
		/// <summary>
		/// 
		/// </summary>
        public override String Name
        {
            get { return "Retr"; }
        }

		/// <summary>
		/// 
		/// </summary>
        public Int64 MailIndex
        {
            get { return _mailIndex; }
            set { _mailIndex = value; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mailIndex"></param>
        public RetrCommand(Int64 mailIndex)
        {
            if (mailIndex < 1)
            { throw new ArgumentException(); }
            _mailIndex = mailIndex;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0} {1}", Name, _mailIndex);
        }
    }
}
