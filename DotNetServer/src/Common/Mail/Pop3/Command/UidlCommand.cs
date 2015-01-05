using System;

namespace Common.Mail.Pop3.Command
{
    /// UIDLコマンドを表現するクラスです。
    /// <summary>
    /// UIDLコマンドを表現するクラスです。
    /// </summary>
    public class UidlCommand : Pop3Command
    {
        private Int64? _mailIndex ;
		/// <summary>
		/// 
		/// </summary>
        public override String Name
        {
            get { return "Uidl"; }
        }

		/// <summary>
		/// 
		/// </summary>
        public Int64? MailIndex
        {
            get { return _mailIndex; }
        }

		/// <summary>
		/// 
		/// </summary>
        public UidlCommand()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mailIndex"></param>
        public UidlCommand(Int64 mailIndex)
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
