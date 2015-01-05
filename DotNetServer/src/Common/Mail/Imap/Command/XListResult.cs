using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Common.Mail.Imap.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class XListResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<XListLineResult> Lines { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        public XListResult(List<XListLineResult> lines)
        {
            Lines = new ReadOnlyCollection<XListLineResult>(lines);
        }
    }
}
