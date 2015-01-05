using System;

namespace Common.Net.SocketClient
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DataTransferContextException : SocketClientException
    {
        /// <summary>
        /// 
        /// </summary>
        public DataTransferContext Context { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String Text { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DataTransferContextException(DataTransferContext context)
        {
            Context = context;
            Text = Context.GetText();
        }
    }
}
