namespace Dto.ApiRequests.ConfigForms
{
    public class EmailConfigForm : FormBase
    {
        public string MailHost { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string ReplyTo { get; set; }
        public bool Ssl { get; set; }
        public bool Tls { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1} [{2}]", base.ToString(), MailHost, Port);
        }

        public override string GetApiAddress()
        {
            return "/CoreConfiguration/Email";
        }
    }
}