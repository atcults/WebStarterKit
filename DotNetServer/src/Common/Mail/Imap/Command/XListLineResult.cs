using System;

namespace Common.Mail.Imap.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class XListLineResult
    {
        /// <summary>
        /// 
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Boolean HasChildren { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Boolean NoSelect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String XName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="noSelect"></param>
        /// <param name="hasChildren"></param>
        /// <param name="xName"></param>
        public XListLineResult(String folderName, Boolean noSelect, Boolean hasChildren, string xName)
        {
            Name = folderName;
            NoSelect = noSelect;
            HasChildren = hasChildren;
            XName = xName;
        }
    }
}
