using System;
using Common.Base;
using Common.Mail.Common;
using Common.Mail.Smtp;

namespace Common.Service.Impl
{
    public class EmailSender : ServiceBase, IEmailSender
    {
        public bool SendTextEmail(string subject, string body, string toAddress = null)
        {
            var emailConfig = ConfigProvider.GetEmailConfig();

            if (string.IsNullOrEmpty(toAddress))
            {
                toAddress = emailConfig.From;
            }

            toAddress = toAddress.Trim();

            try
            {
                var mg = new SmtpMessage
                {
                    Subject = subject,
                    BodyText = body,
                    IsHtml = true,
                    From = new MailAddress(emailConfig.From, emailConfig.DisplayName)
                };

                mg.To.Add(new MailAddress(toAddress));
                var client = new SmtpClient
                {
                    ServerName = emailConfig.MailHost,
                    Port = emailConfig.Port,
                    HostName = emailConfig.MailHost,
                    UserName = emailConfig.User,
                    Password = emailConfig.Password,
                    Ssl = emailConfig.Ssl,
                    Tls = emailConfig.Tls
                };

                var rs = client.SendMail(mg);

                if (!rs.SendSuccessful)
                {
                    throw new Exception("Send state : " + rs.State);
                }
            }
            catch (Exception exception)
            {
                Logger.Log(LogType.Error, this, "Email send error", exception);
                Exception = exception;
                return false;
            }

            return true;    
        }
    }
}
