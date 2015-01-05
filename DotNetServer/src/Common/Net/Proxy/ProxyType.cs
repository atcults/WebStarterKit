namespace Common.Net.Proxy
{
    /// <summary>
    /// The type of proxy.
    /// </summary>
    public enum ProxyType
    {
        /// <summary>
        /// No Proxy specified.  Note this option will cause an exception to be thrown if used to create a proxy object by the factory.
        /// </summary>
        None,
        /// <summary>
        /// HTTP Proxy
        /// </summary>
        Http,
        /// <summary>
        /// SOCKS v4 Proxy
        /// </summary>
        Socks4,
        /// <summary>
        /// SOCKS v4a Proxy
        /// </summary>
        Socks4A,
        /// <summary>
        /// SOCKS v5 Proxy
        /// </summary>
        Socks5
    }
}