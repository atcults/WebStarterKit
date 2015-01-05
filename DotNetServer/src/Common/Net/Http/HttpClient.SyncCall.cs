using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Common.Net.Core;

namespace Common.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public partial class HttpClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpWebResponse GetHttpWebResponse(String url)
        {
            return GetHttpWebResponse(new HttpRequestCommand(url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpWebResponse GetHttpWebResponse(String url, HttpBodyFormUrlEncodedData data)
        {
            return GetHttpWebResponse(new HttpRequestCommand(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpWebResponse GetHttpWebResponse(String url, Byte[] data)
        {
            return GetHttpWebResponse(new HttpRequestCommand(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public HttpWebResponse GetHttpWebResponse(String url, Stream stream)
        {
            return GetHttpWebResponse(new HttpRequestCommand(url, stream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public HttpWebResponse GetHttpWebResponse(HttpRequestCommand command)
        {
            var req = CreateRequest(command);

            if (command.BodyStream != null && command.IsSendBodyStream)
            {
                //Request body
                //req.ContentLength = command.BodyStream.Length;
                if (command.BodyStream.Length > 0)
                {
                    using (var stm = req.GetRequestStream())
                    {
                        var scx = RequestBufferSize.HasValue ? new StreamWriteContext(stm, RequestBufferSize.Value) : new StreamWriteContext(stm);
                        scx.Uploading += (o, e) => OnUploading(e);
                        scx.Write(command.BodyStream);
                        stm.Dispose();
                    }
                }
            }
            return req.GetResponseAsync().Result as HttpWebResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpResponse GetResponse(String url)
        {
            return GetResponse(new HttpRequestCommand(url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponse GetResponse(String url, HttpBodyFormUrlEncodedData data)
        {
            return GetResponse(new HttpRequestCommand(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponse GetResponse(String url, Byte[] data)
        {
            return GetResponse(new HttpRequestCommand(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public HttpResponse GetResponse(String url, Stream stream)
        {
            return GetResponse(new HttpRequestCommand(url, stream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public HttpResponse GetResponse(HttpRequestCommand command)
        {
            HttpWebResponse res ;
            HttpResponse hr ;
            try
            {
                res = GetHttpWebResponse(command);
                hr = new HttpResponse(res, ResponseEncoding);
            }
            catch (WebException ex)
            {
                res = ex.Response as HttpWebResponse;
                if (res != null)
                {
                    hr = new HttpResponse(res, ResponseEncoding);
                }
                else
                {
                    throw;
                }
            }
            return hr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public String GetBodyText(String url)
        {
            return GetBodyText(new HttpRequestCommand(url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public String GetBodyText(String url, HttpBodyFormUrlEncodedData data)
        {
            return GetBodyText(new HttpRequestCommand(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public String GetBodyText(String url, Byte[] data)
        {
            return GetBodyText(new HttpRequestCommand(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public String GetBodyText(String url, Stream stream)
        {
            return GetBodyText(new HttpRequestCommand(url, stream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public String GetBodyText(HttpRequestCommand command)
        {
            var res = GetHttpWebResponse(command);
            if (res != null)
            {
                var sr = new StreamReader(res.GetResponseStream(), ResponseEncoding);
                return sr.ReadToEnd();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="idKey"></param>
        /// <param name="id"></param>
        /// <param name="passwordKey"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static CookieContainer GetCookieContainer(String url, String idKey, String id, String passwordKey, String password)
        {
            return GetCookieContainer(url, DefaultEncoding, idKey, id, passwordKey, password, new Dictionary<string, string>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseEncoding"></param>
        /// <param name="idKey"></param>
        /// <param name="id"></param>
        /// <param name="passwordKey"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static CookieContainer GetCookieContainer(String url, Encoding responseEncoding, String idKey, String id, String passwordKey, String password)
        {
            return GetCookieContainer(url, responseEncoding, idKey, id, passwordKey, password, new Dictionary<string, string>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseEncoding"></param>
        /// <param name="idKey"></param>
        /// <param name="id"></param>
        /// <param name="passwordKey"></param>
        /// <param name="password"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static CookieContainer GetCookieContainer(String url, Encoding responseEncoding, String idKey, String id, String passwordKey, String password
            , Dictionary<String, String> values)
        {
            var cookieContainer = new CookieContainer();
            var client = new HttpClient {ResponseEncoding = responseEncoding, CookieContainer = cookieContainer};

            var reqCmd = new HttpRequestCommand(url)
                {
                    ContentType = ApplicationFormUrlEncoded,
                    MethodName = HttpMethodName.Post
                };
            var d = reqCmd.Headers;
            d[idKey] = id;
            d[passwordKey] = password;
            foreach (var key in values.Keys)
            {
                d[key] = values[key];
            }
            client.GetResponse(reqCmd);

            return cookieContainer;
        }
    } 
}
