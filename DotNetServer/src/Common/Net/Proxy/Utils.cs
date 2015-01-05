using System;
using System.Globalization;
using System.Net.Sockets;

namespace Common.Net.Proxy
{
    internal static class Utils
    {
        internal static string GetHost(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            var host = "";
            try
            {
                host = ((System.Net.IPEndPoint) client.Client.RemoteEndPoint).Address.ToString();
            }
            catch
            {
                
            };

            return host;
        }

        internal static string GetPort(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            var port = "";
            try
            {
                port = ((System.Net.IPEndPoint) client.Client.RemoteEndPoint).Port.ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                
            };

            return port;
        }

    }
}
