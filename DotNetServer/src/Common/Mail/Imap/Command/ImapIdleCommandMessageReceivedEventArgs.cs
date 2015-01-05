using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Common.Mail.Imap.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class ImapIdleCommandMessageReceivedEventArgs : EventArgs
    {
        private struct RegexList
        {
            public static readonly Regex Message = new Regex("\\* (?<Number>[0-9]*) (?<Type>(EXISTS)|(EXPUNGE))", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        private readonly List<ImapIdleCommandMessage> _messageList = new List<ImapIdleCommandMessage>();
        /// <summary>
        /// 
        /// </summary>
        public Boolean Done { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Text { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ImapIdleCommandMessage> MessageList
        {
            get { return _messageList; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public ImapIdleCommandMessageReceivedEventArgs(String text)
        {
            Done = false;
            Text = text;

            var sr = new StringReader(text);
            while (sr.Peek() > -1)
            {
                var line = sr.ReadLine();
                if (line != null && line.StartsWith("+ idling", StringComparison.OrdinalIgnoreCase))
                {
                    _messageList.Add(new ImapIdleCommandMessage(ImapIdleCommandMessageType.Idling, -1));
                }
                else
                {
                    if (line != null)
                    {
                        var m = RegexList.Message.Match(line);
                        if (String.IsNullOrEmpty(m.Value)) { continue; }
                        ImapIdleCommandMessageType tp;
                        switch (m.Groups["Type"].Value)
                        {
                            case "EXISTS": tp = ImapIdleCommandMessageType.Exists; break;
                            case "EXPUNGE": tp = ImapIdleCommandMessageType.Expunge; break;
                            default: continue;
                        }
                        _messageList.Add(new ImapIdleCommandMessage(tp, Int32.Parse(m.Groups["Number"].Value)));
                    }
                }
            }
        }
    }
}
