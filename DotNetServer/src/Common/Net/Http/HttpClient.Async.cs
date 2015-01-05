using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Common.Net.Http
{
    public partial class HttpClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected Task<T> CreateNewTask<T>(Func<T> func)
        {
            return Task.Factory.StartNew(func);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<HttpWebResponse> GetHttpWebResponseAsync(String url)
        {
            return CreateNewTask(() => GetHttpWebResponse(url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<HttpWebResponse> GetHttpWebResponseAsync(String url, HttpBodyFormUrlEncodedData data)
        {
            return CreateNewTask(() => GetHttpWebResponse(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<HttpWebResponse> GetHttpWebResponseAsync(String url, Byte[] data)
        {
            return CreateNewTask(() =>GetHttpWebResponse(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task<HttpWebResponse> GetHttpWebResponseAsync(String url, Stream stream)
        {
            return CreateNewTask(() => GetHttpWebResponse(url, stream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task<HttpWebResponse> GetHttpWebResponseAsync(HttpRequestCommand command)
        {
            return CreateNewTask(() => GetHttpWebResponse(command));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<HttpResponse> GetResponseAsync(String url)
        {
            return CreateNewTask(() => GetResponse(url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<HttpResponse> GetResponseAsync(String url, HttpBodyFormUrlEncodedData data)
        {
            return CreateNewTask(() => GetResponse(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<HttpResponse> GetResponseAsync(String url, Byte[] data)
        {
            return CreateNewTask(() => GetResponse(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task<HttpResponse> GetResponseAsync(String url, Stream stream)
        {
            return CreateNewTask(() => GetResponse(url, stream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task<HttpResponse> GetResponseAsync(HttpRequestCommand command)
        {
            return CreateNewTask(() => GetResponse(command));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<String> GetBodyTextAsync(String url)
        {
            return CreateNewTask(() => GetBodyText(url));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<String> GetBodyTextAsync(String url, HttpBodyFormUrlEncodedData data)
        {
            return CreateNewTask(() => GetBodyText(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<String> GetBodyTextAsync(String url, Byte[] data)
        {
            return CreateNewTask(() => GetBodyText(url, data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task<String> GetBodyTextAsync(String url, Stream stream)
        {
            return CreateNewTask(() => GetBodyText(url, stream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task<String> GetBodyTextAsync(HttpRequestCommand command)
        {
            return CreateNewTask(() => GetBodyText(command));
        }
    }
}
