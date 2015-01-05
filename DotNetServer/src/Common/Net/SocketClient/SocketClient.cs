using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Common.Base;
using Common.Net.Core;
using Common.Net.Proxy;

namespace Common.Net.SocketClient
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketClient
    {
        private Socket _socket;
        private Int32 _receiveTimeout = 60 * 1000;
        private Int32 _sendBufferSize = 8192;
        private Int32 _receiveBufferSize = 8192;
        private RemoteCertificateValidationCallback _remoteCertificateValidationCallback = DefaultRemoteCertificateValidationCallback;

        /// <summary>
        /// 
        /// </summary>
        protected Socket Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Stream Stream { get; set; }

        /// receiving and processing of the timeout in seconds, in milliseconds, to get or set.
        /// <summary>
        /// Get or set timeout milliseconds.
        /// receiving and processing of the timeout in seconds, in milliseconds, to get or set.
        /// </summary>
        public Int32 ReceiveTimeout
        {
            get { return _receiveTimeout; }
            set
            {
                _receiveTimeout = value;
                if (_socket != null)
                {
                    _socket.ReceiveTimeout = _receiveTimeout;
                }
            }
        }

        /// The data sent to get or set the buffer size.
        /// <summary>
        /// Get or set buffer size to send.
        /// The data sent to get or set the buffer size.
        /// </summary>
        public Int32 SendBufferSize
        {
            get { return _sendBufferSize; }
            set
            {
                _sendBufferSize = value;
                if (_socket != null)
                {
                    _socket.SendBufferSize = _sendBufferSize;
                }
            }
        }

        /// The data received to get or set the buffer size.
        /// <summary>
        /// Get or set buffer size to receive.
        /// The data received to get or set the buffer size.
        /// </summary>
        public Int32 ReceiveBufferSize
        {
            get { return _receiveBufferSize; }
            set
            {
                _receiveBufferSize = value;
                if (_socket != null)
                {
                    _socket.ReceiveBufferSize = _receiveBufferSize;
                }
            }
        }


        /// SSL certificate verification methods for you to get or set.
        /// <summary>
        /// SSL certificate verification methods for you to get or set.
        /// </summary>
        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback
        {
            get { return _remoteCertificateValidationCallback; }
            set { _remoteCertificateValidationCallback = value; }
        }

        /// The server you want to connect and communicate to the Socket to obtain the object.
        /// <summary>
        /// Get Socket object to communicate to server.
        /// The server you want to connect and communicate to the Socket to obtain the object.
        /// </summary>
        /// <returns></returns>
        protected void SetSocket()
        {
            Socket tc = null;

            var proxyClient = new ProxyClientFactory().CreateProxyClient();

            if (proxyClient != null)
            {
                try
                {
                    var client = proxyClient.CreateConnection(ServerName, Port);
                    if (client.Connected)
                    {
                        Socket = client.Client;
                    }
                    else
                    {
                        throw new Exception("Client does not connect via proxy");
                    }
                }
                catch (Exception exception)
                {
                    Logger.Log(LogType.Error, this, "SetSocket", exception);
                }
                return;
            }

            //Name of the server to get a list of addresses from the IP.
            var hostEntry = Helper.GetHostEntry(ServerName);
            //A valid IP address to determine whether or not a valid IP address that was set to to get the socket.
            if (hostEntry != null)
            {
                foreach (var address in hostEntry.AddressList)
                {
                    tc = TryGetSocket(address);
                    if (tc != null) { break; }
                }
            }
            Socket = tc;
        }

        private Socket TryGetSocket(IPAddress address)
        {
            var ipe = new IPEndPoint(address, _port);
            Socket tc;

            try
            {
                tc = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tc.Connect(ipe);
                if (tc.Connected)
                {
                    tc.ReceiveTimeout = ReceiveTimeout;
                    tc.SendBufferSize = SendBufferSize;
                    tc.ReceiveBufferSize = ReceiveBufferSize;
                }
            }
            catch
            {
                tc = null;
            }
            return tc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void SetProperty(SocketClient client)
        {
            var cl = client;
            ServerName = cl.ServerName;
            Port = cl.Port;
            UserName = cl.UserName;
            Password = cl.Password;
            ReceiveBufferSize = cl.ReceiveBufferSize;
            ReceiveTimeout = cl.ReceiveTimeout;
            RemoteCertificateValidationCallback = cl.RemoteCertificateValidationCallback;
            ResponseEncoding = cl.ResponseEncoding;
            SendBufferSize = cl.SendBufferSize;
            Ssl = cl.Ssl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean Connect()
        {
            if (Socket == null)
            {
                SetSocket();
            }
            if (Socket == null)
            {
                Stream = null;
            }
            else
            {
                if (Ssl)
                {
                    var ssl = new SslStream(new NetworkStream(Socket), true, RemoteCertificateValidationCallback);
                    ssl.AuthenticateAsClient(ServerName);
                    if (ssl.IsAuthenticated == false)
                    {
                        Socket = null;
                        Stream = null;
                        return false;
                    }
                    Stream = ssl;
                }
                else
                {
                    Stream = new NetworkStream(Socket);
                }
            }

            return Stream != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void Send(String command)
        {
            Send(Encoding.ASCII.GetBytes(command + NewLine));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(Byte[] bytes)
        {
            Send(new MemoryStream(bytes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void Send(Stream stream)
        {
            DataSendContext cx = null;

            if (Socket == null)
            {
                throw new SocketClientException("Connection is closed");
            }
            try
            {
                cx = new DataSendContext(stream, RequestEncoding);
                cx.FillBuffer();
                Stream.BeginWrite(cx.GetByteArray(), 0, cx.SendBufferSize, SendCallback, cx);
                SendDone.WaitOne();

                if (TraceSource.Listeners.Count > 0)
                {
                    TraceSource.TraceInformation("Client Send:" + cx.GetText());
                }
            }
            catch (Exception ex)
            {
                throw new SocketClientException(ex);
            }
            finally
            {
                if (cx != null)
                {
                    cx.Dispose();
                }
            }
            //Throw exception that occor other thread.
            if (cx.Exception != null)
            {
                throw cx.Exception;
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            DataSendContext cx = null;
            try
            {
                cx = (DataSendContext)result.AsyncState;
                Stream.EndWrite(result);
                if (cx.DataRemained)
                {
                    cx.FillBuffer();
                    Stream.BeginWrite(cx.GetByteArray(), 0, cx.SendBufferSize, SendCallback, cx);
                }
                else
                {
                    SendDone.Set();
                }
            }
            catch (Exception ex)
            {
                if (cx != null) cx.Exception = ex;
            }

            if (cx == null || cx.Exception == null) return;
            
            try
            {
                //In the Dispose issues with the timing of that is that if you
                SendDone.Set();
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetResponseText()
        {
            var bb = GetResponseBytes();
            return ResponseEncoding.GetString(bb);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Byte[] GetResponseBytes()
        {
            var ms = new MemoryStream();
            GetResponseStream(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void GetResponseStream(Stream stream)
        {
            GetResponseStream(new DataReceiveContext(stream, ResponseEncoding));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected void GetResponseStream(DataReceiveContext context)
        {
            if (Socket == null)
            {
                throw new SocketClientException("Connection is closed");
            }
            using (var cx = context)
            {
                var bb = cx.GetByteArray();
                Stream.BeginRead(bb, 0, bb.Length, GetResponseCallback, cx);
                var bl = GetResponseDone.WaitOne(ReceiveTimeout);
                if (cx.Exception != null)
                {
                    throw cx.Exception;
                }
                if (cx.Timeout || bl == false)
                {
                    throw new SocketClientException("Response timeout");
                }

                if (TraceSource.Listeners.Count <= 0) return;
                var text = ResponseEncoding.GetString(cx.GetData());
                TraceSource.TraceInformation("Client Receive:" + text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected Byte[] GetResponseBytes(DataReceiveContext context)
        {
            GetResponseStream(context);
            return context.GetData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        protected void GetResponseCallback(IAsyncResult result)
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
                var ts = DateTime.Now - cx.StartTime;

                if (ts.TotalMilliseconds > ReceiveTimeout)
                {
                    cx.Timeout = true;
                    GetResponseDone.Set();
                }
                if (cx.ReadBuffer(size))
                {
                    var bb = cx.GetByteArray();
                    Stream.BeginRead(bb, 0, bb.Length, GetResponseCallback, cx);
                }
                else
                {
                    GetResponseDone.Set();
                }
            }
            catch (Exception ex)
            {
                if (cx != null) cx.Exception = ex;
            }
            if (cx != null && cx.Exception != null)
            {
                try
                {
                    GetResponseDone.Set();
                }
                catch (ObjectDisposedException) { }
            }
        }

        /// Asynchronous POP3 e-mail to send a command to the server. Received response string is callbackFunction can be obtained as the argument.
        /// <summary>
        /// Send a command with asynchronous and get response text by first parameter of callbackFunction.
        /// Asynchronous POP3 e-mail to send a command to the server. Received response string is callbackFunction can be obtained as the argument.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="callbackFunction"></param>
        public void BeginSend(String command, DataReceiveContext context, Action<String> callbackFunction)
        {
            var isException = false;
            var cx = context;

            try
            {
                Send(command);
                var bb = cx.GetByteArray();
                Stream.BeginRead(bb, 0, bb.Length, BeginSendCallBack, cx);
            }
            catch
            {
                isException = true;
                throw;
            }
            finally
            {
                if (isException && cx != null)
                {
                    cx.Dispose();
                }
            }
        }

        /// Asynchronous POP3 e-mail, receiving data from the server.
        /// If the received data is still re-BeginExecute method to retrieve the rest of the data.
        /// <summary>
        /// Send a command with asynchronous and get response text by first parameter of callbackFunction.
        /// If there is more data to receive,continously call BeginExecuteCallback method and get response data.
        /// Asynchronous POP3 e-mail, receiving data from the server.
        /// If the received data is still re-BeginExecute method to retrieve the rest of the data.
        /// </summary>
        /// <param name="result"></param>
        private void BeginSendCallBack(IAsyncResult result)
        {
            var cx = (DataReceiveContext)result.AsyncState;
            var isException = false;

            try
            {
                var size = Stream.EndRead(result);
                if (cx.ReadBuffer(size))
                {
                    //If the data is still being received again the response receives the data.
                    var bb = cx.GetByteArray();
                    Stream.BeginRead(bb, 0, bb.Length, BeginSendCallBack, cx);
                }
                else
                {
                    cx.OnEndGetResponse();
                    cx.Dispose();
                }
            }
            catch (Exception ex)
            {
                isException = true;
                OnError(ex);
            }
            finally
            {
                if (isException && cx != null)
                {
                    cx.Dispose();
                }
            }
        }


        private static Boolean DefaultRemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DisposeSocket()
        {
            if (Socket == null) return;
            ((IDisposable)Socket).Dispose();
            Socket = null;
            Stream = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Close()
        {
            if (Socket != null)
            {
                Socket.Close();
            }
            if (Stream != null)
            {
                Stream.Close();
            }
            Socket = null;
            Stream = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AsyncSocketCallErrorEventArgs> Error;
        /// new line is the value of a string.
        /// <summary>
        /// new line is the value of a string.
        /// </summary>
        public static readonly String NewLine = "\r\n";
        /// <summary>
        /// 
        /// </summary>
        public static TraceSource TraceSource = new TraceSource("Sanelib.Net.SocketClient");
        private String _userName = "";
        private String _password = "";
        private String _serverName = "";
        private Int32 _port = -1;
        private Encoding _requestEncoding = Encoding.UTF8;
        private Encoding _responseEncoding = Encoding.UTF8;
        private readonly AutoResetEvent _sendDone = new AutoResetEvent(false);
        private readonly AutoResetEvent _getResponseDone = new AutoResetEvent(false);

        /// <summary>
        /// If the connection is valid, to obtain the values shown.
        /// </summary>
        public Boolean Connected
        {
            get
            {
                return Socket != null && Socket.Connected;
            }
        }

        /// that you want to use to authenticate to get or set the user name.
        /// <summary>
        /// Get or set UserName.
        /// that you want to use to authenticate to get or set the user name.
        /// </summary>
        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// Password to use for authentication in the get or set.
        /// <summary>
        /// Get or set password.
        /// Password to use for authentication in the get or set.
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// The POP3 e-mail server to get or set the name of the server.
        /// <summary>
        /// Get or set server.
        /// The POP3 e-mail server to get or set the name of the server
        /// </summary>
        public String ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }
        /// Port used to communicate to get or set the number.
        /// <summary>
        /// Get or set port.
        /// Port used to communicate to get or set the number.
        /// </summary>
        public Int32 Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// in communication, whether or not to encrypt SSL gets or sets a value that indicates.
        /// <summary>
        /// Get or set use ssl protocol.
        /// in communication, whether or not to encrypt SSL gets or sets a value that indicates.
        /// </summary>
        public bool Ssl { get; set; }

        /// The data sent to get or set the encoding.
        /// <summary>
        /// The data sent to get or set the encoding.
        /// </summary>
        public Encoding RequestEncoding
        {
            get { return _requestEncoding; }
            set { _requestEncoding = value; }
        }

        /// The data received to get or set the encoding.
        /// <summary>
        /// The data received to get or set the encoding.
        /// </summary>
        public Encoding ResponseEncoding
        {
            get { return _responseEncoding; }
            set { _responseEncoding = value; }
        }

        /// <summary>
        /// Get specify value whether communicating to server or not.
        /// Between send command and finish get all response data,this property get true.
        /// </summary>
        public bool Commnicating { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        protected AutoResetEvent SendDone
        {
            get { return _sendDone; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected AutoResetEvent GetResponseDone
        {
            get { return _getResponseDone; }
        }

        /// <summary>
        /// 
        /// </summary>
        static SocketClient()
        {
            TraceSource.Listeners.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="port"></param>
        public SocketClient(String serverName, Int32 port)
        {
            ServerName = serverName;
            Port = port;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public SocketClient(String serverName, Int32 port, String userName, String password)
        {
            ServerName = serverName;
            Port = port;
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Get string about mail account information.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder(256);

            sb.AppendFormat("ServerName:{0}", ServerName);
            sb.AppendLine();
            sb.AppendFormat("Port:{0}", Port);
            sb.AppendLine();
            sb.AppendFormat("UserName:{0}", UserName);
            sb.AppendLine();
            sb.AppendFormat("SSL:{0}", Ssl);

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        protected void OnError(Exception exception)
        {
            var eh = Error;
            if (eh != null)
            {
                eh(this, new AsyncSocketCallErrorEventArgs(exception));
            }
        }

        /// To perform termination processing, to free up system resources.
        /// <summary>
        /// dipose and release system resoures.
        /// To perform termination processing, to free up system resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (!disposing) return;
            DisposeSocket();
            ((IDisposable)SendDone).Dispose();
            ((IDisposable)GetResponseDone).Dispose();
        }
    }
}
