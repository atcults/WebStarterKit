using System;
using System.Text;
using Common.Net.Core;
using Common.Net.SocketClient;

namespace Common.Mail.Async
{
	/// <summary>
    /// Represent context of request and response process and provide data about context.
    /// </summary>
    internal class SmtpDataReceiveContext : DataReceiveContext
    {
        private enum ParseState
        {
            ResponseCode, HasNextLine, Message, CarriageReturn, LastLineMessage, LastLineCarriageReturn,
        }

        private ParseState _state = ParseState.ResponseCode;

        /// <summary>
        /// Context for data receive from the smtp.
        /// </summary>
        /// <param name="encoding"></param>
		internal SmtpDataReceiveContext(Encoding encoding):
			base(encoding)
		{
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
            var responseCodeIndex = 0;

            for (var i = 0; i < size; i++)
            {
                Stream.WriteByte(bb[i]);
                if (_state == ParseState.ResponseCode)
                {
                    responseCodeIndex += 1;
                    if (responseCodeIndex == 3)
                    {
                        _state = ParseState.HasNextLine;
                    }
                }
                else if (_state == ParseState.HasNextLine)
                {
                    if (bb[i] == AsciiCharCode.Space.GetNumber())
                    {
                        _state = ParseState.LastLineMessage;
                    }
                    else if (bb[i] == AsciiCharCode.Minus.GetNumber())
                    {
                        _state = ParseState.Message;
                    }
                    else if (bb[i] == AsciiCharCode.CarriageReturn.GetNumber())
                    {
                        _state = ParseState.LastLineCarriageReturn;
                    }
                    else { throw new DataTransferContextException(this); }
                }
                else if (_state == ParseState.Message)
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
                        responseCodeIndex = 0;
                        _state = ParseState.ResponseCode;
                    }
                    else { throw new DataTransferContextException(this); }
                }
                else if (_state == ParseState.LastLineMessage)
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
