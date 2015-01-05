using System;
using System.Text;
using Common.Net.Core;
using Common.Net.SocketClient;

namespace Common.Mail.Async
{
    /// <summary>
    /// Represent context of request and response process and provide data about context.
    /// </summary>
    internal class Pop3DataReceiveContext : DataReceiveContext
    {
        private enum ParseState
        {
            StartCharOfLine, Message, CarriageReturn, Period, LastLineCarriageReturn,
        }
        private ParseState _state = ParseState.StartCharOfLine;
        private readonly Boolean _isMultiLine;
        internal Pop3DataReceiveContext(Encoding encoding, Boolean isMultiLine) :
			base(encoding)
		{
			_isMultiLine = isMultiLine;
		}

        internal Pop3DataReceiveContext(Encoding encoding, Boolean isMultiLine, Action<String> callbackFunction) :
			base(encoding)
		{
			_isMultiLine = isMultiLine;
			EndGetResponse = callbackFunction;
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

			for (var i = 0; i < size; i++)
			{
				Stream.WriteByte(bb[i]);
                if (_state == ParseState.StartCharOfLine)
                {
                    if (_isMultiLine)
                    {
                        if (bb[i] == AsciiCharCode.Period.GetNumber())
                        {
                            _state = ParseState.Period;
                        }
                        else if (bb[i] == AsciiCharCode.CarriageReturn.GetNumber())
                        {
                            _state = ParseState.CarriageReturn;
                        }
                        else
                        {
                            _state = ParseState.Message;
                        }
                    }
                    else
                    {
                        _state = ParseState.Message;
                    }
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
                        if (_isMultiLine)
                        {
                            _state = ParseState.StartCharOfLine;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else { throw new DataTransferContextException(this); }
                }
                else if (_state == ParseState.Period)
                {
                    if (bb[i] == AsciiCharCode.CarriageReturn.GetNumber())
                    {
                        _state = ParseState.LastLineCarriageReturn;
                    }
                    else if (bb[i] == AsciiCharCode.Period.GetNumber())
                    {
                        _state = ParseState.Message;
                    }
                    else { throw new DataTransferContextException(this); }
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
