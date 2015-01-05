using System;
using System.IO;
using System.Text;
using Common.Net.Extensions;

namespace Common.Net.SocketClient
{
    /// <summary>
    /// 
    /// </summary>
    public class DataReceiveContext : DataTransferContext
    {
        private Action<String> _endGetResponse;
        private Int32 _readCount;
        /// <summary>
        /// 
        /// </summary>
        protected Action<String> EndGetResponse
        {
            get { return _endGetResponse; }
            set { _endGetResponse = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        public DataReceiveContext(Encoding encoding)
            : base(new MemoryStream(), encoding)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        public DataReceiveContext(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public Boolean ReadBuffer(Int32 size)
        {
            if (size == 0) { return false; }
            var bl = ParseBuffer(size);
            _readCount += 1;
            if (_readCount > 1000000) { throw new SocketClientException("Too much read count.Perhaps parser could not parse correctly."); }
            return bl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        protected virtual Boolean ParseBuffer(Int32 size)
        {
            var bb = GetByteArray();

            for (var i = 0; i < size; i++)
            {
                Stream.WriteByte(bb[i]);
                bb[i] = 0;
            }
            if (size < bb.Length)
            {
                return false;   
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected Byte[] GetLastByte(Int32 size)
        {
            Stream.Position = Stream.Length - size;
            return Stream.ToByteArray();
        }

        /// <summary>
        /// 
        /// </summary>
        internal protected void OnEndGetResponse()
        {
            var eh = _endGetResponse;
            if (eh != null)
            {
                eh(GetText());
            }
        }
    }
}
