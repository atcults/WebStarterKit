using System;

namespace Common.Mail.Pop3
{
	/// <summary>The exception that is thrown when receive response error occurs.
	/// </summary>
    public class InvalidMailMessageException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public String MailText { get; private set; }
		/// <summary>
		/// 
		/// </summary>
        public InvalidMailMessageException()
        {
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="innerException"></param>
	    public InvalidMailMessageException(Exception innerException)
			: base(innerException.Message, innerException)
		{
            MailText = MailText;
		}
	}
}
