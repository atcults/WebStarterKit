using System;

namespace Common.Mail.Pop3.Command
{
    /// <summary>Represents top command.
    /// </summary>
    public class TopCommand : Pop3Command
    {
        private readonly Int64 _mailIndex = 1;
        private readonly Int32 _lineCount;
		/// <summary>
		/// 
		/// </summary>
        public override String Name
        {
            get { return "Top"; }
        }

		/// <summary>
		/// 
		/// </summary>
        public Int64 MailIndex
        {
            get { return _mailIndex; }
        }

		/// <summary>
		/// 
		/// </summary>
        public Int32 LineCount
        {
            get { return _lineCount; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mailIndex"></param>
        public TopCommand(Int64 mailIndex)
        {
            if (mailIndex < 1)
            { throw new ArgumentException(); }
            _mailIndex = mailIndex;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="lineCount"></param>
        public TopCommand(Int64 mailIndex, Int32 lineCount)
        {
            if (mailIndex < 1)
            { throw new ArgumentException(); }
            _mailIndex = mailIndex;
            _lineCount = lineCount;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override String GetCommandString()
        {
            return String.Format("{0} {1} {2}", Name, _mailIndex, _lineCount);
        }
    }
}
