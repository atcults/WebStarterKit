using System;
using Common.Net.SocketClient;

namespace Common.Mail.Common
{
    /// <summary>
    /// This class is used to throw exception when send mail message
    /// </summary>
    [Serializable]
    public class MailClientException : SocketClientException
    {
        /// <summary>
        /// 
        /// </summary>
        public MailClientException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MailClientException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public MailClientException(Exception exception)
            : base(exception)
        {
        }
    }
}
