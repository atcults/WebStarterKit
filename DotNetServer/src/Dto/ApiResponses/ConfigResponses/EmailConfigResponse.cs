namespace Dto.ApiResponses.ConfigResponses
{
    public class EmailConfigResponse : WebApiResponseBase
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
    }
}