using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Common.Mail.Imap.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class ListResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<ListLineResult> Lines { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        public ListResult(List<ListLineResult> lines)
        {
            this.Lines = new ReadOnlyCollection<ListLineResult>(lines);
        }
    }
}
