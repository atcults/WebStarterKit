using System.Collections.Generic;

namespace Common.Mail.Smtp.SendMail
{
    /// <summary>
    /// 
    /// </summary>
    public class SendMailListResult
    {
        private readonly List<SendMailResult> _results = new List<SendMailResult>();
        /// <summary>
        /// 
        /// </summary>
        public SendMailResultState State { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SendMailResult> Results
        {
            get { return _results; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public SendMailListResult(SendMailResultState state)
        {
            State = state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="results"></param>
        public SendMailListResult(SendMailResultState state, IEnumerable<SendMailResult> results)
        {
            State = state;
            Results.AddRange(results);
        }
    }
}
