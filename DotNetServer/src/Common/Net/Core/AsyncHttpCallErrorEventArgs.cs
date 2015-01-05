using System;

namespace Common.Net.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AsyncHttpCallErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public AsyncHttpContext AsyncHttpContext { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        public AsyncHttpCallErrorEventArgs(AsyncHttpContext context, Exception ex)
        {
            AsyncHttpContext = context;
            Exception = ex;
        }
    }
}
