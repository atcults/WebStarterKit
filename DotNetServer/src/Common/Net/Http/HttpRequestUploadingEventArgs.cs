using System;

namespace Common.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpRequestUploadingEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Int32 Size { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 TotalSize { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="totalSize"></param>
        public HttpRequestUploadingEventArgs(Int32 size, Int32 totalSize)
        {
            Size = size;
            TotalSize = totalSize;
        }
    }
}
