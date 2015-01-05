using Common.Net.Proxy;

namespace Dto.ApiRequests.ConfigForms
{
    public class NetworkConfigForm : FormBase
    {
        public string ProxyHost { get; set; }
        public int ProxyPort { get; set; }
        public ProxyType ProxyType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseCredential { get; set; }
        public bool UseProxy { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1} [{2}]", base.ToString(), ProxyHost, UseProxy);
        }

        public override string GetApiAddress()
        {
            return "/CoreConfiguration/Network";
        }
    }
}