using System;
using System.Text;
using Common.Mail.Async;
using Common.Net.Extensions;

namespace Common.Mail.Imap.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class ImapIdleCommand : ImapDataReceiveContext
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ImapIdleCommandMessageReceivedEventArgs> MessageReceived;
        /// <summary>
        /// 
        /// </summary>
        public IAsyncResult AsyncResult { get; set; }
        internal ImapIdleCommand(String tag, Encoding encoding)
            : base(tag, encoding)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnMessageReceived(ImapIdleCommandMessageReceivedEventArgs e)
        {
            var eh = MessageReceived;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        protected override bool ParseBuffer(int size)
        {
            var position = Stream.Position;
            var bl = base.ParseBuffer(size);
            Stream.Position = position;

            var text = Encoding.GetString(Stream.ToByteArray());
            var e = new ImapIdleCommandMessageReceivedEventArgs(text);
            OnMessageReceived(e);
            return e.Done == false;
        }
    }
}
