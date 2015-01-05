using System;
using System.Collections.Generic;

namespace Common.Net.SocketClient
{
	/// <summary>
	/// A manager to handle buffers for the socket connections
	/// </summary>
	/// <remarks>
	/// When used in an async call a buffer is pinned. Large numbers of pinned buffers
	/// cause problem with the GC (in particular it causes heap fragmentation).
	///
	/// This class maintains a set of large segments and gives clients pieces of these
	/// segments that they can use for their buffers. The alternative to this would be to
    /// create many small arrays which it then maintains. This methodology should be slightly
	/// better than the many small array methodology because in creating only a few very
	/// large Objects it will force these Objects to be placed on the LOH. Since the
	/// Objects are on the LOH they are at this time not subject to compacting which would
	/// require an update of all GC roots as would be the case with lots of smaller arrays
	/// that were in the normal heap.
	/// </remarks>
    public class BufferManager
    {
        private readonly Int32 _poolSize = 512; // initial size of the pool
        private readonly Int32 _bufferSize = 1024; // size of the buffers
        private Int32 _dequeueRetryCount = 4;
        private Queue<Byte[]> _buffers;
        private readonly Object _lockObject = new Object();
        /// <summary>
        /// 
        /// </summary>
        public Int32 DequeueRetryCount
        {
            get { return _dequeueRetryCount; }
            set { _dequeueRetryCount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public BufferManager()
        {
            Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poolSize"></param>
        /// <param name="bufferSize"></param>
        public BufferManager(Int32 poolSize, Int32 bufferSize)
        {
            _poolSize = poolSize;
            _bufferSize = bufferSize;
            Initialize();
        }

        private void Initialize()
        {
            _buffers = new Queue<Byte[]>(_poolSize);
            for (var i = 0; i < _poolSize; i++)
            {
                _buffers.Enqueue(new byte[_bufferSize]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Byte[] CheckOut()
        {
            var count = 1;

            while (true)
            {
                if (_buffers.Count > 0)
                {
                    lock (_lockObject)
                    {
                        if (_buffers.Count > 0)
                        {
                            return _buffers.Dequeue();
                        }
                    }
                }
                count += 1;
                if (count > _dequeueRetryCount) { break; }
                System.Threading.Thread.Sleep(100);
            }
            throw new InvalidOperationException("Buffer dequeue failed.You must be set more pool size.Or some class may not CheckIn buffer ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public void CheckIn(byte[] buffer)
        {
            lock (_lockObject)
            {
                _buffers.Enqueue(buffer);
            }
        }
    }
}