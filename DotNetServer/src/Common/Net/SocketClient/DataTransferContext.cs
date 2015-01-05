using System;
using System.IO;
using System.Text;
using Common.Net.Extensions;

namespace Common.Net.SocketClient
{
	/// <summary>
    /// Represent context of request and response process and provide data about context.
    /// </summary>
    public class DataTransferContext : IDisposable
    {
		private static BufferManager _bufferManager ;
		private DateTime _startTime = DateTime.Now;
		private readonly Byte[] _buffer;
        private readonly Stream _stream ;
	    private Encoding _encoding = Encoding.UTF8;
		private Boolean _isDisposed;
		/// <summary>
		/// 
		/// </summary>
		public static BufferManager BufferManager
		{
			get { return _bufferManager ?? (_bufferManager = new BufferManager(256, 8192)); }
		    set { _bufferManager = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        internal protected Stream Stream
        {
            get { return _stream; }
        }

        /// <summary>
        /// 
        /// </summary>
        internal protected DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected Encoding Encoding
		{
			get { return _encoding; }
			set { _encoding = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        protected Byte[] Buffer
        {
            get { return _buffer; }
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    public Exception Exception { get; set; }

	    /// <summary>
	    /// 
	    /// </summary>
	    public bool Timeout { get; set; }

	    internal DataTransferContext(Stream stream, Encoding encoding)
		{
            _stream = stream;
            _encoding = encoding;
			_buffer = BufferManager.CheckOut();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Byte[] GetByteArray()
        {
            return _buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal protected Byte[] GetData()
        {
            _stream.Position = 0;
            return _stream.ToByteArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal protected String GetText()
        {
            return Encoding.GetString(GetData());
        }

        /// To perform termination processing, to free up system resources.
		/// <summary>
		/// dipose and release system resoures.
        /// To perform termination processing, to free up system resources.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				if (_isDisposed == false &&
					_buffer !=null)
				{
					BufferManager.CheckIn(_buffer);
					_isDisposed = true;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		~DataTransferContext()
		{
			Dispose(false);
		}
    }
}
