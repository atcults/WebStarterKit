using System;
using System.Net.Sockets;
using Common.Net.Proxy.Exceptions;
using Common.Service.Impl;

namespace Common.Net.Proxy
{
    /// <summary>
    /// Factory class for creating new proxy client objects.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // create an instance of the client proxy factory
    /// ProxyClientFactory factory = new ProxyClientFactory();
    ///        
	/// // use the proxy client factory to generically specify the type of proxy to create
    /// // the proxy factory method CreateProxyClient returns an IProxyClient object
    /// IProxyClient proxy = factory.CreateProxyClient(ProxyType.Http, "localhost", 6588);
    ///
	/// // create a connection through the proxy to www.starksoft.com over port 80
    /// System.Net.Sockets.TcpClient tcpClient = proxy.CreateConnection("www.starksoft.com", 80);
    /// </code>
    /// </remarks>
    public class ProxyClientFactory
    {

        /// <summary>
        /// Factory method for creating new proxy client objects.
        /// </summary>
        /// <param name="type">The type of proxy client to create.</param>
        /// <returns>Proxy client object.</returns>
        public IProxyClient CreateProxyClient(ProxyType type)
        {
            if (type == ProxyType.None)
                throw new ArgumentOutOfRangeException("type");

            switch (type)
            {
                case ProxyType.Http:
                    return new HttpProxyClient();
                case ProxyType.Socks4:
                    return new Socks4ProxyClient();
                case ProxyType.Socks4A:
                    return new Socks4AProxyClient();
                case ProxyType.Socks5:
                    return new Socks5ProxyClient();
                default:
                    throw new ProxyException(String.Format("Unknown proxy type {0}.", type.ToString()));
            }
        }        

        /// <summary>
        /// Factory method for creating new proxy client objects using an existing TcpClient connection object.
        /// </summary>
        /// <param name="type">The type of proxy client to create.</param>
        /// <param name="tcpClient">Open TcpClient object.</param>
        /// <returns>Proxy client object.</returns>
        public IProxyClient CreateProxyClient(ProxyType type, TcpClient tcpClient)
        {
            if (type == ProxyType.None)
                throw new ArgumentOutOfRangeException("type");
            
            switch (type)
            {
                case ProxyType.Http:
                    return new HttpProxyClient(tcpClient);
                case ProxyType.Socks4:
                    return new Socks4ProxyClient(tcpClient);
                case ProxyType.Socks4A:
                    return new Socks4AProxyClient(tcpClient);
                case ProxyType.Socks5:
                    return new Socks5ProxyClient(tcpClient);
                default:
                    throw new ProxyException(String.Format("Unknown proxy type {0}.", type.ToString()));
            }
        }        
        
        /// <summary>
        /// Factory method for creating new proxy client objects.  
        /// </summary>
        /// <param name="type">The type of proxy client to create.</param>
        /// <param name="proxyHost">The proxy host or IP address.</param>
        /// <param name="proxyPort">The proxy port number.</param>
        /// <returns>Proxy client object.</returns>
        public IProxyClient CreateProxyClient(ProxyType type, string proxyHost, int proxyPort)
        {
            if (type == ProxyType.None)
                throw new ArgumentOutOfRangeException("type");
            
            switch (type)
            {
                case ProxyType.Http:
                    return new HttpProxyClient(proxyHost, proxyPort);
                case ProxyType.Socks4:
                    return new Socks4ProxyClient(proxyHost, proxyPort);
                case ProxyType.Socks4A:
                    return new Socks4AProxyClient(proxyHost, proxyPort);
                case ProxyType.Socks5:
                    return new Socks5ProxyClient(proxyHost, proxyPort);
                default:
                    throw new ProxyException(String.Format("Unknown proxy type {0}.", type.ToString()));
            }
        }

        /// <summary>
        /// Factory method for creating new proxy client objects.  
        /// </summary>
        /// <param name="type">The type of proxy client to create.</param>
        /// <param name="proxyHost">The proxy host or IP address.</param>
        /// <param name="proxyPort">The proxy port number.</param>
        /// <param name="proxyUsername">The proxy username.  This parameter is only used by Http, Socks4 and Socks5 proxy objects.</param>
        /// <param name="proxyPassword">The proxy user password.  This parameter is only used Http, Socks5 proxy objects.</param>
        /// <returns>Proxy client object.</returns>
        public IProxyClient CreateProxyClient(ProxyType type, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword)
        {
            if (type == ProxyType.None)
                throw new ArgumentOutOfRangeException("type");

            switch (type)
            {
                case ProxyType.Http:
                    return new HttpProxyClient(proxyHost, proxyPort, proxyUsername, proxyPassword);
                case ProxyType.Socks4:
                    return new Socks4ProxyClient(proxyHost, proxyPort, proxyUsername);
                case ProxyType.Socks4A:
                    return new Socks4AProxyClient(proxyHost, proxyPort, proxyUsername);
                case ProxyType.Socks5:
                    return new Socks5ProxyClient(proxyHost, proxyPort, proxyUsername, proxyPassword);
                default:
                    throw new ProxyException(String.Format("Unknown proxy type {0}.", type.ToString()));
            }
        }

        /// <summary>
        /// Factory method for creating new proxy client objects.  
        /// </summary>
        /// <param name="type">The type of proxy client to create.</param>
        /// <param name="tcpClient">Open TcpClient object.</param>
        /// <param name="proxyHost">The proxy host or IP address.</param>
        /// <param name="proxyPort">The proxy port number.</param>
        /// <param name="proxyUsername">The proxy username.  This parameter is only used by Http, Socks4 and Socks5 proxy objects.</param>
        /// <param name="proxyPassword">The proxy user password.  This parameter is only used Http, Socks5 proxy objects.</param>
        /// <returns>Proxy client object.</returns>
        public IProxyClient CreateProxyClient(ProxyType type, TcpClient tcpClient, string proxyHost, int proxyPort, string proxyUsername, string proxyPassword)
        {
            var c = CreateProxyClient(type, proxyHost, proxyPort, proxyUsername, proxyPassword);
            c.TcpClient = tcpClient;
            return c;
        }

        /// <summary>
        /// Factory method for creating new proxy client objects from Network Config
        /// </summary>
        /// <returns>Proxy client object.</returns>
        public IProxyClient CreateProxyClient()
        {
            //Read Network Setting file.
            var config = ConfigProvider.GetNetworkConfig();

            if (config.ProxyType == ProxyType.None)
            {
                return null;
            }

            return config.UseCredential ? CreateProxyClient(config.ProxyType, config.ProxyHost, config.ProxyPort, config.UserName, config.Password) : CreateProxyClient(config.ProxyType, config.ProxyHost, config.ProxyPort);
        }
    }
}
