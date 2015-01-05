using System;
using System.Net;

namespace Common.Net.Core
{
    /// <summary>
    /// This class defines http web request created event arguments
    /// </summary>
    public class HttpWebRequestCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Defines http web request
        /// </summary>
        public HttpWebRequest HttpWebRequest { get; private set; }
        /// <summary>
        /// Defines http web request created events arguments
        /// </summary>
        /// <param name="request"></param>
        public HttpWebRequestCreatedEventArgs(HttpWebRequest request)
        {
            HttpWebRequest = request;
        }
    }
}
