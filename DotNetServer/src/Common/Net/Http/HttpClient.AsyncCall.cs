using System;
using System.IO;
using System.Net;
using System.Text;
using Common.Net.Core;

namespace Common.Net.Http
{
    /// <summary>
    /// In HTTP request and response data to retrieve the class that provides the functionality you want.
    /// </summary>
    public partial class HttpClient
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="context"></param>
        public void GetHttpWebResponse(String url, AsyncHttpContext context)
        {
            var req = CreateRequest(new HttpRequestCommand(url));
            context.BeginRequest(req);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void GetHttpWebResponse(String url, Action<HttpWebResponse> callback)
        {
            GetHttpWebResponse(new HttpRequestCommand(url), callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void GetHttpWebResponse(String url, HttpBodyFormUrlEncodedData data, Action<HttpWebResponse> callback)
        {
            GetHttpWebResponse(new HttpRequestCommand(url, data), callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void GetHttpWebResponse(String url, Byte[] data, Action<HttpWebResponse> callback)
        {
            GetHttpWebResponse(new HttpRequestCommand(url, data), callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <param name="callback"></param>
        public void GetHttpWebResponse(String url, Stream stream, Action<HttpWebResponse> callback)
        {
            var httpReqCmd = new HttpRequestCommand(url, stream);
            var req = CreateRequest(httpReqCmd);
            var httpContext = RequestBufferSize.HasValue ? new AsyncHttpContext(httpReqCmd,RequestBufferSize.Value, AsyncHttpContextCallback(callback)) : new AsyncHttpContext(httpReqCmd, AsyncHttpContextCallback(callback));
            httpContext.Uploading += (o, e) => OnUploading(e);
            httpContext.Error += (o, e) => OnError(e);
            httpContext.BeginRequest(req);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="callback"></param>
        public void GetHttpWebResponse(HttpRequestCommand command, Action<HttpWebResponse> callback)
        {
            var req = CreateRequest(command);
            var httpContext = CreateAsyncHttpContext(command, callback);
            httpContext.Uploading += (o, e) => OnUploading(e);
            httpContext.Error += (o, e) => OnError(e);
            httpContext.BeginRequest(req);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected AsyncHttpContext CreateAsyncHttpContext(HttpRequestCommand command, Action<HttpWebResponse> callback)
        {
            var httpContext = RequestBufferSize.HasValue ? new AsyncHttpContext(command, RequestBufferSize.Value, AsyncHttpContextCallback(callback)) : new AsyncHttpContext(command, AsyncHttpContextCallback(callback));
            return httpContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        private Action<HttpWebResponse> AsyncHttpContextCallback(Action<HttpWebResponse> callback)
        {
            Action<HttpWebResponse> f = res =>
            {
                var httpwebRes = callback;
                if (httpwebRes == null) return;
                if (BeginInvoke == null)
                {
                    httpwebRes(res);
                }
                else
                {
                    BeginInvoke(() => httpwebRes(res));
                }
            };
            return f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void GetResponse(String url, Action<HttpResponse> callback)
        {
            GetResponse(new HttpRequestCommand(url), callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void GetResponse(String url, HttpBodyFormUrlEncodedData data, Action<HttpResponse> callback)
        {
            GetResponse(new HttpRequestCommand(url, data), callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void GetResponse(String url, Byte[] data, Action<HttpResponse> callback)
        {
            GetHttpWebResponse(new HttpRequestCommand(url, data), res => this.GetResponseCallback(res, callback));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <param name="callback"></param>
        public void GetResponse(String url, Stream stream, Action<HttpResponse> callback)
        {
            GetHttpWebResponse(new HttpRequestCommand(url, stream), res => GetResponseCallback(res, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="callback"></param>
        public void GetResponse(HttpRequestCommand command, Action<HttpResponse> callback)
        {
            var req = CreateRequest(command);
            var cx = CreateAsyncHttpContext(command, res => GetResponseCallback(res, callback));
            cx.Uploading += (o, e) => OnUploading(e);
            cx.Error += (o, e) => GetResponseErrorCallback(e, callback);
            cx.BeginRequest(req);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="callback"></param>
        protected void GetResponseCallback(HttpWebResponse response, Action<HttpResponse> callback)
        {
            var res = response;
            var hr = new HttpResponse(res, ResponseEncoding);
            callback(hr);
        }

        private void GetResponseErrorCallback(AsyncHttpCallErrorEventArgs e, Action<HttpResponse> callback)
        {
            var ex = e.Exception as WebException;
            if (ex == null)
            {
                OnError(e);
                return;
            }
            var res = ex.Response as HttpWebResponse;
            if (res == null)
            {
                OnError(e);
                return;
            }
            GetResponseCallback(res, callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void GetBodyText(String url, Action<String> callback)
        {
            GetBodyText(new HttpRequestCommand(url), callback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void GetBodyText(String url, HttpBodyFormUrlEncodedData data, Action<String> callback)
        {
            GetBodyText(new HttpRequestCommand(url, data), text => callback(text));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        public void GetBodyText(String url, Byte[] data, Action<String> callback)
        {
            GetHttpWebResponse(new HttpRequestCommand(url, data), res => GetBodyTextCallback(res, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <param name="callback"></param>
        public void GetBodyText(String url, Stream stream, Action<String> callback)
        {
            GetHttpWebResponse(new HttpRequestCommand(url, stream), res => GetBodyTextCallback(res, callback));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="callback"></param>
        public void GetBodyText(HttpRequestCommand command, Action<String> callback)
        {
            GetHttpWebResponse(command, res => GetBodyTextCallback(res, callback));
        }

        private void GetBodyTextCallback(HttpWebResponse response, Action<String> callback)
        {
            GetBodyTextCallback(response, DefaultEncoding, callback);
        }

        private void GetBodyTextCallback(HttpWebResponse response, Encoding responseEncoding, Action<String> callback)
        {
            var res = response;
            var sr = new StreamReader(res.GetResponseStream(), responseEncoding);
            var text = sr.ReadToEnd();
            callback(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnError(AsyncHttpCallErrorEventArgs e)
        {
            var eh = Error;
            if (eh != null)
            {
                if (BeginInvoke == null)
                {
                    eh(this, e);
                }
                else
                {
                    BeginInvoke(() => eh(this, e));
                }
            }
        }
    }
}
