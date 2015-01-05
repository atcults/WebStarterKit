using System;
using System.Collections.Generic;

namespace Common.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpResponseException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpResponse HttpResponse { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, String> Headers
        {
            get { return HttpResponse.Headers; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String BodyText
        {
            get { return HttpResponse.BodyText; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponse"></param>
        public HttpResponseException(HttpResponse httpResponse)
        {
            HttpResponse = httpResponse;
        }
    }
}
