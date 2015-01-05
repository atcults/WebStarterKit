using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Common.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class HttpRequestCommand
    {
        private readonly WebHeaderCollection _headers = new WebHeaderCollection();

        /// <summary>
        /// 
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpMethodName MethodName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsSendBodyStream { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String ContentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Stream BodyStream { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<String, String> UrlEncodeFunction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public HttpRequestCommand(String url)
        {
            InitializeProperty();
            Url = url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="methodName"></param>
        public HttpRequestCommand(String url, HttpMethodName methodName)
        {
            InitializeProperty();
            Url = url;
            MethodName = methodName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        public HttpRequestCommand(String url, Stream stream)
        {
            InitializeProperty();
            Url = url;
            MethodName = HttpMethodName.Post;
            BodyStream = stream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public HttpRequestCommand(String url, HttpBodyFormUrlEncodedData data)
        {
            InitializeProperty();
            Url = url;
            MethodName = HttpMethodName.Post;
            ContentType = HttpClient.ApplicationFormUrlEncoded;
            BodyStream = new MemoryStream(CreateRequestBodyData(data.Encoding, data.Values));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public HttpRequestCommand(String url, Byte[] data)
        {
            InitializeProperty();
            Url = url;
            MethodName = HttpMethodName.Post;
            BodyStream = new MemoryStream(data);
        }

        private void InitializeProperty()
        {
            UrlEncodeFunction = HttpClient.UrlEncode;
            MethodName = HttpMethodName.Get;
            IsSendBodyStream = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public Byte[] CreateRequestBodyData(Encoding encoding, Dictionary<String, String> values)
        {
            var sb = new StringBuilder(512);
            var d = values;
            if (d == null || d.Keys.Count == 0) { return new Byte[0]; }

            var isFirst = true;
            foreach (var key in d.Keys)
            {
                if (isFirst )
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append("&");
                }
                sb.AppendFormat("{0}={1}", UrlEncodeFunction(key), UrlEncodeFunction(d[key]));
            }
            return encoding.GetBytes(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetBodyStream(HttpBodyFormUrlEncodedData data)
        {
            if (String.IsNullOrEmpty(ContentType) )
            {
                ContentType = HttpClient.ApplicationFormUrlEncoded;
            }
            var bb = CreateRequestBodyData(data.Encoding, data.Values);
            BodyStream = new MemoryStream(bb);
            IsSendBodyStream = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetBodyStream(Byte[] data)
        {
            BodyStream = new MemoryStream(data);
            IsSendBodyStream = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void SetBodyStream(Stream stream)
        {
            BodyStream = stream;
            IsSendBodyStream = true;
        }
    }
}
