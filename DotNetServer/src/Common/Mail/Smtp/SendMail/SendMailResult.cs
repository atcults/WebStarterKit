using System;
using System.Collections.Generic;
using Common.Mail.Common;

namespace Common.Mail.Smtp.SendMail
{
    /// Represent the result of sending smtp mail.
    /// <summary>
    /// Represent the result of sending smtp mail.
    /// </summary>
    public class SendMailResult
    {
        private readonly List<MailAddress> _invalidMailAddressList = new List<MailAddress>();
        /// <summary>
        /// Send notification to the sender that send mail successful
        /// </summary>
        public Boolean SendSuccessful
        {
            get { return State == SendMailResultState.Success; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SendMailResultState State { get; private set; }
		/// <summary>
		/// 
		/// </summary>
        public List<MailAddress> InvalidMailAddressList
        {
            get { return _invalidMailAddressList; }
        }

		/// <summary>
		/// 
		/// </summary>
        public SendMailCommand Command { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="command"></param>
        public SendMailResult(SendMailResultState state, SendMailCommand command)
        {
            State = state;
            Command = command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="command"></param>
        /// <param name="invalidMailAddressList"></param>
        public SendMailResult(SendMailResultState state, SendMailCommand command, IEnumerable<MailAddress> invalidMailAddressList)
        {
            State = state;
            Command = command;
            InvalidMailAddressList.AddRange(invalidMailAddressList);
        }
    }
}
