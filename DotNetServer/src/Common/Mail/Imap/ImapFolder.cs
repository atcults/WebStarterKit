using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Mail.Imap.Command;

namespace Common.Mail.Imap
{
    /// <summary>
    /// 
    /// </summary>
    public class ImapFolder
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
        public Int32 MailCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 RecentMailCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ImapFolder> Children { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<String> Flags { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public ImapFolder(SelectResult result)
        {
            Name = result.FolderName;
            MailCount = result.Exists;
            RecentMailCount = result.Recent;
            Flags = new ReadOnlyCollection<string>(new List<string>(result.Flags));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public ImapFolder(ListLineResult result)
        {
            Name = result.Name;
            NoSelect = result.NoSelect;
            HasChildren = result.HasChildren;
            MailCount = -1;
            RecentMailCount = -1;
            Flags = new ReadOnlyCollection<string>(new List<string>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="noSelect"></param>
        /// <param name="hasChildren"></param>
        public ImapFolder(String folderName, Boolean noSelect, Boolean hasChildren)
        {
            Name = folderName;
            NoSelect = noSelect;
            HasChildren = hasChildren;
            MailCount = -1;
            RecentMailCount = -1;
            Flags = new ReadOnlyCollection<string>(new List<string>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
