using Common.Net.Proxy;

namespace Dto.ApiResponses.ConfigResponses
{
    public class NetworkConfigResponse : WebApiResponseBase
    {
        public ProxyType ProxyType { get; set; }
        public string ProxyHost { get; set; }
        public int ProxyPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseCredential { get; set; }
    }
}