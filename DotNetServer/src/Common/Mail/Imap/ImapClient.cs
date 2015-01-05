using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.Mail.Async;
using Common.Mail.Common;
using Common.Mail.Imap.Command;
using Common.Net.SocketClient;

namespace Common.Mail.Imap
{
    /// Represent and probide functionality about IMAP command.
    /// <summary>
    /// Represent and probide functionality about IMAP command.
    /// </summary>
    public class ImapClient : SocketClient, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Int32 DefaultTagNo = 1;
        /// <summary>
        /// 
        /// </summary>
        public class RegexList
        {
            /// <summary>
            /// 
            /// </summary>
            public static readonly Regex SelectFolderResultFlagsLine = new Regex(@"^\* FLAGS \((?<Flags>[^)]*)\)\r\n", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            /// <summary>
            /// 
            /// </summary>
            public static readonly Regex SelectFolderResult = new Regex(@"^\* (?<exst>\d+) EXISTS\r\n\* (?<rcnt>\d+) RECENT\r\n", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            /// <summary>
            /// 
            /// </summary>
            public static readonly Regex GetListFolderResult = new Regex("^\\* LIST \\(((?<opt>\\\\\\w+)\\s?)+\\) \".\" ((\"(?<name>.*)\")|(?<name>[^\r\n]*))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            /// <summary>
            /// 
            /// </summary>
            public static readonly Regex GetXListFolderResult = new Regex("^\\* XLIST \\(((?<opt>\\\\\\w+)\\s?)+\\) \".\" ((\"(?<name>.*)\")|(?<name>[^\r\n]*))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            /// <summary>
            /// 
            /// </summary>
            public static readonly Regex GetLsubFolderResult = new Regex("^\\* LSUB \\(((?<opt>\\\\\\w+)\\s?)+\\) \".\" ((\"(?<name>.*)\")|(?<name>[^\r\n]*))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            /// <summary>
            /// 
            /// </summary>
            public static readonly Regex GetRlsubFolderResult = new Regex("^\\* RLSUB \\(((?<opt>\\\\\\w+)\\s?)+\\) \".\" ((\"(?<name>.*)\")|(?<name>[^\r\n]*))", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly Int32 DefaultPort = 143;
        private ImapConnectionState _state = ImapConnectionState.Disconnected;
        /// <summary>
        /// Get connection state.
        /// </summary>
        public ImapConnectionState State
        {
            get
            {
                if (Connected == false)
                {
                    _state = ImapConnectionState.Disconnected;
                }
                return _state;
            }
        }

        /// <summary>
        /// Get selected folder
        /// </summary>
        public ImapFolder CurrentFolder { get; private set; }
        /// <summary>
        /// Get connection is ready.
        /// </summary>
        public Boolean Available
        {
            get { return _state != ImapConnectionState.Disconnected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 TagNo { get; set; }
        private String Tag
        {
            get { return "tag" + TagNo; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ImapClient(String serverName)
            : base(serverName, DefaultPort)
        {
            TagNo = DefaultTagNo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public ImapClient(String serverName, Int32 port, String userName, String password)
            : base(serverName, port)
        {
            UserName = userName;
            Password = password;
            TagNo = DefaultTagNo;
        }

        /// <summary>
        /// Open connection to a server.
        /// </summary>
        public ImapConnectionState Open()
        {
            _state = Connect() ? ImapConnectionState.Connected : ImapConnectionState.Disconnected;
            return _state;
        }

        /// The connection to the server if it is not already open, and opens a connection to a server.
        /// <summary>
        /// Ensure connection is opened.
        /// The connection to the server if it is not already open, and opens a connection to a server.
        /// </summary>
        public ImapConnectionState EnsureOpen()
        {
            if (Connected)
            { return _state; }

            return Open();
        }

        private void ValidateState(ImapConnectionState state)
        {
            ValidateState(state,false);
        }

        private void ValidateState(ImapConnectionState state, Boolean folderSelected)
        {
            if (_state != state)
            {
                switch (state)
                {
                    case ImapConnectionState.Disconnected: throw new MailClientException("You can execute this command only when State is Disconnected");
                    case ImapConnectionState.Connected: throw new MailClientException("You can execute this command only when State is Connected");
                    case ImapConnectionState.Authenticated: throw new MailClientException("You can execute this command only when State is Authenticated");
                    default: throw new MailClientException();
                }
            }
            if (folderSelected && CurrentFolder == null)
            {
                throw new MailClientException("You must select folder before executing this command."
                  + "You can select folder by calling SelectFolder,ExecuteSelect,ExecuteExamine method of this object.");
            }
        }

        private ImapCommandResult GetResponse()
        {
            var ms = new MemoryStream();
            GetResponse(ms);
            var s = ResponseEncoding.GetString(ms.ToArray());
            return new ImapCommandResult(Tag, s);
        }

        private void GetResponse(Stream stream)
        {
            var bb = GetResponseBytes(new ImapDataReceiveContext(Tag, ResponseEncoding));
            stream.Write(bb, 0, bb.Length);
            Commnicating = false;
        }

        /// <summary>
        /// Log in to IMAP server.
        /// </summary>
        /// <returns></returns>
        public Boolean Authenticate()
        {
            if (_state == ImapConnectionState.Authenticated) { return true; }
            var rs = ExecuteLogin();
            return State == ImapConnectionState.Authenticated;
        }

        private ImapCommandResult Execute(String command)
        {
            Send(command);
            Commnicating = true;
            return GetResponse();
        }

        /// <summary>
        /// Send capability command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public CapabilityResult ExecuteCapability()
        {
            var rs = Execute(Tag + " CAPABILITY");
            return new CapabilityResult(Tag, rs.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteLogin()
        {
            if (EnsureOpen() == ImapConnectionState.Disconnected) { throw new MailClientException(); }

            var commandText = String.Format(Tag + " LOGIN {0} {1}", UserName, Password);
            var rs = Execute(commandText);
            _state = rs.Status == ImapCommandResultStatus.Ok ? ImapConnectionState.Authenticated : ImapConnectionState.Connected;
            return rs;
        }

        /// <summary>
        /// Send Logout command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteLogout()
        {
            ValidateState(ImapConnectionState.Authenticated);
            var rs = Execute(Tag + " Logout");
            if (rs.Status == ImapCommandResultStatus.Ok)
            {
                _state = ImapConnectionState.Connected;
            }
            return rs;
        }

        /// <summary>
        /// Send select command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public SelectResult ExecuteSelect(String folderName)
        {
            ValidateState(ImapConnectionState.Authenticated);
            var commandText = String.Format(Tag + " Select {0}", NamingConversion.EncodeString(folderName));
            var rs = Execute(commandText);
            var srs = GetSelectResult(folderName, rs.Text);
            CurrentFolder = new ImapFolder(srs);
            return srs;
        }

        /// <summary>
        /// Send examine command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public SelectResult ExecuteExamine(String folderName)
        {
            ValidateState(ImapConnectionState.Authenticated);
            var commandText = String.Format(Tag + " Examine {0}", NamingConversion.EncodeString(folderName));
            var rs = Execute(commandText);
            var srs = GetSelectResult(folderName, rs.Text);
            CurrentFolder = new ImapFolder(srs);
            return srs;
        }

        private SelectResult GetSelectResult(String folderName, String text)
        {
            var rs = new ImapCommandResult(Tag, text);
            if (rs.Status == ImapCommandResultStatus.Ok)
            {
                var exists = 0;
                var recent = 0;
                var l = new List<string>();
                var m = RegexList.SelectFolderResult.Match(rs.Text);
                if (m.Success)
                {
                    Int32.TryParse(m.Groups["exst"].Value, out exists);
                    Int32.TryParse(m.Groups["rcnt"].Value, out recent);
                }
                m = RegexList.SelectFolderResultFlagsLine.Match(rs.Text);
                if (m.Success)
                {
                    var flags = m.Groups["Flags"].Value;
                    l.AddRange(from el in flags.Split(' ') where el.StartsWith("\\")  select el.Substring(1, el.Length - 1));
                }
                return new SelectResult(folderName, exists, recent, l.ToArray());
            }
            throw new MailClientException();
        }

        /// <summary>
        /// Send create folder command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteCreate(String folderName)
        {
            ValidateState(ImapConnectionState.Authenticated);
            var commandText = String.Format(Tag + " Create {0}", NamingConversion.EncodeString(folderName));
            return Execute(commandText);
        }

        /// <summary>
        /// Send delete folder command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteDelete(String folderName)
        {
            ValidateState(ImapConnectionState.Authenticated);
            var commandText = String.Format(Tag + " Delete {0}", NamingConversion.EncodeString(folderName));
            return Execute(commandText);
        }

        /// <summary>
        /// Send close command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteClose()
        {
            ValidateState(ImapConnectionState.Authenticated);
            var rs = Execute(Tag + " Close");
            CurrentFolder = null;
            return rs;
        }
        /// <summary>
        /// Send list command to IMAP server.
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public ListResult ExecuteList(String folderName, Boolean recursive)
        {
            ValidateState(ImapConnectionState.Authenticated);

            var l = new List<ListLineResult>();
            var noSelect = false;
            var hasChildren = false;
            var rc = "%";
            if (recursive )
            {
                rc = "*";
            }
            var rs = Execute(String.Format(Tag + " LIST \"{0}\" \"{1}\"", folderName, rc));
            foreach (Match m in RegexList.GetListFolderResult.Matches(rs.Text))
            {
                var name = NamingConversion.DecodeString(m.Groups["name"].Value);
                foreach (Capture c in m.Groups["opt"].Captures)
                {
                    if (c.Value == "\\Noselect")
                    {
                        noSelect = true;
                    }
                    else if (c.Value == "\\HasNoChildren")
                    {
                        hasChildren = false;
                    }
                    else if (c.Value == "\\HasChildren")
                    {
                        hasChildren = true;
                    }
                }
                l.Add(new ListLineResult(name, noSelect, hasChildren));
            }
            return new ListResult(l);
        }

        /// <summary>
        /// Send xlist command to IMAP server.
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public XListResult ExecuteXList(String folderName, Boolean recursive)
        {
            ValidateState(ImapConnectionState.Authenticated);

            var l = new List<XListLineResult>();
            var noSelect = false;
            var hasChildren = false;
            var rc = "%";
            if (recursive )
            {
                rc = "*";
            }
            var rs = Execute(String.Format(Tag + " XLIST \"{0}\" \"{1}\"", folderName, rc));
            foreach (Match m in RegexList.GetXListFolderResult.Matches(rs.Text))
            {
                var xname = "";
                var name = NamingConversion.DecodeString(m.Groups["name"].Value);
                foreach (Capture c in m.Groups["opt"].Captures)
                {
                    if (c.Value == "\\Noselect")
                    {
                        noSelect = true;
                    }
                    else if (c.Value == "\\HasNoChildren")
                    {
                        hasChildren = false;
                    }
                    else if (c.Value == "\\HasChildren")
                    {
                        hasChildren = true;
                    }
                    else if (c.Value.Length > 0)
                    {
                        xname = c.Value;
                    }
                }
                l.Add(new XListLineResult(name, noSelect, hasChildren, xname));
            }
            return new XListResult(l);
        }

        /// <summary>
        /// Send subscribe command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteSubscribe(String folderName)
        {
            ValidateState(ImapConnectionState.Authenticated);
            return Execute(Tag + " subscribe " + folderName);
        }

        /// <summary>
        /// Send unsubscribe command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteUnsubscribe(String folderName)
        {
            ValidateState(ImapConnectionState.Authenticated);
            return Execute(Tag + " Unsubscribe " + folderName);
        }

        /// <summary>
        /// Send list command to IMAP server.
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public ListResult ExecuteLsub(String folderName, Boolean recursive)
        {
            ValidateState(ImapConnectionState.Authenticated);

            var l = new List<ListLineResult>();
            var noSelect = false;
            var hasChildren = false;
            var rc = "%";
            if (recursive )
            {
                rc = "*";
            }
            var rs = Execute(String.Format(Tag + " Lsub \"{0}\" \"{1}\"", folderName, rc));
            foreach (Match m in RegexList.GetLsubFolderResult.Matches(rs.Text))
            {
                var name = NamingConversion.DecodeString(m.Groups["name"].Value);
                foreach (Capture c in m.Groups["opt"].Captures)
                {
                    if (c.Value == "\\Noselect")
                    {
                        noSelect = true;
                    }
                    else if (c.Value == "\\HasNoChildren")
                    {
                        hasChildren = false;
                    }
                    else if (c.Value == "\\HasChildren")
                    {
                        hasChildren = true;
                    }
                }
                l.Add(new ListLineResult(name, noSelect, hasChildren));
            }
            return new ListResult(l);
        }

        /// <summary>
        /// Send Fetch command to IMAP server.
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <returns></returns>
        public MailMessage ExecuteFetch(Int64 mailIndex)
        {
            ValidateState(ImapConnectionState.Authenticated, true);
            var rs = Execute(String.Format(Tag + " FETCH {0} (BODY[])", mailIndex));
            var messageRegex = new Regex(@"^\* \d+ FETCH \([^\r\n]*BODY\[\] \{\d+\}\r\n(?<msg>.*?)\)\r\n" + Tag + " OK"
                , RegexOptions.Multiline | RegexOptions.Singleline);
            var m = messageRegex.Match(rs.Text);
            if (m.Success)
            {
                return new MailMessage(m.Groups["msg"].Value, mailIndex);
            }
            throw new MailClientException();
        }

        /// <summary>
        /// Send search command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public SearchResult ExecuteSearch(String searchText)
        {
            ValidateState(ImapConnectionState.Authenticated, true);
            var rs = Execute(Tag + " SEARCH " + searchText);
            return new SearchResult(Tag, rs.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <param name="command"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public ImapCommandResult ExecuteStore(Int64 mailIndex, StoreItem command, String flags)
        {
            ValidateState(ImapConnectionState.Authenticated, true);
            var sb = new StringBuilder(256);
            sb.Append(Tag);
            sb.Append(" STORE ");
            sb.Append(mailIndex);
            sb.Append(" ");
            if (command == StoreItem.FlagsReplace)
            {
                sb.Append("FLAGS ");
            }
            else if (command == StoreItem.FlagsReplaceSilent)
            {
                sb.Append("FLAGS.SILENT ");
            }
            else if (command == StoreItem.FlagsAdd)
            {
                sb.Append("+FLAGS ");
            }
            else if (command == StoreItem.FlagsAddSilent)
            {
                sb.Append("+FLAGS.SILENT ");
            }
            else if (command == StoreItem.FlagsRemove)
            {
                sb.Append("-FLAGS ");
            }
            else if (command == StoreItem.FlagsRemoveSilent)
            {
                sb.Append("-FLAGS.SILENT ");
            }
            else
            {
                throw new ArgumentException("command");
            }
            if (String.IsNullOrEmpty(flags))
            {
                throw new ArgumentException("flags");
            }
            sb.Append("(");
            sb.Append(flags);
            sb.Append(")");

            return Execute(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="mailData"></param>
        /// <returns></returns>
        public ImapCommandResult ExecuteAppend(String folderName, String mailData)
        {
            return ExecuteAppend(folderName, mailData, "", DateTimeOffset.Now);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="mailData"></param>
        /// <param name="flag"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public ImapCommandResult ExecuteAppend(String folderName, String mailData, String flag, DateTimeOffset datetime)
        {
            var commandText = String.Format(Tag + " APPEND \"{0}\" ({1}) \"{2}\" "
                , NamingConversion.EncodeString(folderName), flag, MailParser.DateTimeOffsetString(datetime));
            commandText += "{" + mailData.Length + "}";
            var rs = Execute(commandText + Environment.NewLine + mailData);
            return new ImapCommandResult(Tag, rs.Text);
        }

        /// <summary>
        /// Send rename folder command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteRename(String oldFolderName, String folderName)
        {
            ValidateState(ImapConnectionState.Authenticated);
            var commandText = String.Format(Tag + " Rename {0} {1}", NamingConversion.EncodeString(oldFolderName), NamingConversion.EncodeString(folderName));
            var rs = Execute(commandText);
            if (rs.Status == ImapCommandResultStatus.Ok ||
                rs.Status == ImapCommandResultStatus.None)
            {
                return rs;
            }
            return rs;
        }

        /// <summary>
        /// Send rlsub command to IMAP server.
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public ListResult ExecuteRlsub(String folderName, Boolean recursive)
        {
            ValidateState(ImapConnectionState.Authenticated);

            var l = new List<ListLineResult>();
            var noSelect = false;
            var hasChildren = false;
            var rc = "%";
            if (recursive )
            {
                rc = "*";
            }
            var rs = Execute(String.Format(Tag + " RLSUB \"{0}\" \"{1}\"", folderName, rc));
            foreach (Match m in RegexList.GetRlsubFolderResult.Matches(rs.Text))
            {
                var name = NamingConversion.DecodeString(m.Groups["name"].Value);
                foreach (Capture c in m.Groups["opt"].Captures)
                {
                    if (c.Value == "\\Noselect")
                    {
                        noSelect = true;
                    }
                    else if (c.Value == "\\HasNoChildren")
                    {
                        hasChildren = false;
                    }
                    else if (c.Value == "\\HasChildren")
                    {
                        hasChildren = true;
                    }
                }
                l.Add(new ListLineResult(name, noSelect, hasChildren));
            }
            return new ListResult(l);

        }

        /// <summary>
        /// Send status command to IMAP server.
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="message"></param>
        /// <param name="recent"></param>
        /// <param name="uidnext"></param>
        /// <param name="uidvalidity"></param>
        /// <param name="unseen"></param>
        /// <returns></returns>
        public ImapCommandResult ExecuteStatus(String folderName, Boolean message, Boolean recent, Boolean uidnext, Boolean uidvalidity, Boolean unseen)
        {
            ValidateState(ImapConnectionState.Authenticated);
            var sb = new StringBuilder(256);
            sb.Append(Tag);
            sb.Append(" Status");
            sb.Append(" ");
            sb.Append(folderName);
            if (message || recent || uidnext || uidvalidity || unseen)
            {
                sb.Append(" ");
                sb.Append("(");
                if (message)
                {
                    sb.Append("messages");
                }

                if (recent)
                {
                    sb.Append(" recent");
                }

                if (uidnext)
                {
                    sb.Append(" uidnext");
                }

                if (uidvalidity)
                {
                    sb.Append(" uidvalidity");
                }

                if (unseen)
                {
                    sb.Append(" unseen");
                }

                sb.Append(")");
            }

            var rs = Execute(sb.ToString());
            if (rs.Status == ImapCommandResultStatus.Ok ||
                rs.Status == ImapCommandResultStatus.None)
            {
                return rs;
            }
            return rs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteCheck()
        {
            ValidateState(ImapConnectionState.Authenticated, true);
            return Execute(Tag + " Check");
        }
        /// <summary>
        /// Send copy command to IMAP server.
        /// </summary>
        /// <param name="mailindexstart"></param>
        /// <param name="mailindexend"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public ImapCommandResult ExecuteCopy(Int32 mailindexstart, Int32 mailindexend, String folderName)
        {
            ValidateState(ImapConnectionState.Authenticated);
            var sb = new StringBuilder(256);
            sb.Append(Tag);
            sb.Append(" Copy ");
            if (!Equals(mailindexstart, 0))
            {
                sb.Append(mailindexstart);
            }
            if (!Equals(mailindexend, 0) && !Equals(mailindexstart, 0))
            {
                sb.Append(":");
                sb.Append(mailindexend);
            }
            else if (!Equals(mailindexend, 0))
            {
                sb.Append(mailindexend);
            }
            sb.Append(" ");
            sb.Append(folderName);

            var rs = Execute(sb.ToString());
            if (rs.Status == ImapCommandResultStatus.Ok ||
                rs.Status == ImapCommandResultStatus.None)
            {
                return rs;
            }
            return rs;

        }

        /// <summary>
        /// Send UID command to IMAP server.
        /// <param name="command"></param>
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteUid(String command)
        {
            ValidateState(ImapConnectionState.Authenticated);
            return Execute(Tag + " UID " + command);
        }

        /// <summary>
        /// Send NAMESPACE command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteNamespace()
        {
            ValidateState(ImapConnectionState.Authenticated);
            return Execute(Tag + " NAMESPACE");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImapIdleCommand CreateImapIdleCommand()
        {
            return new ImapIdleCommand(Tag, ResponseEncoding);
        }

        /// <summary>
        /// Send IDLE command to IMAP server.
        /// You can receive message from server by register event handler to MessageReceived event of ImapIdleCommand object
        /// </summary>
        /// <returns></returns>
        public void ExecuteIdle(ImapIdleCommand command)
        {
            ValidateState(ImapConnectionState.Authenticated);
            Send(Tag + " IDLE");
            var bb = command.GetByteArray();
            command.AsyncResult = Stream.BeginRead(bb, 0, bb.Length, ExecuteIdleCallback, command);
            _state = ImapConnectionState.Idle;
        }

        private void ExecuteIdleCallback(IAsyncResult result)
        {
            DataReceiveContext cx = null;

            try
            {
                cx = (DataReceiveContext)result.AsyncState;
                if (Socket == null)
                {
                    throw new SocketClientException("Connection is closed");
                }
                var size = Stream.EndRead(result);
                if (cx.ReadBuffer(size) )
                {
                    var bb = cx.GetByteArray();
                    Stream.BeginRead(bb, 0, bb.Length, GetResponseCallback, cx);
                }
                else
                {
                    cx.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (cx != null) cx.Exception = ex;
                OnError(ex);
            }
            finally
            {
                if (cx != null && cx.Exception != null)
                {
                    cx.Dispose();
                }
            }
        }

        /// <summary>
        /// Send done command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteDone(ImapIdleCommand command)
        {
            ValidateState(ImapConnectionState.Idle);
            if (command.AsyncResult != null)
            {
                var x = Stream.EndRead(command.AsyncResult);
            }
            var rs = Execute("DONE");
            _state = ImapConnectionState.Authenticated;
            return rs;
        }

        /// <summary>
        /// Send GetQuota command to IMAP server.
        /// <param name="resourceName"></param>
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteGetQuota(String resourceName)
        {
            return Execute(Tag + " GetQuota " + resourceName);
        }

        /// <summary>
        /// Send SETQUOTA command to IMAP server.
        /// <param name="resourceName"></param>
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteSetQuota(String resourceName)
        {
            return Execute(Tag + " SETQUOTA " + resourceName);
        }

        /// <summary>
        /// Send GETQUOTAROOT command to IMAP server.
        /// <param name="folderName"></param>
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteGetQuotaRoot(String folderName)
        {
            var rs = Execute(Tag + " GETQUOTAROOT " + folderName);
            var commandText =String.Format(Tag + " GETQUOTAROOT {0}", NamingConversion.EncodeString(folderName));
            return rs;
        }

        /// <summary>
        /// Send noop command to IMAP server.
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteNoop()
        {
            ValidateState(ImapConnectionState.Authenticated);
            return Execute(Tag + " NOOP");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImapCommandResult ExecuteExpunge()
        {
            ValidateState(ImapConnectionState.Authenticated);
            return Execute(Tag + " EXPUNGE");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        public ImapFolder SelectFolder(String folderName)
        {
            var rs = ExecuteSelect(folderName);
            return new ImapFolder(rs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        public void UnselectFolder(String folderName)
        {
            ExecuteClose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ImapFolder> GetAllFolders()
        {
            return GetFolders("", true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public List<ImapFolder> GetFolders(String folderName, Boolean recursive)
        {
            ValidateState(ImapConnectionState.Authenticated);
            var rs = ExecuteList(folderName, recursive);
            return rs.Lines.Select(el => new ImapFolder(el)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <returns></returns>
        public MailMessage GetMessage(Int64 mailIndex)
        {
            return ExecuteFetch(mailIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <returns></returns>
        public Boolean ReadMail(params Int64[] mailIndex)
        {
            ValidateState(ImapConnectionState.Authenticated, true);
            return ReadMail(CurrentFolder.Name, mailIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="mailIndex"></param>
        /// <returns></returns>
        public Boolean ReadMail(String folderName, params Int64[] mailIndex)
        {
            return SetMailStore(StoreItem.FlagsRemove, "UnSeen", mailIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <returns></returns>
        public Boolean UnReadMail(params Int64[] mailIndex)
        {
            ValidateState(ImapConnectionState.Authenticated, true);
            return ReadMail(CurrentFolder.Name, mailIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="mailIndex"></param>
        /// <returns></returns>
        public Boolean UnReadMail(String folderName, params Int64[] mailIndex)
        {
            return SetMailStore(StoreItem.FlagsAdd, "UnSeen", mailIndex);
        }

        private Boolean SetMailStore(StoreItem storeItem, String value, params Int64[] mailIndex)
        {
            if (EnsureOpen() == ImapConnectionState.Disconnected) { return false; }
            if (Authenticate() == false) { return false; }

            if (mailIndex.Select(t => ExecuteStore(t, storeItem, value)).Any(rs => rs.Status != ImapCommandResultStatus.Ok))
            {
                return false;
            }
            ExecuteLogout();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailIndex"></param>
        /// <returns></returns>
        public Boolean DeleteMail(params Int64[] mailIndex)
        {
            ValidateState(ImapConnectionState.Authenticated, true);
            return DeleteMail(CurrentFolder.Name, mailIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="mailIndex"></param>
        /// <returns></returns>
        public Boolean DeleteMail(String folderName, params Int64[] mailIndex)
        {
            if (EnsureOpen() == ImapConnectionState.Disconnected) { return false; }
            if (Authenticate() == false) { return false; }

            if (mailIndex.Select(t => ExecuteStore(t, StoreItem.FlagsAdd, @"\Deleted")).Any(rs => rs.Status != ImapCommandResultStatus.Ok))
            {
                return false;
            }
            ExecuteExpunge();
            ExecuteLogout();
            return true;
        }

        /// <summary>
        /// disconnect connection to IMAP server.
        /// </summary>
        public override void Close()
        {
            base.Close();
            _state = ImapConnectionState.Disconnected;
        }

        /// <summary>
        /// 
        /// </summary>
        ~ImapClient()
        {
            Dispose(false);
        }
    }
}