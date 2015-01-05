using System;

namespace Common.Mail.Pop3.Command
{
    /// <summary>Represents list command.
    /// </summary>
    public class ListCommand : Pop3Command
    {
        private Int64? _mailIndex;
		/// <summary>
		/// 
		/// </summary>
        public override String Name
        {
            get { return "List"; }
        }

		/// <summary>
		/// 
		/// </summary>
        public Int64? MailIndex
        {
            get { return _mailIndex; }
            set { _mailIndex = value; }
        }

		/// <summary>
		/// 
		/// </summary>
        public ListCommand()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mailIndex"></param>
        public ListCommand(Int64 mailIndex)
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
			if (_mailIndex.HasValue)
			{
				return String.Format("{0} {1}", Name, _mailIndex);
			}
			return Name;
        }
    }
}
