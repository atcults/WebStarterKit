using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using Common.Mail.Async;
using Common.Mail.Common;
using Common.Mail.Pop3;
using Common.Mail.Smtp.Command;
using Common.Mail.Smtp.SendMail;
using Common.Net.SocketClient;

namespace Common.Mail.Smtp
{
    /// Represent and probide functionality about smtp command.
    /// <summary>
    ///     Represent and probide functionality about smtp command.
    /// </summary>
    public class SmtpClient : SocketClient, IDisposable
    {
        /// <summary>
        ///     Default smtp port
        /// </summary>
        public static readonly Int32 DefaultPort = 25;

        /// <summary>
        ///     Default ssl port
        /// </summary>
        public static readonly Int32 DefaultSslPort = 443;

        private SmtpAuthenticateMode _mode = SmtpAuthenticateMode.Auto;
        private String _hostName = "localhost";
        private Boolean _tls;
        private readonly Pop3Client _pop3Client = new Pop3Client("127.0.0.1");
        private SmtpConnectionState _state = SmtpConnectionState.Disconnected;

        /// To get or set the authentication method.
        /// <summary>
        ///     To get or set the authentication method.
        /// </summary>
        public SmtpAuthenticateMode AuthenticateMode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// Source Host Name of the machine you get or set.
        /// <summary>
        ///     Source Host Name of the machine you get or set.
        /// </summary>
        public String HostName
        {
            get { return _hostName; }
            set { _hostName = value; }
        }

        /// To communicate using TLS Gets a value that indicates whether to.
        /// <summary>
        ///     To communicate using TLS Gets a value that indicates whether to.
        /// </summary>
        public Boolean Tls
        {
            get { return _tls; }
            set { _tls = value; }
        }

        /// To indicate the status of connections to get the value.
        /// <summary>
        ///     To indicate the status of connections to get the value.
        /// </summary>
        public SmtpConnectionState State
        {
            get { return _state; }
        }

        /// Connected to the server gets a value that indicates whether or not.
        /// <summary>
        ///     Connected to the server gets a value that indicates whether or not.
        /// </summary>
        public Boolean Available
        {
            get { return _state != SmtpConnectionState.Disconnected; }
        }

        /// If you want to authenticate PopBeforeSmtp used to obtain Client Pop 3.
        /// <summary>
        ///     If you want to authenticate PopBeforeSmtp used to obtain Client Pop 3.
        /// </summary>
        public Pop3Client Pop3Client
        {
            get { return _pop3Client; }
        }

        /// <summary>
        /// </summary>
        public SmtpClient()
            : base("127.0.0.1", DefaultPort)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="serverName"></param>
        public SmtpClient(String serverName)
            : base(serverName, DefaultPort)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public SmtpClient(String serverName, Int32 port, String userName, String password)
            : base(serverName, port)
        {
            UserName = userName;
            Password = password;
        }

        /// Opens a connection to a server.
        /// <summary>
        ///     Opens a connection to a server.
        /// </summary>
        public SmtpConnectionState Open()
        {
            if (Connect())
            {
                var rs = GetResponse();
                if (rs.StatusCode == SmtpCommandResultCode.ServiceReady)
                {
                    _state = SmtpConnectionState.Connected;
                }
                else
                {
                    Close();
                }
            }
            else
            {
                _state = SmtpConnectionState.Disconnected;
            }
            return _state;
        }

        /// The connection to the server if it is not already open, and opens a connection to a server.
        /// <summary>
        ///     The connection to the server if it is not already open, and opens a connection to a server.
        /// </summary>
        public SmtpConnectionState EnsureOpen()
        {
            if (Socket != null)
            {
                return _state;
            }
            return Open();
        }

        private SmtpCommandResult GetResponse()
        {
            var l = new List<SmtpCommandResultLine>();
            var bb = GetResponseBytes(new SmtpDataReceiveContext(ResponseEncoding));
            var sr = new StringReader(ResponseEncoding.GetString(bb));
            while (true)
            {
                var lineText = sr.ReadLine();
                var currentLine = new SmtpCommandResultLine(lineText);
                l.Add(currentLine);
                //Check if you have the line:
                if (currentLine.HasNextLine == false)
                {
                    break;
                }
            }
            SetSmtpCommandState();
            return new SmtpCommandResult(l.ToArray());
        }

        /// Upon receiving the response from the server based on the current state to change the state.
        /// <summary>
        ///     Upon receiving the response from the server based on the current state to change the state.
        /// </summary>
        private void SetSmtpCommandState()
        {
            Commnicating = false;
            switch (_state)
            {
                case SmtpConnectionState.MailFromCommandExecuting:
                    _state = SmtpConnectionState.MailFromCommandExecuted;
                    break;
                case SmtpConnectionState.RcptToCommandExecuted:
                    _state = SmtpConnectionState.RcptToCommandExecuted;
                    break;
                case SmtpConnectionState.DataCommandExecuting:
                    _state = SmtpConnectionState.DataCommandExecuted;
                    break;
            }
        }

        /// SMTP whether authentication is required on the server to get the value.
        /// <summary>
        ///     SMTP whether authentication is required on the server to get the value.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static Boolean NeedAuthenticate(String text)
        {
            return text.IndexOf("auth", StringComparison.OrdinalIgnoreCase) > -1;
        }

        /// StartTLS sends a command to the server, the encrypted to start the communication.
        /// <summary>
        ///     StartTLS sends a command to the server, the encrypted to start the communication.
        /// </summary>
        private Boolean StartTls()
        {
            if (EnsureOpen() == SmtpConnectionState.Connected)
            {
                var rs = Execute("STARTTLS");
                if (rs.StatusCode != SmtpCommandResultCode.ServiceReady)
                {
                    return false;
                }

                Ssl = true;
                Tls = true;
                var ssl = new SslStream(new NetworkStream(Socket), true, RemoteCertificateValidationCallback, null);
                ssl.AuthenticateAsClient(ServerName);
                Stream = ssl;
                return true;
            }
            return false;
        }

        /// SMTP Login to your e-mail server.
        /// <summary>
        ///     SMTP Login to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public Boolean Authenticate()
        {
            if (_mode == SmtpAuthenticateMode.Auto)
            {
                if (EnsureOpen() == SmtpConnectionState.Connected)
                {
                    var rs = ExecuteEhlo();
                    var s = rs.Message.ToUpper();
                    //SMTP if supported by the authentication
                    if (s.Contains("AUTH"))
                    {
                        if (s.Contains("LOGIN"))
                        {
                            return AuthenticateByLogin();
                        }
                        if (s.Contains("PLAIN"))
                        {
                            return AuthenticateByPlain();
                        }
                        if (s.Contains("CRAM-MD5"))
                        {
                            return AuthenticateByCramMd5();
                        }
                    }
                    else
                    {
                        rs = ExecuteEhlo();
                        return rs.StatusCode == SmtpCommandResultCode.ServiceReady;
                    }
                    //TLS authentication
                    if (Tls)
                    {
                        if (s.Contains("STARTTLS") == false)
                        {
                            throw new MailClientException("TLS is not allowed.");
                        }
                        StartTls();
                        rs = ExecuteEhlo();
                        return rs.StatusCode == SmtpCommandResultCode.ServiceReady;
                    }
                }
            }
            else
            {
                switch (_mode)
                {
                    case SmtpAuthenticateMode.None:
                        return true;
                    case SmtpAuthenticateMode.Plain:
                        return AuthenticateByPlain();
                    case SmtpAuthenticateMode.Login:
                        return AuthenticateByLogin();
                    case SmtpAuthenticateMode.CramMd5:
                        return AuthenticateByCramMd5();
                    case SmtpAuthenticateMode.PopBeforeSmtp:
                        {
                            var bl = _pop3Client.Authenticate();
                            _pop3Client.Close();
                            return bl;
                        }
                }
                throw new InvalidOperationException();
            }
            return false;
        }

        /// SMTP Plain authentication log in to the mail server.
        /// <summary>
        ///     SMTP Plain authentication log in to the mail server.
        /// </summary>
        /// <returns></returns>
        public Boolean AuthenticateByPlain()
        {
            if (EnsureOpen() == SmtpConnectionState.Connected)
            {
                var rs = Execute("Auth Plain");
                if (rs.StatusCode != SmtpCommandResultCode.WaitingForAuthentication)
                {
                    return false;
                }
                //User name & password to send a string.
                rs = Execute(MailParser.ToBase64String(String.Format("{0}\0{0}\0{1}", UserName, Password)));
                if (rs.StatusCode == SmtpCommandResultCode.AuthenticationSuccessful)
                {
                    _state = SmtpConnectionState.Authenticated;
                }
            }
            return _state == SmtpConnectionState.Authenticated;
        }

        /// SMTP Login Authentication log in to the mail server.
        /// <summary>
        ///     SMTP Login Authentication log in to the mail server.
        /// </summary>
        /// <returns></returns>
        public Boolean AuthenticateByLogin()
        {
            if (EnsureOpen() == SmtpConnectionState.Connected)
            {
                var rs = Execute("Auth Login");
                if (rs.StatusCode != SmtpCommandResultCode.WaitingForAuthentication)
                {
                    return false;
                }
                //Send User Name

                rs = Execute(MailParser.ToBase64String(UserName));
                if (rs.StatusCode != SmtpCommandResultCode.WaitingForAuthentication)
                {
                    return false;
                }
                //Send Password
                rs = Execute(MailParser.ToBase64String(Password));
                if (rs.StatusCode == SmtpCommandResultCode.AuthenticationSuccessful)
                {
                    _state = SmtpConnectionState.Authenticated;
                }
            }
            return _state == SmtpConnectionState.Authenticated;
        }

        /// to your e-mail server SMTP CRAM-MD5 authentication to log in.
        /// <summary>
        ///     to your e-mail server SMTP CRAM-MD5 authentication to log in.
        /// </summary>
        /// <returns></returns>
        public Boolean AuthenticateByCramMd5()
        {
            if (EnsureOpen() == SmtpConnectionState.Connected)
            {
                var rs = Execute("Auth CRAM-MD5");
                if (rs.StatusCode != SmtpCommandResultCode.WaitingForAuthentication)
                {
                    return false;
                }
                //User name + challenge string Base64-encoded string that you send.
                var s = MailParser.ToCramMd5String(rs.Message, UserName, Password);
                rs = Execute(s);
                if (rs.StatusCode == SmtpCommandResultCode.AuthenticationSuccessful)
                {
                    _state = SmtpConnectionState.Authenticated;
                }
            }
            return _state == SmtpConnectionState.Authenticated;
        }

        /// In synchronized SMTP to your e-mail server to send the command, depending on the type of the command and response data received and returned.
        /// <summary>
        ///     In synchronized SMTP to your e-mail server to send the command, depending on the type of the command and response data received and returned.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public SmtpCommandResult Execute(SmtpCommand command)
        {
            return Execute(command.GetCommandString());
        }

        /// In synchronized SMTP to your e-mail server to send the command, depending on the type of the command and response data received and returned.
        /// <summary>
        ///     In synchronized SMTP to your e-mail server to send the command, depending on the type of the command and response data received and returned.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private SmtpCommandResult Execute(String command)
        {
            Send(command);
            Commnicating = true;
            return GetResponse();
        }

        /// SMTP EHLO commands to be sent to your e-mail server. To send a command, the command type
        /// <summary>
        ///     SMTP EHLO commands to be sent to your e-mail server. To send a command, the command type
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteEhlo()
        {
            EnsureOpen();
            return Execute(new EhloCommand(_hostName));
        }

        /// SMTP HELO commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP HELO commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteHelo()
        {
            EnsureOpen();
            return Execute(new HeloCommand(_hostName));
        }

        private SmtpCommandResult ExecuteEhloAndHelo()
        {
            //Transaction Server Send E-Mail to send the command to start.
            var rs = ExecuteEhlo();
            if (rs.StatusCode != SmtpCommandResultCode.RequestedMailActionOkayCompleted)
            {
                rs = ExecuteHelo();
            }
            return rs;
        }

        /// SMTP MAIL commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP MAIL commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteMail(String reversePath)
        {
            EnsureOpen();
            return Execute(new MailCommand(reversePath));
        }

        /// SMTP RCPT commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP RCPT commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteRcpt(String forwardPath)
        {
            EnsureOpen();
            return Execute(new RcptCommand(forwardPath));
        }

        /// SMTP DATA commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP DATA commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteData()
        {
            EnsureOpen();
            return Execute(new DataCommand());
        }

        /// SMTP RESET commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP RESET commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteRset()
        {
            EnsureOpen();
            return Execute(new RsetCommand());
        }

        /// SMTP VRFY commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP VRFY commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteVrfy(String userName)
        {
            EnsureOpen();
            return Execute(new VrfyCommand(userName));
        }

        /// SMTP EXPN commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP EXPN commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteExpn(String mailingList)
        {
            EnsureOpen();
            return Execute(new ExpnCommand(mailingList));
        }

        /// SMTP HELP commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP HELP commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteHelp()
        {
            EnsureOpen();
            return Execute(new HelpCommand());
        }

        /// SMTP NOOP commands to be sent to your e-mail server.
        /// <summary>
        ///     SMTP NOOP commands to be sent to your e-mail server.
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteNoop()
        {
            EnsureOpen();
            return Execute("Noop");
        }

        /// SMTPメールサーバーへQUITコマンドを送信します。
        /// <summary>
        ///     SMTPメールサーバーへQUITコマンドを送信します。
        /// </summary>
        /// <returns></returns>
        public SmtpCommandResult ExecuteQuit()
        {
            EnsureOpen();
            var rs = Execute("Quit");
            //From the server side to force a connection is disconnected because once you call Dispose TcpClient = null.
            if (rs.StatusCode == SmtpCommandResultCode.ServiceClosingTransmissionChannel)
            {
                DisposeSocket();
            }
            return rs;
        }

        /// Send an email with the results of the submission will be to get the SendMailResult.
        /// <summary>
        ///     Send an email with the results of the submission will be to get the SendMailResult.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public SendMailResult SendMail(String from, String to, String cc, String bcc, String text)
        {
            var ss = to.Split(',');
            var l =
                (from t in ss where String.IsNullOrEmpty(t) != true select MailAddress.Create(t)).ToList();
            ss = cc.Split(',');
            l.AddRange(from t in ss where String.IsNullOrEmpty(t) != true select MailAddress.Create(t));
            ss = bcc.Split(',');
            l.AddRange(from t in ss where String.IsNullOrEmpty(t) != true select MailAddress.Create(t));
            return SendMail(new SendMailCommand(from, text, l));
        }

        /// Send an email with the results of the submission will be to get the SendMailResult.
        /// <summary>
        ///     Send an email with the results of the submission will be to get the SendMailResult.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public SendMailResult SendMail(String from, SmtpMessage message)
        {
            return SendMail(new SendMailCommand(from, message));
        }

        /// Send an email with the results of the submission will be to get the SendMailListResult.
        /// <summary>
        ///     Send an email with the results of the submission will be to get the SendMailListResult.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public SendMailResult SendMail(SmtpMessage message)
        {
            return SendMail(new SendMailCommand(message));
        }

        /// Send an email with the results of the submission will be to get the SendMailListResult.
        /// <summary>
        ///     Send an email with the results of the submission will be to get the SendMailListResult.
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public SendMailListResult SendMailList(IEnumerable<SmtpMessage> messages)
        {
            return SendMailList(messages.Select(mg => new SendMailCommand(mg)).ToArray());
        }

        /// Send an email with the results of the submission will be to get the SendMailListResult.
        /// <summary>
        ///     Send an email with the results of the submission will be to get the SendMailListResult.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public SendMailResult SendMail(SendMailCommand command)
        {
            var l = SendMailList(new[] {command});
            return l.Results.Count == 1
                       ? new SendMailResult(l.Results[0].State, command)
                       : new SendMailResult(l.State, command);
        }

        /// Send an email with the results of the submission will be to get the SendMailListResult.
        /// <summary>
        ///     Send an email with the results of the submission will be to get the SendMailListResult.
        /// </summary>
        /// <param name="commandList"></param>
        /// <returns></returns>
        public SendMailListResult SendMailList(IEnumerable<SendMailCommand> commandList)
        {
            SmtpCommandResult rs;
            var hasRcpt = false;

            //Connection failed
            if (EnsureOpen() == SmtpConnectionState.Disconnected)
            {
                return new SendMailListResult(SendMailResultState.Connection);
            }

            //illegal state in the execution of a method
            if (State != SmtpConnectionState.Connected &&
                State != SmtpConnectionState.Authenticated)
            {
                return new SendMailListResult(SendMailResultState.InvalidState);
            }
            //If you do not have authenticated.
            if (State != SmtpConnectionState.Authenticated)
            {
                //Transaction Server Send E-Mail to send the command to start.
                rs = ExecuteEhloAndHelo();
                if (rs.StatusCode != SmtpCommandResultCode.RequestedMailActionOkayCompleted)
                {
                    return new SendMailListResult(SendMailResultState.Helo);
                }
                //TLS/SSL communication

                if (_tls)
                {
                    if (StartTls() == false)
                    {
                        return new SendMailListResult(SendMailResultState.Tls);
                    }
                    rs = ExecuteEhloAndHelo();
                    if (rs.StatusCode != SmtpCommandResultCode.RequestedMailActionOkayCompleted)
                    {
                        return new SendMailListResult(SendMailResultState.Helo);
                    }
                }
                //Login authentication is required and that the check?
                if (NeedAuthenticate(rs.Message))
                {
                    if (Authenticate() == false)
                    {
                        return new SendMailListResult(SendMailResultState.Authenticate);
                    }
                }
            }

            var results = new List<SendMailResult>();

            foreach (var command in commandList)
            {
                //Send Mail From
                if (command.From == null ||
                    command.From.Value == null)
                {
                    rs = ExecuteMail("");
                }
                else if (command.From.Value.StartsWith("<"))
                {
                    rs = ExecuteMail(command.From.Value);
                }
                else
                {
                    rs = ExecuteMail("<" + command.From.Value + ">");
                }
                if (rs.StatusCode != SmtpCommandResultCode.RequestedMailActionOkayCompleted)
                {
                    results.Add(new SendMailResult(SendMailResultState.MailFrom, command));
                    continue;
                }
                var mailAddressList = new List<MailAddress>();
                //Send Rcpt To
                foreach (var m in command.RcptTo)
                {
                    var mailAddress = m.ToString();
                    if (mailAddress.StartsWith("<") && mailAddress.EndsWith(">"))
                    {
                        rs = ExecuteRcpt(mailAddress);
                    }
                    else
                    {
                        rs = ExecuteRcpt("<" + mailAddress + ">");
                    }
                    if (rs.StatusCode == SmtpCommandResultCode.RequestedMailActionOkayCompleted)
                    {
                        hasRcpt = true;
                    }
                    else
                    {
                        mailAddressList.Add(m);
                    }
                }
                if (hasRcpt == false)
                {
                    results.Add(new SendMailResult(SendMailResultState.Rcpt, command, mailAddressList));
                    continue;
                }
                //Send Data
                rs = ExecuteData();
                if (rs.StatusCode == SmtpCommandResultCode.StartMailInput)
                {
                    Send(command.Text + MailParser.NewLine + ".");
                    rs = GetResponse();
                    if (rs.StatusCode == SmtpCommandResultCode.RequestedMailActionOkayCompleted)
                    {
                        results.Add(new SendMailResult(SendMailResultState.Success, command, mailAddressList));
                        ExecuteRset();
                    }
                    else
                    {
                        results.Add(new SendMailResult(SendMailResultState.Data, command, mailAddressList));
                    }
                }
                else
                {
                    results.Add(new SendMailResult(SendMailResultState.Data, command, mailAddressList));
                }
            }
            ExecuteQuit();
            //Check whether all the success
            if (results.Exists(el => el.State != SendMailResultState.Success))
            {
                return new SendMailListResult(SendMailResultState.SendMailData, results);
            }
            return new SendMailListResult(SendMailResultState.Success, results);
        }

        /// <summary>
        /// </summary>
        ~SmtpClient()
        {
            Dispose(false);
        }
    }
}
