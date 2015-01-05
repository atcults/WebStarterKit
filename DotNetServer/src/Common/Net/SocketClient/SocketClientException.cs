using System;

namespace Common.Net.SocketClient
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SocketClientException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public SocketClientException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public SocketClientException(String message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public SocketClientException(Exception exception) : base(exception.Message, exception)
        {
        }
    }
}
