using Common.Net.Proxy;

namespace Common.SystemSettings
{
    public class NetworkConfig
    {
        public NetworkConfig()
        {
            ProxyType = ProxyType.None;
            ProxyHost = "";
            ProxyPort = 0;
            UseCredential = false;
            UserName = "";
            Password = "";
        }

        public ProxyType ProxyType { get; set; }
        public string ProxyHost { get; set; }
        public int ProxyPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseCredential { get; set; }
    }
}