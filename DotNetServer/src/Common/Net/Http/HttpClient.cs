using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Common.Net.Core;
using Common.Net.Proxy;
using Common.Service.Impl;

namespace Common.Net.Http
{
    /// <summary>
    /// In HTTP request and response data to retrieve the class that provides the functionality you want.
    /// </summary>
    public partial class HttpClient
    {
        private const String UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        internal static readonly Encoding DefaultEncoding = Encoding.GetEncoding("us-ascii");
        private readonly X509CertificateCollection _clientCertificates = new X509CertificateCollection();
        /// <summary>
        /// To post to the server WEB required header of characters to represent key of the attribute.
        /// </summary>
        public static readonly String ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<HttpWebRequestCreatedEventArgs> HttpWebRequestCreated;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<HttpRequestUploadingEventArgs> Uploading;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AsyncHttpCallErrorEventArgs> Error;
        /// <summary>
        /// 
        /// </summary>
        public Int32? RequestBufferSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Encoding ResponseEncoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CookieContainer CookieContainer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICredentials Credentials { get; set; }
        /// <summary>
        /// Certificate information
        /// </summary>
        public X509CertificateCollection ClientCertificates
        {
            get { return _clientCertificates; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<System.Action> BeginInvoke { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HttpClient()
        {
            ResponseEncoding = DefaultEncoding;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public HttpWebRequest CreateRequest(HttpRequestCommand command)
        {
            var req = WebRequest.Create(command.Url) as HttpWebRequest;

            if (req != null)
            {
                var settings = ConfigProvider.GetNetworkConfig();

                if (settings.ProxyType != ProxyType.None)
                {
                    req.Proxy = new WebProxy(settings.ProxyHost, settings.ProxyPort);
                    if (settings.UseCredential)
                    {
                        req.Proxy.Credentials = new NetworkCredential(settings.UserName, settings.Password);
                    }
                }

                req.Method = command.MethodName.ToString().ToUpper();
                req.ContentType = command.ContentType;
                if (CookieContainer != null)
                {
                    req.CookieContainer = CookieContainer;
                }
                if (Credentials != null)
                {
                    req.Credentials = Credentials;
                }
                req.ClientCertificates.AddRange(ClientCertificates);
                foreach (var key in command.Headers.AllKeys)
                {
                    req.Headers[key] = command.Headers[key];
                }
                OnHttpWebRequestCreated(new HttpWebRequestCreatedEventArgs(req));
                return req;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static String CreateQueryString(String baseUrl, IDictionary<String, String> parameters)
        {
            return CreateQueryString(baseUrl, parameters, s => s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="urlEncodingFunction"></param>
        /// <returns></returns>
        public static String CreateQueryString(String baseUrl, IDictionary<String, String> parameters, Func<String, String> urlEncodingFunction)
        {
            var result = CreateKeyEqualValueAndFormatString(parameters, urlEncodingFunction);
            if (String.IsNullOrEmpty(result))
            {
                return baseUrl;
            }
            return baseUrl + "?" + result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="urlEncodingFunction"></param>
        /// <returns></returns>
        public static String CreateKeyEqualValueAndFormatString(IDictionary<String, String> parameters, Func<String, String> urlEncodingFunction)
        {
            var sb = new StringBuilder(256);
            var first = true;
            foreach (var parameter in parameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append('&');
                }
                sb.Append(parameter.Key);
                sb.Append('=');
                sb.Append(urlEncodingFunction(parameter.Value));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UrlEncode(String value)
        {
            return UrlEncode(value, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string UrlEncode(String value, Encoding encode)
        {
            var result = new StringBuilder();
            var data = encode.GetBytes(value);
            var len = data.Length;

            for (var i = 0; i < len; i++)
            {
                int c = data[i];
                if (c < 0x80 && UnreservedChars.IndexOf((char)c) != -1)
                {
                    result.Append((char)c);
                }
                else
                {
                    result.Append('%' + String.Format("{0:X2}", (int)data[i]));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsNullOrWhiteSpace(String value)
        {
            if (String.IsNullOrEmpty(value)) { return true; }
            if (value.Trim().Length == 0) { return true; }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnUploading(HttpRequestUploadingEventArgs e)
        {
            OnEventHandler(Uploading, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnHttpWebRequestCreated(HttpWebRequestCreatedEventArgs e)
        {
            OnEventHandler(HttpWebRequestCreated, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="e"></param>
        protected void OnEventHandler<T>(EventHandler<T> handler, T e)
            where T : EventArgs
        {
            var eventHandler = handler;
            if (eventHandler != null)
            {
                if (BeginInvoke == null)
                {
                    eventHandler(this, e);
                }
                else
                {
                    BeginInvoke(() => eventHandler(this, e));
                }
            }
        }
    }
}
