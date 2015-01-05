using System;
using System.IO;
using Common.Net.Http;

namespace Common.Net.Core
{
    /// <summary>
    /// This class defines stream data that you want to write in text.
    /// </summary>
    internal class StreamWriteContext
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<HttpRequestUploadingEventArgs> Uploading;
        private readonly Stream _targetStream;
        private Int32? _bufferSize ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetStream"></param>
        public StreamWriteContext(Stream targetStream)
        {
            if (targetStream == null) { throw new ArgumentNullException("targetStream"); }
            _targetStream = targetStream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetStream"></param>
        /// <param name="bufferSize"></param>
        public StreamWriteContext(Stream targetStream, Int32 bufferSize)
        {
            if (targetStream == null) { throw new ArgumentNullException("targetStream"); }
            if (bufferSize <= 0) { throw new ArgumentException("bufferSize must be larger than zero"); }
            _targetStream = targetStream;
            _bufferSize = bufferSize;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Write(Byte[] data)
        {
            var mm = new MemoryStream(data);
            Write(mm);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        internal void Write(Stream sourceStream)
        {
            if (sourceStream.Length > Int32.MaxValue) { throw new NotSupportedException("sourceStream length must be less than Int32.MaxValue."); }
            var length = (Int32)sourceStream.Length;
            
            if (_bufferSize.HasValue)
            {
                var index = 0;
                var size = _bufferSize.Value;
                var isBreak = false;
                while (true)
                {
                    if (index + size >= length)
                    {
                        size = length - index;
                        isBreak = true;
                    }
                    var bb = new Byte[size];
                    sourceStream.Read(bb, 0, size);
                    _targetStream.Write(bb, 0, size);
                    OnUploading(new HttpRequestUploadingEventArgs(size, index + size));
                    if (isBreak) { break; }
                    index = index + size;
                }
            }
            else
            {
                sourceStream.CopyTo(_targetStream);
                OnUploading(new HttpRequestUploadingEventArgs(length, length));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnUploading(HttpRequestUploadingEventArgs e)
        {
            var eh = Uploading;
            if (eh != null)
            {
                eh(this, e);
            }
        }
    }
}
