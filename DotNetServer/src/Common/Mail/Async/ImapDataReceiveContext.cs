using System;
using System.Text;
using Common.Net.Core;
using Common.Net.SocketClient;

namespace Common.Mail.Async
{
    /// <summary>
    /// Represent context of request and response process and provide data about context.
    /// </summary>
    public class ImapDataReceiveContext : DataReceiveContext
    {
        private enum ParseState
        {
            TagValidating, MultiLine, CarriageReturn, LastLine, LastLineCarriageReturn, 
        }
        private readonly Byte[] _tagBytes;
        private ParseState _state = ParseState.TagValidating;

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsFetchCommand { get; set; }
        internal ImapDataReceiveContext(String tag, Encoding encoding) :
            base(encoding)
        {
            _tagBytes = Encoding.GetBytes(tag);
            IsFetchCommand = false;
        }
        
        /// <summary>
        /// Read buffer data to Data property and initialize buffer.
        /// If response has next data,return true.
        /// </summary>
        /// <param name="size"></param>
        /// <returns>If response has next data,return true</returns>
        protected override Boolean ParseBuffer(Int32 size)
        {
            var bb = Buffer;
            var tagIndex = 0;

            for (var i = 0; i < size; i++)
            {
                Stream.WriteByte(bb[i]);
                if (_state == ParseState.TagValidating)
                {
                    if (bb[i] == _tagBytes[tagIndex])
                    {
                        tagIndex = tagIndex + 1;
                        if (_tagBytes.Length == tagIndex)
                        {
                            _state = ParseState.LastLine;
                        }
                    }
                    _state = ParseState.MultiLine;
                }
                else if (_state == ParseState.MultiLine)
                {
                    if (bb[i] == AsciiCharCode.CarriageReturn.GetNumber())
                    {
                        _state = ParseState.CarriageReturn;
                    }
                }
                else if (_state == ParseState.CarriageReturn)
                {
                    if (bb[i] == AsciiCharCode.LineFeed.GetNumber())
                    {
                        tagIndex = 0;
                        _state = ParseState.TagValidating;
                    }
                    else { throw new DataTransferContextException(this); }
                }
                else if (_state == ParseState.LastLine)
                {
                    if (bb[i] == AsciiCharCode.CarriageReturn.GetNumber())
                    {
                        _state = ParseState.LastLineCarriageReturn;
                    }
                }
                else if (_state == ParseState.LastLineCarriageReturn)
                {
                    if (bb[i] == AsciiCharCode.LineFeed.GetNumber())
                    {
                        return false;
                    }
                     throw new DataTransferContextException(this); 
                }
                bb[i] = 0;
            }
            return true;
        }
    }
}
