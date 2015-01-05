namespace Common.SystemSettings
{
    public class EmailConfig
    {
        public EmailConfig()
        {
            MailHost = "";
            Port = 25;
            User = "";
            Password = "";
            From = "";
            DisplayName = "";
            ReplyTo = "";
            Ssl = false;
            Tls = false;
        }

        public string MailHost { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string ReplyTo { get; set; }
        public bool Ssl { get; set; }
        public bool Tls { get; set; }

        public override string ToString()
        {
            return string.Format("Host:{0}, Port:{1}, User:{2}, Password: {3}, From{4}, DisplayName:{5}, ReplyTo:{6}, SSL:{7}, TLS:{8}", MailHost, Port, User, Password, From, DisplayName, ReplyTo, Ssl, Tls);
        }
    }
}
