using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common.Net.Proxy.EventArgs;
using Common.Net.Proxy.Exceptions;

namespace Common.Net.Proxy
{
    /// <summary>
    /// Socks5 connection proxy class.  This class implements the Socks5 standard proxy protocol.
    /// </summary>
    /// <remarks>
    /// This implementation supports TCP proxy connections with a Socks v5 server.
    /// </remarks>
    public class Socks5ProxyClient : IProxyClient
    {
        private string _proxyHost;
        private int _proxyPort;
        private string _proxyUserName;
        private string _proxyPassword;
        private SocksAuthentication _proxyAuthMethod;
        private TcpClient _tcpClient;
        private TcpClient _tcpClientCached;

        private const int Socks5DefaultPort = 1080;
        private const byte Socks5VersionNumber = 5;
        private const byte Socks5Reserved = 0x00;
        private const byte Socks5AuthNumberOfAuthMethodsSupported = 2;
        private const byte Socks5AuthMethodNoAuthenticationRequired = 0x00;
        //private const byte Socks5AuthMethodGssapi = 0x01;
        private const byte Socks5AuthMethodUsernamePassword = 0x02;
        //private const byte Socks5AuthMethodIanaAssignedRangeBegin = 0x03;
        //private const byte Socks5AuthMethodIanaAssignedRangeEnd = 0x7f;
        //private const byte Socks5AuthMethodReservedRangeBegin = 0x80;
        //private const byte Socks5AuthMethodReservedRangeEnd = 0xfe;
        private const byte Socks5AuthMethodReplyNoAcceptableMethods = 0xff;
        private const byte Socks5CmdConnect = 0x01;
        //private const byte Socks5CmdBind = 0x02;
        //private const byte Socks5CmdUdpAssociate = 0x03;
        private const byte Socks5CmdReplySucceeded = 0x00;
        private const byte Socks5CmdReplyGeneralSocksServerFailure = 0x01;
        private const byte Socks5CmdReplyConnectionNotAllowedByRuleset = 0x02;
        private const byte Socks5CmdReplyNetworkUnreachable = 0x03;
        private const byte Socks5CmdReplyHostUnreachable = 0x04;
        private const byte Socks5CmdReplyConnectionRefused = 0x05;
        private const byte Socks5CmdReplyTtlExpired = 0x06;
        private const byte Socks5CmdReplyCommandNotSupported = 0x07;
        private const byte Socks5CmdReplyAddressTypeNotSupported = 0x08;
        private const byte Socks5AddrtypeIpv4 = 0x01;
        private const byte Socks5AddrtypeDomainName = 0x03;
        private const byte Socks5AddrtypeIpv6 = 0x04;

        /// <summary>
        /// Authentication itemType.
        /// </summary>
        private enum SocksAuthentication
        {
            /// <summary>
            /// No authentication used.
            /// </summary>
            None,
            /// <summary>
            /// Username and password authentication.
            /// </summary>
            UsernamePassword
        }

        /// <summary>
        /// Create a Socks5 proxy client object. 
        /// </summary>
        public Socks5ProxyClient() { }

        /// <summary>
        /// Creates a Socks5 proxy client object using the supplied TcpClient object connection.
        /// </summary>
        /// <param name="tcpClient">A TcpClient connection object.</param>
        public Socks5ProxyClient(TcpClient tcpClient)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");

            _tcpClientCached = tcpClient;
        }
        
        /// <summary>
        /// Create a Socks5 proxy client object.  The default proxy port 1080 is used.
        /// </summary>
        /// <param name="proxyHost">Host name or IP address of the proxy server.</param>
        public Socks5ProxyClient(string proxyHost)
        {
            if (String.IsNullOrEmpty(proxyHost))
                throw new ArgumentNullException("proxyHost");

            _proxyHost = proxyHost;
            _proxyPort = Socks5DefaultPort;
        }

        /// <summary>
        /// Create a Socks5 proxy client object.
        /// </summary>
        /// <param name="proxyHost">Host name or IP address of the proxy server.</param>
        /// <param name="proxyPort">Port used to connect to proxy server.</param>
        public Socks5ProxyClient(string proxyHost, int proxyPort)
        {
            if (String.IsNullOrEmpty(proxyHost))
                throw new ArgumentNullException("proxyHost");

            if (proxyPort <= 0 || proxyPort > 65535)
                throw new ArgumentOutOfRangeException("proxyPort", "port must be greater than zero and less than 65535");

            _proxyHost = proxyHost;
            _proxyPort = proxyPort;
        }

        /// <summary>
        /// Create a Socks5 proxy client object.  The default proxy port 1080 is used.
        /// </summary>
        /// <param name="proxyHost">Host name or IP address of the proxy server.</param>
        /// <param name="proxyUserName">Proxy authentication user name.</param>
        /// <param name="proxyPassword">Proxy authentication password.</param>
        public Socks5ProxyClient(string proxyHost, string proxyUserName, string proxyPassword)
        {
            if (String.IsNullOrEmpty(proxyHost))
                throw new ArgumentNullException("proxyHost");

            if (proxyUserName == null)
                throw new ArgumentNullException("proxyUserName");

            if (proxyPassword == null)
                throw new ArgumentNullException("proxyPassword");
            
            _proxyHost = proxyHost;
            _proxyPort = Socks5DefaultPort;
            _proxyUserName = proxyUserName;
            _proxyPassword = proxyPassword;
        }

        /// <summary>
        /// Create a Socks5 proxy client object.  
        /// </summary>
        /// <param name="proxyHost">Host name or IP address of the proxy server.</param>
        /// <param name="proxyPort">Port used to connect to proxy server.</param>
        /// <param name="proxyUserName">Proxy authentication user name.</param>
        /// <param name="proxyPassword">Proxy authentication password.</param>
        public Socks5ProxyClient(string proxyHost, int proxyPort, string proxyUserName, string proxyPassword)
        {
            if (String.IsNullOrEmpty(proxyHost))
                throw new ArgumentNullException("proxyHost");

            if (proxyPort <= 0 || proxyPort > 65535)
                throw new ArgumentOutOfRangeException("proxyPort", "port must be greater than zero and less than 65535");

            if (proxyUserName == null)
                throw new ArgumentNullException("proxyUserName");

            if (proxyPassword == null)
                throw new ArgumentNullException("proxyPassword");
            
            _proxyHost = proxyHost;
            _proxyPort = proxyPort;
            _proxyUserName = proxyUserName;
            _proxyPassword = proxyPassword;
        }

        /// <summary>
        /// Gets or sets host name or IP address of the proxy server.
        /// </summary>
        public string ProxyHost
        {
            get { return _proxyHost; }
            set { _proxyHost = value; }
        }

        /// <summary>
        /// Gets or sets port used to connect to proxy server.
        /// </summary>
        public int ProxyPort
        {
            get { return _proxyPort; }
            set { _proxyPort = value; }
        }

        /// <summary>
        /// Gets String representing the name of the proxy. 
        /// </summary>
        /// <remarks>This property will always return the value 'SOCKS5'</remarks>
        public string ProxyName
        {
            get { return "SOCKS5"; }
        }
        
        /// <summary>
        /// Gets or sets proxy authentication user name.
        /// </summary>
        public string ProxyUserName
        {
            get { return _proxyUserName; }
            set { _proxyUserName = value; }
        }

        /// <summary>
        /// Gets or sets proxy authentication password.
        /// </summary>
        public string ProxyPassword
        {
            get { return _proxyPassword; }
            set { _proxyPassword = value; }
        }

        /// <summary>
        /// Gets or sets the TcpClient object. 
        /// This property can be set prior to executing CreateConnection to use an existing TcpClient connection.
        /// </summary>
        public TcpClient TcpClient
        {
            get { return _tcpClientCached; }
            set { _tcpClientCached = value; }
        }

        /// <summary>
        /// Creates a remote TCP connection through a proxy server to the destination host on the destination port.
        /// </summary>
        /// <param name="destinationHost">Destination host name or IP address of the destination server.</param>
        /// <param name="destinationPort">Port number to connect to on the destination host.</param>
        /// <returns>
        /// Returns an open TcpClient object that can be used normally to communicate
        /// with the destination server
        /// </returns>
        /// <remarks>
        /// This method creates a connection to the proxy server and instructs the proxy server
        /// to make a pass through connection to the specified destination host on the specified
        /// port.  
        /// </remarks>
        public TcpClient CreateConnection(string destinationHost, int destinationPort)
        {
            if (String.IsNullOrEmpty(destinationHost))
                throw new ArgumentNullException("destinationHost");

            if (destinationPort <= 0 || destinationPort > 65535)
                throw new ArgumentOutOfRangeException("destinationPort", "port must be greater than zero and less than 65535");

            try
            {
                // if we have no cached tcpip connection then create one
                if (_tcpClientCached == null)
                {
                    if (String.IsNullOrEmpty(_proxyHost))
                        throw new ProxyException("ProxyHost property must contain a value.");

                    if (_proxyPort <= 0 || _proxyPort > 65535)
                        throw new ProxyException("ProxyPort value must be greater than zero and less than 65535");

                    //  create new tcp client object to the proxy server
                    _tcpClient = new TcpClient();

                    // attempt to open the connection
                    _tcpClient.Connect(_proxyHost, _proxyPort);
                }
                else
                {
                    _tcpClient = _tcpClientCached;
                }

                //  determine which authentication method the client would like to use
                DetermineClientAuthMethod();

                // negotiate which authentication methods are supported / accepted by the server
                NegotiateServerAuthMethod();

                // send a connect command to the proxy server for destination host and port
                SendCommand(Socks5CmdConnect, destinationHost, destinationPort);

                // remove the private reference to the tcp client so the proxy object does not keep it
                // return the open proxied tcp client object to the caller for normal use
                var rtn = _tcpClient;
                _tcpClient = null;
                return rtn;
            }
            catch (Exception ex)
            {
                throw new ProxyException(String.Format(CultureInfo.InvariantCulture, "Connection to proxy host {0} on port {1} failed.", Utils.GetHost(_tcpClient), Utils.GetPort(_tcpClient)), ex);
            }
        }


        private void DetermineClientAuthMethod()
        {
            //  set the authentication itemType used based on values inputed by the user
            if (_proxyUserName != null && _proxyPassword != null)
                _proxyAuthMethod = SocksAuthentication.UsernamePassword;
            else
                _proxyAuthMethod = SocksAuthentication.None;
        }
       
        private void NegotiateServerAuthMethod()
        {
            //  get a reference to the network stream
            var stream = _tcpClient.GetStream();

            // SERVER AUTHENTICATION REQUEST
            // The client connects to the server, and sends a version
            // identifier/method selection message:
            //
            //      +----+----------+----------+
            //      |VER | NMETHODS | METHODS  |
            //      +----+----------+----------+
            //      | 1  |    1     | 1 to 255 |
            //      +----+----------+----------+
            
            var authRequest = new byte[4];
            authRequest[0] = Socks5VersionNumber;
            authRequest[1] = Socks5AuthNumberOfAuthMethodsSupported;
            authRequest[2] = Socks5AuthMethodNoAuthenticationRequired; 
            authRequest[3] = Socks5AuthMethodUsernamePassword; 

            //  send the request to the server specifying authentication types supported by the client.
            stream.Write(authRequest, 0, authRequest.Length);
            
            //  SERVER AUTHENTICATION RESPONSE
            //  The server selects from one of the methods given in METHODS, and
            //  sends a METHOD selection message:
            //
            //     +----+--------+
            //     |VER | METHOD |
            //     +----+--------+
            //     | 1  |   1    |
            //     +----+--------+
            //
            //  If the selected METHOD is X'FF', none of the methods listed by the
            //  client are acceptable, and the client MUST close the connection.
            //
            //  The values currently defined for METHOD are:
            //   * X'00' NO AUTHENTICATION REQUIRED
            //   * X'01' GSSAPI
            //   * X'02' USERNAME/PASSWORD
            //   * X'03' to X'7F' IANA ASSIGNED
            //   * X'80' to X'FE' RESERVED FOR PRIVATE METHODS
            //   * X'FF' NO ACCEPTABLE METHODS

            //  receive the server response 
            var response = new byte[2];
            stream.Read(response, 0, response.Length);

            //  the first byte contains the socks version number (e.g. 5)
            //  the second byte contains the auth method acceptable to the proxy server
            var acceptedAuthMethod = response[1];
            
            // if the server does not accept any of our supported authenication methods then throw an error
            if (acceptedAuthMethod == Socks5AuthMethodReplyNoAcceptableMethods)
            {
                _tcpClient.Close();
                throw new ProxyException("The proxy destination does not accept the supported proxy client authentication methods.");
            }

            // if the server accepts a username and password authentication and none is provided by the user then throw an error
            if (acceptedAuthMethod == Socks5AuthMethodUsernamePassword && _proxyAuthMethod == SocksAuthentication.None)
            {
                _tcpClient.Close();
                throw new ProxyException("The proxy destination requires a username and password for authentication.");
            }

            if (acceptedAuthMethod != Socks5AuthMethodUsernamePassword) return;
            
            // USERNAME / PASSWORD SERVER REQUEST
            // Once the SOCKS V5 server has started, and the client has selected the
            // Username/Password Authentication protocol, the Username/Password
            // subnegotiation begins.  This begins with the client producing a
            // Username/Password request:
            //
            //       +----+------+----------+------+----------+
            //       |VER | ULEN |  UNAME   | PLEN |  PASSWD  |
            //       +----+------+----------+------+----------+
            //       | 1  |  1   | 1 to 255 |  1   | 1 to 255 |
            //       +----+------+----------+------+----------+

            // create a data structure (binary array) containing credentials
            // to send to the proxy server which consists of clear username and password data
            var credentials = new byte[_proxyUserName.Length + _proxyPassword.Length + 3];
                
            // for SOCKS5 username/password authentication the VER field must be set to 0x01
            //  http://en.wikipedia.org/wiki/SOCKS
            //      field 1: version number, 1 byte (must be 0x01)"
            credentials[0] = 0x01;  
            credentials[1] = (byte)_proxyUserName.Length; 
            Array.Copy(Encoding.ASCII.GetBytes(_proxyUserName), 0, credentials, 2, _proxyUserName.Length); 
            credentials[_proxyUserName.Length + 2] = (byte)_proxyPassword.Length;
            Array.Copy(Encoding.ASCII.GetBytes(_proxyPassword), 0, credentials, _proxyUserName.Length + 3, _proxyPassword.Length); 

            // USERNAME / PASSWORD SERVER RESPONSE
            // The server verifies the supplied UNAME and PASSWD, and sends the
            // following response:
            //
            //   +----+--------+
            //   |VER | STATUS |
            //   +----+--------+
            //   | 1  |   1    |
            //   +----+--------+
            //
            // A STATUS field of X'00' indicates success. If the server returns a
            // `failure' (STATUS value other than X'00') status, it MUST close the
            // connection.

            // transmit credentials to the proxy server
            stream.Write(credentials, 0, credentials.Length);

            // read the response from the proxy server
            var crResponse = new byte[2];
            stream.Read(crResponse, 0, crResponse.Length);

            // check to see if the proxy server accepted the credentials
            if (crResponse[1] == 0) return;
            
            _tcpClient.Close();
            throw new ProxyException("Proxy authentification failure!  The proxy server has reported that the userid and/or password is not valid.");
        }

        private byte GetDestAddressType(string host)
        {
            IPAddress ipAddr;

            var result = IPAddress.TryParse(host, out ipAddr);

            if (!result) 
                return Socks5AddrtypeDomainName;

            switch (ipAddr.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    return Socks5AddrtypeIpv4;
                case AddressFamily.InterNetworkV6:
                    return Socks5AddrtypeIpv6;
                default:
                    throw new ProxyException(String.Format(CultureInfo.InvariantCulture, "The host addess {0} of type '{1}' is not a supported address type.  The supported types are InterNetwork and InterNetworkV6.", host, Enum.GetName(typeof(AddressFamily), ipAddr.AddressFamily)));
            }
            
        }

        private static byte[] GetDestAddressBytes(byte addressType, string host)
        {
            switch (addressType)
            {
                case Socks5AddrtypeIpv4:
                case Socks5AddrtypeIpv6:
                    return IPAddress.Parse(host).GetAddressBytes();
                case Socks5AddrtypeDomainName:
                    //  create a byte array to hold the host name bytes plus one byte to store the length
                    var bytes = new byte[host.Length + 1]; 
                    //  if the address field contains a fully-qualified domain name.  The first
                    //  octet of the address field contains the number of octets of name that
                    //  follow, there is no terminating NUL octet.
                    bytes[0] = Convert.ToByte(host.Length);
                    Encoding.ASCII.GetBytes(host).CopyTo(bytes, 1);
                    return bytes;
                default:
                    return null;
            }
        }

        private static byte[] GetDestPortBytes(int value)
        {
            var array = new byte[2];
            array[0] = Convert.ToByte(value / 256);
            array[1] = Convert.ToByte(value % 256);
            return array;
        }

        private void SendCommand(byte command, string destinationHost, int destinationPort)
        {
            var stream = _tcpClient.GetStream();

            var addressType = GetDestAddressType(destinationHost);
            var destAddr = GetDestAddressBytes(addressType, destinationHost);
            var destPort = GetDestPortBytes(destinationPort);

            //  The connection request is made up of 6 bytes plus the
            //  length of the variable address byte array
            //
            //  +----+-----+-------+------+----------+----------+
            //  |VER | CMD |  RSV  | ATYP | DST.ADDR | DST.PORT |
            //  +----+-----+-------+------+----------+----------+
            //  | 1  |  1  | X'00' |  1   | Variable |    2     |
            //  +----+-----+-------+------+----------+----------+
            //
            // * VER protocol version: X'05'
            // * CMD
            //   * CONNECT X'01'
            //   * BIND X'02'
            //   * UDP ASSOCIATE X'03'
            // * RSV RESERVED
            // * ATYP address itemType of following address
            //   * IP V4 address: X'01'
            //   * DOMAINNAME: X'03'
            //   * IP V6 address: X'04'
            // * DST.ADDR desired destination address
            // * DST.PORT desired destination port in network octet order            

            var request = new byte[4 + destAddr.Length + 2];
            request[0] = Socks5VersionNumber;
            request[1] = command;
            request[2] = Socks5Reserved;
            request[3] = addressType;
            destAddr.CopyTo(request, 4);
            destPort.CopyTo(request, 4 + destAddr.Length);
            
            // send connect request.
            stream.Write(request, 0, request.Length);
        
            //  PROXY SERVER RESPONSE
            //  +----+-----+-------+------+----------+----------+
            //  |VER | REP |  RSV  | ATYP | BND.ADDR | BND.PORT |
            //  +----+-----+-------+------+----------+----------+
            //  | 1  |  1  | X'00' |  1   | Variable |    2     |
            //  +----+-----+-------+------+----------+----------+
            //
            // * VER protocol version: X'05'
            // * REP Reply field:
            //   * X'00' succeeded
            //   * X'01' general SOCKS server failure
            //   * X'02' connection not allowed by ruleset
            //   * X'03' Network unreachable
            //   * X'04' Host unreachable
            //   * X'05' Connection refused
            //   * X'06' TTL expired
            //   * X'07' Command not supported
            //   * X'08' Address itemType not supported
            //   * X'09' to X'FF' unassigned
            //* RSV RESERVED
            //* ATYP address itemType of following address

            var response = new byte[255];
            
            // read proxy server response
            stream.Read(response, 0, response.Length);
            
            var replyCode = response[1];

            //  evaluate the reply code for an error condition
            if (replyCode != Socks5CmdReplySucceeded)
                HandleProxyCommandError(response, destinationHost, destinationPort );
        }

        private static void HandleProxyCommandError(byte[] response, string destinationHost, int destinationPort)
        {
            string proxyErrorText;
            var replyCode = response[1];
            var addrType = response[3];
            var addr = "";
            Int16 port = 0;

            switch (addrType)
            {
                case Socks5AddrtypeDomainName:
                    var addrLen = Convert.ToInt32(response[4]);
                    var addrBytes = new byte[addrLen];
                    for (var i = 0; i < addrLen; i++) addrBytes[i] = response[i + 5];
                    addr = Encoding.ASCII.GetString(addrBytes);
                    var portBytesDomain = new byte[2];
                    portBytesDomain[0] = response[6 + addrLen];
                    portBytesDomain[1] = response[5 + addrLen];
                    port = BitConverter.ToInt16(portBytesDomain, 0); 
                    break;

                case Socks5AddrtypeIpv4:
                    var ipv4Bytes = new byte[4];
                    for (var i = 0; i < 4; i++) ipv4Bytes[i] = response[i + 4];
                    var ipv4 = new IPAddress(ipv4Bytes);
                    addr = ipv4.ToString();
                    var portBytesIpv4 = new byte[2];
                    portBytesIpv4[0] = response[9];
                    portBytesIpv4[1] = response[8];
                    port = BitConverter.ToInt16(portBytesIpv4, 0); 
                    break;

                case Socks5AddrtypeIpv6:
                    var ipv6Bytes = new byte[16];
                    for (var i = 0; i < 16; i++) ipv6Bytes[i] = response[i + 4];
                    var ipv6 = new IPAddress(ipv6Bytes);
                    addr = ipv6.ToString();
                    var portBytesIpv6 = new byte[2];
                    portBytesIpv6[0] = response[21];
                    portBytesIpv6[1] = response[20];
                    port = BitConverter.ToInt16(portBytesIpv6, 0); 
                    break;
            }

            switch (replyCode)
            {
                case Socks5CmdReplyGeneralSocksServerFailure:
                    proxyErrorText = "a general socks destination failure occurred";
                    break;
                case Socks5CmdReplyConnectionNotAllowedByRuleset:
                    proxyErrorText = "the connection is not allowed by proxy destination rule set";
                    break;
                case Socks5CmdReplyNetworkUnreachable:
                    proxyErrorText = "the network was unreachable";
                    break;
                case Socks5CmdReplyHostUnreachable:
                    proxyErrorText = "the host was unreachable";
                    break;
                case Socks5CmdReplyConnectionRefused:
                    proxyErrorText = "the connection was refused by the remote network";
                    break;
                case Socks5CmdReplyTtlExpired:
                    proxyErrorText = "the time to live (TTL) has expired";
                    break;
                case Socks5CmdReplyCommandNotSupported:
                    proxyErrorText = "the command issued by the proxy client is not supported by the proxy destination";
                    break;
                case Socks5CmdReplyAddressTypeNotSupported:
                    proxyErrorText = "the address type specified is not supported";
                    break;
                default:
                    proxyErrorText = String.Format(CultureInfo.InvariantCulture, "that an unknown reply with the code value '{0}' was received by the destination", replyCode.ToString(CultureInfo.InvariantCulture));
                    break;
            }

            var exceptionMsg = String.Format(CultureInfo.InvariantCulture, "The {0} concerning destination host {1} port number {2}.  The destination reported the host as {3} port {4}.", proxyErrorText, destinationHost, destinationPort, addr, port.ToString(CultureInfo.InvariantCulture));

            throw new ProxyException(exceptionMsg);
        }


        #region "Async Methods"

        private BackgroundWorker _asyncWorker;
        private Exception _asyncException;

        /// <summary>
        /// Gets a value indicating whether an asynchronous operation is running.
        /// </summary>
        /// <remarks>Returns true if an asynchronous operation is running; otherwise, false.
        /// </remarks>
        public bool IsBusy
        {
            get { return _asyncWorker != null && _asyncWorker.IsBusy; }
        }

        /// <summary>
        /// Gets a value indicating whether an asynchronous operation is cancelled.
        /// </summary>
        /// <remarks>Returns true if an asynchronous operation is cancelled; otherwise, false.
        /// </remarks>
        public bool IsAsyncCancelled { get; private set; }

        /// <summary>
        /// Cancels any asychronous operation that is currently active.
        /// </summary>
        public void CancelAsync()
        {
            if (_asyncWorker == null || _asyncWorker.CancellationPending || !_asyncWorker.IsBusy) return;
            
            IsAsyncCancelled = true;
            _asyncWorker.CancelAsync();
        }

        private void CreateAsyncWorker()
        {
            if (_asyncWorker != null)
                _asyncWorker.Dispose();
            _asyncException = null;
            _asyncWorker = null;
            IsAsyncCancelled = false;
            _asyncWorker = new BackgroundWorker();
        }

        /// <summary>
        /// Event handler for CreateConnectionAsync method completed.
        /// </summary>
        public event EventHandler<CreateConnectionAsyncCompletedEventArgs> CreateConnectionAsyncCompleted;


        /// <summary>
        /// Asynchronously creates a remote TCP connection through a proxy server to the destination host on the destination port.
        /// </summary>
        /// <param name="destinationHost">Destination host name or IP address.</param>
        /// <param name="destinationPort">Port number to connect to on the destination host.</param>
        /// <returns>
        /// Returns TcpClient object that can be used normally to communicate
        /// with the destination server.
        /// </returns>
        /// <remarks>
        /// This method instructs the proxy server
        /// to make a pass through connection to the specified destination host on the specified
        /// port.  
        /// </remarks>
        public void CreateConnectionAsync(string destinationHost, int destinationPort)
        {
            if (_asyncWorker != null && _asyncWorker.IsBusy)
                throw new InvalidOperationException("The Socks4 object is already busy executing another asynchronous operation.  You can only execute one asychronous method at a time.");

            CreateAsyncWorker();
            
            Debug.Assert(_asyncWorker != null, "_asyncWorker != null");

            _asyncWorker.WorkerSupportsCancellation = true;
            _asyncWorker.DoWork += CreateConnectionAsync_DoWork;
            _asyncWorker.RunWorkerCompleted += CreateConnectionAsync_RunWorkerCompleted;
            var args = new Object[2];
            args[0] = destinationHost;
            args[1] = destinationPort;
            _asyncWorker.RunWorkerAsync(args);
        }

        private void CreateConnectionAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var args = (Object[])e.Argument;
                e.Result = CreateConnection((string)args[0], (int)args[1]);
            }
            catch (Exception ex)
            {
                _asyncException = ex;
            }
        }

        private void CreateConnectionAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (CreateConnectionAsyncCompleted != null)
                CreateConnectionAsyncCompleted(this, new CreateConnectionAsyncCompletedEventArgs(_asyncException, IsAsyncCancelled, (TcpClient)e.Result));
        }

        #endregion
    }
}
