using System;

namespace Common.Net.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AsyncSocketCallErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public AsyncSocketCallErrorEventArgs(Exception ex)
        {
            Exception = ex;
        }
    }
}
