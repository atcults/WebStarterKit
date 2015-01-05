using System;
using System.Collections.Generic;
using Common.Mail.Common;

namespace Common.Mail.Smtp.SendMail
{
    /// <summary>
    /// 
    /// </summary>
    public class SendMailCommand
    {
        private readonly List<MailAddress> _rcptTo = new List<MailAddress>();
        /// <summary>
        /// 
        /// </summary>
        public MailAddress From { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MailAddress> RcptTo
        {
            get { return _rcptTo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="text"></param>
        /// <param name="rcptTo"></param>
        public SendMailCommand(String from, String text, IEnumerable<MailAddress> rcptTo)
        {
            From = new MailAddress(from);
            RcptTo.AddRange(rcptTo);
            Text = text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public SendMailCommand(SmtpMessage message)
        {
            From = message.From;
            var addresslist = new List<MailAddress>();
            addresslist.AddRange(message.To);
            addresslist.AddRange(message.Cc);
            addresslist.AddRange(message.Bcc);
            RcptTo.AddRange(addresslist);
            Text = message.GetDataText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="message"></param>
        public SendMailCommand(String from, SmtpMessage message)
        {
            From = new MailAddress(from);
            var addresslist = new List<MailAddress>();
            addresslist.AddRange(message.To);
            addresslist.AddRange(message.Cc);
            addresslist.AddRange(message.Bcc);
            RcptTo.AddRange(addresslist);
            Text = message.GetDataText();
        }
    }
}
