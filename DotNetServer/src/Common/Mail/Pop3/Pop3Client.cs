using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Mail.Async;
using Common.Mail.Common;
using Common.Mail.Pop3.Command;
using Common.Net.SocketClient;

namespace Common.Mail.Pop3
{
	/// Represent and probide functionality about pop3 command.
	/// <summary>
	/// Represent and probide functionality about pop3 command.
	/// </summary>
	public class Pop3Client : SocketClient, IDisposable
	{
        /// <summary>
        /// 
        /// </summary>
        public static readonly Int32 DefaultPort = 110;
        private Pop3AuthenticateMode _mode = Pop3AuthenticateMode.Pop;
		private Pop3ConnectionState _state = Pop3ConnectionState.Disconnected;
        /// To get or set the authentication method.
		/// <summary>
		/// Get or set how authenticate to server.
        /// To get or set the authentication method.
		/// </summary>
		public Pop3AuthenticateMode AuthenticateMode
		{
			get { return _mode; }
			set { _mode = value; }
		}

        /// To indicate the status of connections to get the value.
		/// <summary>
		/// Get connection state.
        /// To indicate the status of connections to get the value.
		/// </summary>
		public Pop3ConnectionState State
		{
            get
            {
                if (Connected == false)
                {
                    _state = Pop3ConnectionState.Disconnected;
                }
                return _state;
            }
            private set { _state = value; }
		}

        /// Connected to the server gets a value that indicates whether or not.
		/// <summary>
		/// Get connection is ready.
        /// Connected to the server gets a value that indicates whether or not.
		/// </summary>
		public Boolean Available
		{
			get { return _state != Pop3ConnectionState.Disconnected; }
		}

		/// <summary>
		/// 
		/// </summary>
        public Pop3Client(String serverName)
            : base(serverName, DefaultPort)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
		public Pop3Client(String serverName, Int32 port, String userName, String password)
            : base(serverName, port)
		{
			UserName = userName;
			Password = password;
		}

        /// Opens a connection to a server.
		/// <summary>
		/// Open connection to a server.
        /// Opens a connection to a server.
		/// </summary>
		public Pop3ConnectionState Open()
		{
            if (Connect())
            {
                var response = GetResponse();
                if (response.Ok)
                {
                    State = Pop3ConnectionState.Connected;
                }
                else
                {
                    Close();
                }
            }
            else
            {
                State = Pop3ConnectionState.Disconnected;
            }
            return _state;
		}

        /// The connection to the server if it is not already open, and opens a connection to a server.
		/// <summary>
		/// Ensure connection is opened.
        /// The connection to the server if it is not already open, and opens a connection to a server.
		/// </summary>
		public Pop3ConnectionState EnsureOpen()
		{
			if (Socket != null)
			{ return _state; }

			return Open();
		}

		private void CheckAuthenticate()
		{
			if (_state == Pop3ConnectionState.Authenticated) { return; }
            throw new MailClientException("You must authenticate to pop3 server before executing this command.");
		}

        private void CheckResponseError(Pop3CommandResult result)
        {
            if (result.Ok) { return; }
            throw new MailClientException(result.Text);
        }

        private Pop3CommandResult GetResponse()
		{
			return GetResponse(false);
		}

		private Pop3CommandResult GetResponse(Boolean isMultiLine)
		{
			var ms = new MemoryStream();
			GetResponse(ms, isMultiLine);
			var s = ResponseEncoding.GetString(ms.ToArray());
            return new Pop3CommandResult(s);
		}

	    private void GetResponse(Stream stream, Boolean isMultiLine)
		{
            var bb = GetResponseBytes(new Pop3DataReceiveContext(ResponseEncoding, isMultiLine));
            stream.Write(bb, 0, bb.Length);
            Commnicating = false;
		}

        /// Log in to POP3 e-mail server.
		/// <summary>
		/// Log in to pop3 server.
        /// Log in to POP3 e-mail server.
		/// </summary>
		/// <returns></returns>
		public Boolean Authenticate()
		{
			if (_mode == Pop3AuthenticateMode.Auto)
			{
				if (AuthenticateByPop())
				{
					_mode = Pop3AuthenticateMode.Pop;
					return true;
				}
			    if (AuthenticateByAPop())
			    {
			        _mode = Pop3AuthenticateMode.APop;
			        return true;
			    }
			    return false;
			}
            switch (_mode)
            {
                case Pop3AuthenticateMode.Pop: return AuthenticateByPop();
                case Pop3AuthenticateMode.APop: return AuthenticateByAPop();
            }
            return false;
		}

        /// 3 POP POP Authentication log in to the mail server.
		/// <summary>
		/// Log in to pop3 server by POP authenticate.
        /// 3 POP POP Authentication log in to the mail server.
		/// </summary>
		/// <returns></returns>
		public Boolean AuthenticateByPop()
		{
            if (EnsureOpen() == Pop3ConnectionState.Connected)
			{
			    //Send User Name
			    var rs = Execute("user " + UserName, false);
			    if (rs.Ok)
				{
                    //Send Password
					rs = Execute("pass " + Password, false);
					if (rs.Ok)
					{
						_state = Pop3ConnectionState.Authenticated;
					}
				}
			}
            return _state == Pop3ConnectionState.Authenticated;
		}

        /// 3 POP APOP Authentication log in to the mail server.
		/// <summary>
		/// Log in to pop3 server by A-POP authenticate.
        /// 3 POP APOP Authentication log in to the mail server.
		/// </summary>
		/// <returns></returns>
		public Boolean AuthenticateByAPop()
		{
            if (EnsureOpen() == Pop3ConnectionState.Connected)
            {
                //Send User Name
                var rs = Execute("user " + UserName, false);
                if (rs.Ok)
				{
					if (rs.Text.IndexOf("<", StringComparison.Ordinal) > -1 &&
						rs.Text.IndexOf(">", StringComparison.Ordinal) > -1)
					{
						var startIndex = rs.Text.IndexOf("<", StringComparison.Ordinal);
						var endIndex = rs.Text.IndexOf(">", StringComparison.Ordinal);
						var timeStamp = rs.Text.Substring(startIndex, endIndex - startIndex + 1);
                        //Send Password
						rs = Execute("pass " + Cryptography.ToMd5DigestString(timeStamp + Password), false);
						if (rs.Ok)
						{
							_state = Pop3ConnectionState.Authenticated;
						}
					}
				}
            }
            return _state == Pop3ConnectionState.Authenticated;
		}

        /// Synchronization POP3 e-mail server to send a command to the command, depending on the type of data received by the response returns it as a string.
		/// <summary>
		/// Send a command with synchronous and get response data as string text if the command is a type to get response.
        /// Synchronization POP3 e-mail server to send a command to the command, depending on the type of data received by the response returns it as a string.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
        public Pop3CommandResult Execute(Pop3Command command)
		{
            var isResponseMultiLine = command is TopCommand ||
                                       command is RetrCommand ||
                                       command is ListCommand ||
                                       command is UidlCommand;

            return Execute(command.GetCommandString(), isResponseMultiLine);
		}

        /// Synchronization POP3 e-mail server to send a command to the command, depending on the type of data received by the response returns it as a string.
		/// <summary>
		/// Send a command with synchronous and get response data as string text if the command is a type to get response.
        /// Synchronization POP3 e-mail server to send a command to the command, depending on the type of data received by the response returns it as a string.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="isMultiLine"></param>
		/// <returns></returns>
        private Pop3CommandResult Execute(String command, Boolean isMultiLine)
		{
			Send(command);
			Commnicating = true;
			return GetResponse(isMultiLine);
		}

        /// Synchronization POP3 e-mail server to send a command to the command, depending on the type of data received by the response returns it as a string.
		/// <summary>
		/// Send a command with synchronous and get response data as string text if the command is a type to get response.
        /// Synchronization POP3 e-mail server to send a command to the command, depending on the type of data received by the response returns it as a string.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="command"></param>
		/// <returns></returns>
		public void Execute(Stream stream, Pop3Command command)
		{
            var isResponseMultiLine = command is TopCommand ||
                                       command is RetrCommand ||
                                       command is ListCommand ||
                                       command is UidlCommand;

            Execute(stream, command.GetCommandString(), isResponseMultiLine);
		}

        /// Synchronization POP3 e-mail server to send a command to the command, depending on the type of data received by the response returns it as a string.
		/// <summary>
		/// Send a command with synchronous and get response data as string text if the command is a type to get response.
        /// Synchronization POP3 e-mail server to send a command to the command, depending on the type of data received by the response returns it as a string.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="command"></param>
		/// <param name="isMultiLine"></param>
		/// <returns></returns>
		private void Execute(Stream stream, String command, Boolean isMultiLine)
		{
			Send(command);
			Commnicating = true;
			GetResponse(stream, isMultiLine);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="isMultiLine"></param>
        /// <param name="callbackFunction"></param>
        public void BeginExecute(String command, Boolean isMultiLine, Action<Pop3CommandResult> callbackFunction)
        {
            BeginSend(command, new Pop3DataReceiveContext(ResponseEncoding, isMultiLine, s => callbackFunction(new Pop3CommandResult(s)))
                , s => callbackFunction(new Pop3CommandResult(s)));
        }

        /// Asynchronous POP3 e-mail to send a command to the server. Received response string is callbackFunction can be obtained as the argument.
		/// <summary>
		/// Send a command with asynchronous and get response text by first parameter of callbackFunction.
        /// Asynchronous POP3 e-mail to send a command to the server. Received response string is callbackFunction can be obtained as the argument.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="callbackFunction"></param>
        public void BeginExecute(Pop3Command command, Action<Pop3CommandResult> callbackFunction)
		{
            var isMultiLine = command is TopCommand ||
                               command is RetrCommand ||
                               command is ListCommand ||
                               command is UidlCommand;

            BeginExecute(command.GetCommandString(), isMultiLine, callbackFunction);
		}

        /// List POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send list command to pop3 server.
        /// List POP3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public List<ListCommandResult> ExecuteList(ListCommand command)
		{
			var l = new List<ListCommandResult>();
			if (command.MailIndex.HasValue)
			{
				var rs = ExecuteList(command.MailIndex.Value);
				l.Add(rs);
			}
			else
			{
				l = ExecuteList();
			}
			return l;
		}

        /// List POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send list command to pop3 server.
        /// List POP3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <returns></returns>
		public ListCommandResult ExecuteList(Int64 mailIndex)
		{
			var cm = new ListCommand(mailIndex);

            CheckAuthenticate();
			var rs = Execute(cm);
            CheckResponseError(rs);
			return new ListCommandResult(rs.Text);
		}

        /// List POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send list command to pop3 server.
        /// List POP3 e-mail to send a command to the server.
		/// </summary>
		/// <returns></returns>
		public List<ListCommandResult> ExecuteList()
		{
			var cm = new ListCommand();
			var l = new List<ListCommandResult>();

            CheckAuthenticate();
			var rs = Execute(cm);
            CheckResponseError(rs);
            
            var sr = new StringReader(rs.Text);
			while (sr.Peek() > -1)
			{
				var line = sr.ReadLine();
				if (line == ".")
				{ break; }
				if (line != null && line.StartsWith("+OK", StringComparison.OrdinalIgnoreCase))
				{ continue; }

				l.Add(new ListCommandResult(line));
			}
			return l;
		}

        /// UIDL POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send uidl command to pop3 server.
        /// UIDL POP3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <returns></returns>
		public UidlCommandResult ExecuteUidl(Int64 mailIndex)
		{
			var cm = new UidlCommand(mailIndex);

            CheckAuthenticate();
			var rs = Execute(cm);
            CheckResponseError(rs);

            return new UidlCommandResult(rs.Text);
		}

        /// UIDL POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send uidl command to pop3 server.
        /// UIDL POP3 e-mail to send a command to the server.
		/// </summary>
		/// <returns></returns>
		public List<UidlCommandResult> ExecuteUidl()
		{
			var cm = new UidlCommand();
			var l = new List<UidlCommandResult>();

            CheckAuthenticate();
			var rs = Execute(cm);
            CheckResponseError(rs);
            
            var sr = new StringReader(rs.Text);
			while (sr.Peek() > -1)
			{
				var line = sr.ReadLine();
				if (line == ".")
				{ break; }
				if (line != null && line.StartsWith("+OK", StringComparison.OrdinalIgnoreCase))
				{ continue; }

				l.Add(new UidlCommandResult(line));
			}
			return l;
		}

        /// RETR POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send retr command to pop3 server.
        /// RETR POP3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <returns></returns>
		public MailMessage ExecuteRetr(Int64 mailIndex)
		{
			return GetMessage(mailIndex, Int32.MaxValue);
		}

        /// TOP POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send top command to pop3 server.
        /// TOP POP3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="lineCount"></param>
		/// <returns></returns>
		public MailMessage ExecuteTop(Int64 mailIndex, Int32 lineCount)
		{
			return GetMessage(mailIndex, lineCount);
		}

        /// DELE POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send dele command to pop3 server.
        /// DELE POP3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <returns></returns>
		public Pop3CommandResult ExecuteDele(Int64 mailIndex)
		{
			var cm = new DeleCommand(mailIndex);

            CheckAuthenticate();
			var rs = Execute(cm);
            CheckResponseError(rs);
			return rs;
		}

        /// STAT POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send stat command to pop3 server.
        /// STAT POP3 e-mail to send a command to the server.
		/// </summary>
		/// <returns></returns>
		public StatCommandResult ExecuteStat()
		{
            CheckAuthenticate();
            var rs = Execute("Stat", false);
            CheckResponseError(rs);
            return new StatCommandResult(rs.Text);
		}

        /// NOOP POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send noop command to pop3 server.
        /// NOOP POP3 e-mail to send a command to the server.
		/// </summary>
		/// <returns></returns>
		public Pop3CommandResult ExecuteNoop()
		{
            EnsureOpen();
			var rs = Execute("Noop", false);
			return new Pop3CommandResult(rs.Text);
		}

        /// RESET POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send reset command to pop3 server.
        /// RESET POP3 e-mail to send a command to the server.
		/// </summary>
		/// <returns></returns>
		public Pop3CommandResult ExecuteRset()
		{
            CheckAuthenticate();
            var rs = Execute("Rset", false);
            CheckResponseError(rs);
			return rs;
		}

        /// QUIT POP3 e-mail to send a command to the server.
		/// <summary>
		/// Send quit command to pop3 server.
        /// QUIT POP3 e-mail to send a command to the server.
		/// </summary>
		/// <returns></returns>
		public Pop3CommandResult ExecuteQuit()
		{
            EnsureOpen();
            var rs = Execute("Quit", false);
            CheckResponseError(rs);
			return rs;
		}

        /// Total number of email messages in your mailbox.
		/// <summary>
		/// Get total mail count at mailbox.
        /// Total number of email messages in your mailbox.
		/// </summary>
		/// <returns></returns>
		public Int64 GetTotalMessageCount()
        {
            var rs = ExecuteStat();
            return rs.TotalMessageCount;
        }

	    /// Specified MailIndex email to retrieve data.
		/// <summary>
		/// Get mail data of specified mail index.
        /// Specified MailIndex email to retrieve data.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <returns></returns>
		public MailMessage GetMessage(Int64 mailIndex)
		{
			MailMessage mailMessage;

	        CheckAuthenticate();
            var rs = Execute(new RetrCommand(mailIndex));
            CheckResponseError(rs);
            try
			{
                mailMessage = new MailMessage(rs.Text, mailIndex);
			}
			catch (Exception ex)
			{
				throw new InvalidMailMessageException(ex);
			}
            return mailMessage;
		}

        /// Specified MailIndex email data from the body and to specify the number of rows.
		/// <summary>
		/// Get mail data of specified mail index with indicate body line count.
        /// Specified MailIndex email data from the body and to specify the number of rows.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="lineCount"></param>
		/// <returns></returns>
		public MailMessage GetMessage(Int64 mailIndex, Int32 lineCount)
		{
            MailMessage mailMessage;

            CheckAuthenticate();
            var rs = Execute(new TopCommand(mailIndex, lineCount));
            CheckResponseError(rs);
            try
			{
                mailMessage = new MailMessage(rs.Text, mailIndex);
			}
			catch (Exception ex)
			{
                throw new InvalidMailMessageException(ex);
			}
            return mailMessage;
		}

        /// Specified MailIndex email data, as a string.
		/// <summary>
		/// Get mail text of specified mail index.
        /// Specified MailIndex email data, as a string.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <returns></returns>
		public String GetMessageText(Int64 mailIndex)
		{
            CheckAuthenticate();
			try
			{
				var cm = new RetrCommand(mailIndex);
				var rs = Execute(cm);
                return rs.Text;
			}
			catch (Exception ex)
			{
                throw new MailClientException(ex);
			}
		}

        /// Specified MailIndex email a string of data to specify the number of rows of the body.
		/// <summary>
		/// Get mail text of specified mail index with indicate body line count.
        /// Specified MailIndex email a string of data to specify the number of rows of the body.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="lineCount"></param>
		/// <returns></returns>
		public String GetMessageText(Int64 mailIndex, Int32 lineCount)
		{
            CheckAuthenticate();
			try
			{
				var cm = new TopCommand(mailIndex, lineCount);
				var rs = Execute(cm);
                return rs.Text;
			}
			catch (Exception ex)
			{
                throw new MailClientException(ex);
			}
		}

        /// Specified MailIndex the mail data from the body of the string to specify the number of rows in the output stream.
		/// <summary>
		/// Get mail text of specified mail index with indicate body line count.
        /// Specified MailIndex the mail data from the body of the string to specify the number of rows in the output stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="mailIndex"></param>
		/// <returns></returns>
		public void GetMessageText(Stream stream, Int64 mailIndex)
		{
            CheckAuthenticate();
			try
			{
			    var cm = new RetrCommand(mailIndex);
			    Execute(stream, cm);
			}
			catch (Exception ex)
			{
                throw new MailClientException(ex);
			}
		}

        /// Specified MailIndex the mail data from the body of the string to specify the number of rows in the output stream.
		/// <summary>
		/// Get mail text of specified mail index with indicate body line count.
        /// Specified MailIndex the mail data from the body of the string to specify the number of rows in the output stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="mailIndex"></param>
		/// <param name="lineCount"></param>
		/// <returns></returns>
		public void GetMessageText(Stream stream, Int64 mailIndex, Int32 lineCount)
		{
            CheckAuthenticate();
			try
			{
			    var cm = new TopCommand(mailIndex, lineCount);
			    Execute(stream, cm);
			}
			catch (Exception ex)
			{
                throw new MailClientException(ex);
			}
		}

        /// Specified MailIndex asynchronously, the email data, as a string.
		/// <summary>
		/// Get mail text of specified mail index by asynchronous request.
        /// Specified MailIndex asynchronously, the email data, as a string.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="callbackFunction"></param>
        public void GetMessageText(Int64 mailIndex, Action<Pop3CommandResult> callbackFunction)
		{
            var md = callbackFunction;

			CheckAuthenticate();
			var cm = new RetrCommand(mailIndex);
			BeginExecute(cm, md);
		}

        /// Specified MailIndex removed from the mail to the mail server.
		/// <summary>
		/// Set delete flag to specify mail index.
		/// To complete delete execution,call quit command after calling dele command.
        /// Specified MailIndex removed from the mail to the mail server.
        /// In addition to actually remove QUIT in command with the delete operation will need to be completed.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <returns></returns>
		public Boolean DeleteMail(params Int64[] mailIndex)
		{
            if (EnsureOpen() == Pop3ConnectionState.Disconnected) { return false; }
            if (Authenticate() == false) { return false; }
            if (mailIndex.Select(t => new DeleCommand(t)).Select(Execute).Any(rs => rs.Ok == false))
            {
                return false;
            }
            ExecuteQuit();
            return true;
		}

        /// Asynchronous POP List3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous list command to pop3 server.
        /// Asynchronous POP List3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="callbackFunction"></param>
		/// <returns></returns>
		public void ExecuteList(Int64 mailIndex, Action<List<ListCommandResult>> callbackFunction)
		{
			var cm = new ListCommand(mailIndex);

			Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                var l = new List<ListCommandResult>();
				var rs = new ListCommandResult(response.Text);
				l.Add(rs);
				callbackFunction(l);
			};
			CheckAuthenticate();
			BeginExecute(cm, md);
		}

        /// Asynchronous POP List3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous list command to pop3 server.
        /// Asynchronous POP List3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="callbackFunction"></param>
		/// <returns></returns>
		public void ExecuteList(Action<List<ListCommandResult>> callbackFunction)
		{
			var cm = new ListCommand();

			Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                var commandList = new List<ListCommandResult>();

			    var sr = new StringReader(response.Text);
				while (sr.Peek() > -1)
				{
					var line = sr.ReadLine();
					if (line == ".")
					{ break; }
					if (line != null && line.StartsWith("+OK", StringComparison.OrdinalIgnoreCase))
					{ continue; }

					commandList.Add(new ListCommandResult(line));
				}
				callbackFunction(commandList);
			};
			CheckAuthenticate();
			BeginExecute(cm, md);
		}

        /// Asynchronous POP UIDL3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous uidl command to pop3 server.
        /// Asynchronous POP UIDL3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="callbackFunction"></param>
		public void ExecuteUidl(Int64 mailIndex, Action<UidlCommandResult[]> callbackFunction)
		{
			var cm = new UidlCommand(mailIndex);

			Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                var rs = new UidlCommandResult[1];
				rs[0] = new UidlCommandResult(response.Text);
				callbackFunction(rs);
			};
			CheckAuthenticate();
			BeginExecute(cm, md);
		}

        /// Asynchronous POP UIDL3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous uidl command to pop3 server.
        /// Asynchronous POP UIDL3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="callbackFunction"></param>
		public void ExecuteUidl(Action<List<UidlCommandResult>> callbackFunction)
		{
			var cm = new UidlCommand();

			Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                var l = new List<UidlCommandResult>();

			    var sr = new StringReader(response.Text);
				while (sr.Peek() > -1)
				{
					var line = sr.ReadLine();
					if (line == ".")
					{ break; }
					if (line != null && line.StartsWith("+OK", StringComparison.OrdinalIgnoreCase))
					{ continue; }

					l.Add(new UidlCommandResult(line));
				}
				callbackFunction(l);
			};
			CheckAuthenticate();
			BeginExecute(cm, md);
		}

        /// Specified MailIndex asynchronously, the email to retrieve data.
		/// <summary>
		/// Get mail data by asynchronous request.
        /// Specified MailIndex asynchronously, the email to retrieve data.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="callbackFunction"></param>
		public void GetMessage(Int64 mailIndex, Action<MailMessage> callbackFunction)
		{
            Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                callbackFunction(new MailMessage(response.Text, mailIndex));
			};
			CheckAuthenticate();
			var cm = new RetrCommand(mailIndex);
			BeginExecute(cm, md);
		}

        /// Asynchronous POP RETR3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous retr command to pop3 server.
        /// Asynchronous POP RETR3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="mailIndex"></param>
		/// <param name="callbackFunction"></param>
		public void ExecuteRetr(Int64 mailIndex, Action<MailMessage> callbackFunction)
		{
            Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                callbackFunction(new MailMessage(response.Text, mailIndex));
			};
			CheckAuthenticate();
			var cm = new RetrCommand(mailIndex);
			BeginExecute(cm, md);
		}

        /// Asynchronous POP STAT3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous stat command to pop3 server.
        /// Asynchronous POP STAT3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="callbackFunction"></param>
		public void ExecuteStat(Action<StatCommandResult> callbackFunction)
		{
			Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                callbackFunction(new StatCommandResult(response.Text));
			};
			CheckAuthenticate();
			BeginExecute("Stat", false, md);
		}

        /// Asynchronous POP NOOP3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous noop command to pop3 server.
        /// Asynchronous POP NOOP3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="callbackFunction"></param>
		public void ExecuteNoop(Action<Pop3CommandResult> callbackFunction)
		{
			Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                callbackFunction(response);
			};
			EnsureOpen();
            BeginExecute("Noop", false, md);
        }

        /// Asynchronous POP RESET3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous reset command to pop3 server.
        /// Asynchronous POP RESET3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="callbackFunction"></param>
		public void ExecuteRset(Action<Pop3CommandResult> callbackFunction)
		{
			Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                callbackFunction(response);
			};
			CheckAuthenticate();
            BeginExecute("Rset", false, md);
		}

        /// Asynchronous POP QUIT3 e-mail to send a command to the server.
		/// <summary>
		/// Send asynchronous quit command to pop3 server.
        /// Asynchronous POP QUIT3 e-mail to send a command to the server.
		/// </summary>
		/// <param name="callbackFunction"></param>
		public void ExecuteQuit(Action<Pop3CommandResult> callbackFunction)
		{
            Action<Pop3CommandResult> md = response =>
			{
                CheckResponseError(response);
                callbackFunction(response);
			};
			EnsureOpen();
            BeginExecute("Quit", false, md);
		}

        /// The mail server is disconnected.
		/// <summary>
		/// disconnect connection to pop3 server.
        /// The mail server is disconnected.
		/// </summary>
		public override void Close()
		{
            base.Close();
			_state = Pop3ConnectionState.Disconnected;
		}
		/// <summary>
		/// 
		/// </summary>
		~Pop3Client()
		{
			Dispose(false);
		}
	}
}