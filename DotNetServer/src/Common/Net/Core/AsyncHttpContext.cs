using System;
using System.IO;
using System.Net;
using Common.Net.Http;

namespace Common.Net.Core
{
    /// <summary>
    /// Reperesent Asynchronous http context data
    /// </summary>
    [Serializable]
    public class AsyncHttpContext
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<HttpRequestUploadingEventArgs> Uploading;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AsyncHttpCallErrorEventArgs> Error;
        private readonly HttpRequestCommand _command;
        private Action<HttpWebResponse> _callback;
        /// <summary>
        /// Define the size of buffer of requested message
        /// </summary>
        public Int32? RequestBufferSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Action<HttpWebResponse> Callback
        {
            get { return _callback; }
            set { _callback = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="callback"></param>
        public AsyncHttpContext(HttpRequestCommand command, Action<HttpWebResponse> callback)
        {
            _command = command;
            _callback = callback;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="requestBufferSize"></param>
        /// <param name="callback"></param>
        public AsyncHttpContext(HttpRequestCommand command, Int32 requestBufferSize, Action<HttpWebResponse> callback)
        {
            _command = command;
            RequestBufferSize = requestBufferSize;
            _callback = callback;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public void BeginRequest(HttpWebRequest request)
        {
            var req = request;
            Int64 length = 0;

            if (_command.BodyStream == null || _command.IsSendBodyStream == false)
            {
                req.BeginGetResponse(GetResponse, request);
            }
            else
            {
                var stream = _command.BodyStream;
                if (stream != null)
                {
                    try
                    {
                        length = stream.Length;
                    }
                    catch (NotSupportedException)
                    { throw new NotSupportedException("This stream is not supported to read length property."); }
                }

                if (length == 0)
                {
                    req.BeginGetResponse(GetResponse, req);
                }
                else
                {
                    if (RequestBufferSize <= 0) { throw new InvalidOperationException("RequestBufferSize must be larger than zero."); }
                    req.BeginGetRequestStream(WriteRequestStream, req);
                }
            }
        }

        private void WriteRequestStream(IAsyncResult result)
        {
            Stream stm = null;
            try
            {
                var req = result.AsyncState as HttpWebRequest;
                if (req != null) stm = req.EndGetRequestStream(result);
                var scx = RequestBufferSize.HasValue ? new StreamWriteContext(stm, RequestBufferSize.Value) : new StreamWriteContext(stm);
                scx.Uploading += (o, e) => OnUploading(e);
                scx.Write(_command.BodyStream);
                if (stm != null) stm.Dispose();
                stm = null;
                if (req != null) req.BeginGetResponse(GetResponse, req);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            finally
            {
                if (stm != null)
                {
                    stm.Dispose();
                }
            }
        }

        private void GetResponse(IAsyncResult result)
        {
            try
            {
                var req = result.AsyncState as HttpWebRequest;
                if (req != null)
                {
                    var res = req.EndGetResponse(result) as HttpWebResponse;
                    OnCallback(res);
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnUploading(HttpRequestUploadingEventArgs e)
        {
            var uploadData = Uploading;
            if (uploadData != null)
            {
                uploadData(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        protected void OnCallback(HttpWebResponse response)
        {
            var callbackData = Callback;
            if (callbackData != null)
            {
                callbackData(response);
            }
        }

        /// <summary>
        /// Define error when exception raised during connection
        /// </summary>
        /// <param name="exception"></param>
        protected void OnError(Exception exception)
        {
            var eh = Error;
            if (eh != null)
            {
                eh(this, new AsyncHttpCallErrorEventArgs(this, exception));
            }
        }
    }
}
