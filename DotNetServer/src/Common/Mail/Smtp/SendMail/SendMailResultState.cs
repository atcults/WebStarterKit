namespace Common.Mail.Smtp.SendMail
{
    /// <summary>
    /// This class represent send mail result state.
    /// </summary>
    public enum SendMailResultState
    {
        /// <summary>
        /// Could not open connection to mail server
        /// </summary>
        Connection,
        /// <summary>
        /// Unknown error
        /// </summary>
        InvalidState,
        /// <summary>
        /// Helo command response is invalid
        /// </summary>
        Helo,
        /// <summary>
        /// Start tls command is failure
        /// </summary>
        Tls,
        /// <summary>
        /// Authenticate failure
        /// </summary>
        Authenticate,
        /// <summary>
        /// Invalid mail from
        /// </summary>
        MailFrom,
        /// <summary>
        /// All recpt failure
        /// </summary>
        Rcpt,
        /// <summary>
        /// Data command failure
        /// </summary>
        Data,
        /// <summary>
        /// Failure when from,rcpt,data command executing.
        /// </summary>
        SendMailData,
        /// <summary>
        /// 
        /// </summary>
        Success,
    }
}
