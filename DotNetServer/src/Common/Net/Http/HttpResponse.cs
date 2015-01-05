using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Common.Net.Extensions;

namespace Common.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpResponse
    {
        private readonly Dictionary<String, String> _headers = new Dictionary<String, String>();
        private readonly Byte[] _bodyData ;
        private readonly String _bodyText = "";
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String StatusDescription { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String ContentType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Int64 ContentLength { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public CookieCollection Cookies { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String Method { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String CharacterSet { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsMutuallyAuthenticated { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastModified { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Version ProtocolVersion { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Uri ResponseUri { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String Server { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsFromCache { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, String> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Byte[] BodyData
        {
            get { return _bodyData; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String BodyText
        {
            get { return _bodyText; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="encoding"></param>
        public HttpResponse(HttpWebResponse response, Encoding encoding)
        {
            var res = response;

            StatusCode = res.StatusCode;
            StatusDescription = res.StatusDescription;
            Method = res.Method;
            ContentType = res.ContentType;
            ContentLength = res.ContentLength;
            Cookies = res.Cookies;
            CharacterSet = res.CharacterSet;
            IsMutuallyAuthenticated = res.IsMutuallyAuthenticated;
            LastModified = res.LastModified;
            ProtocolVersion = res.ProtocolVersion;
            Server = res.Server;
            IsFromCache = res.IsFromCache;
            foreach (var key in res.Headers.AllKeys)
            {
                Headers[key] = res.Headers[key];
            }
            var bb = res.GetResponseStream().ToByteArray();
            var stm = new MemoryStream(bb);
            var reader = new StreamReader(stm, encoding);
            _bodyText = reader.ReadToEnd();
            _bodyData = bb;
        }
    }
}
