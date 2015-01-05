using System;
using System.IO;
using System.Text;

namespace Common.Net.SocketClient
{
	/// <summary>
    /// Represent context of request and response process and provide data about context.
    /// </summary>
    internal class DataSendContext : DataTransferContext
    {
        private Int32 _sendBufferSize;
        internal Int32 SendBufferSize
        {
            get { return _sendBufferSize; }
        }

        internal Boolean DataRemained
        {
            get { return Stream.Position < Stream.Length; }
        }

        internal DataSendContext(Stream stream, Encoding encoding) :
            base(stream, encoding)
        {
            _sendBufferSize = (Int32)Stream.Length;
        }

        internal void FillBuffer()
        {
            var bb = GetByteArray();

            if (Stream.Position + bb.Length < Stream.Length)
            {
                Stream.Read(bb, 0, bb.Length);
                _sendBufferSize = bb.Length;
            }
            else
            {
                var count = (Int32)(Stream.Length - Stream.Position);
                Stream.Read(bb, 0, count);
                _sendBufferSize = count;
            }
        }
    }
}
