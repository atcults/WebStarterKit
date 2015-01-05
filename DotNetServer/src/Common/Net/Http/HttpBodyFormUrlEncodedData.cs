using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Net.Http
{
    /// <summary>
    /// This class Represent http body form url of encoded data
    /// </summary>
    public class HttpBodyFormUrlEncodedData
    {
        private readonly Dictionary<String, String> _values;
        /// <summary>
        /// 
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, String> Values
        {
            get { return _values; }
        }

        /// <summary>
        /// 
        /// </summary>
        public HttpBodyFormUrlEncodedData()
        {
            Encoding = HttpClient.DefaultEncoding;
            _values = new Dictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public HttpBodyFormUrlEncodedData(Dictionary<String, String> values)
        {
            Encoding = HttpClient.DefaultEncoding;
            _values = values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="values"></param>
        public HttpBodyFormUrlEncodedData(Encoding encoding, Dictionary<String, String> values)
        {
            Encoding = encoding;
            _values = values;
        }
    }
}
