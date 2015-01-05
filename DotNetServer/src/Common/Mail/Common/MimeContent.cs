using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Mail.Common
{
    /// Represent Mime-Content as class.
    /// <summary>
    /// Represent Mime-Content as class.
    /// </summary>
    public class MimeContent : InternetTextMessage
    {
        private List<MimeContent> _contents;
        /// The Header portion of the data is retrieved.
        /// <summary>
        /// Get header text data of this mail.
        /// The Header portion of the data is retrieved.
        /// </summary>
        public new String HeaderData
        {
            get { return base.HeaderData; }
            protected set { base.HeaderData = value; }
        }

        /// The Body portion of the data is retrieved.
        /// <summary>
        /// Get body text data of this mail.
        /// The Body portion of the data is retrieved.
        /// </summary>
        public new String BodyData
        {
            get { return base.BodyData; }
            protected set { base.BodyData = value; }
        }

        /// Gets the collection of MimeContent.
        /// <summary>
        /// Get mime content collection.
        /// Gets the collection of MimeContent.
        /// </summary>
        public List<MimeContent> Contents
        {
            get { return _contents; }
        }

		/// <summary>
		/// Get mime content
		/// </summary>
        public MimeContent()
		{
            Initialize();
        }

		/// <summary>
		/// Get mime content text
		/// </summary>
		/// <param name="text"></param>
        public MimeContent(String text):
            base(text)
        {
            Initialize();
        }

        private void Initialize()
        {
            _contents = new List<MimeContent>();
            if (IsMultiPart)
            {
                var l = ParseToContentTextList(BodyData, MultiPartBoundary);
                foreach (var t in l)
                {
                    _contents.Add(new MimeContent(t));
                }
            }
        }

        /// Some of Body parsing the text of each of the split in the MIME portion of the text.
        /// <summary>
        /// Parse body text and separate as text foe each mime content.
        /// Some of Body parsing the text of each of the split in the MIME portion of the text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="multiPartBoundary"></param>
        public static List<String> ParseToContentTextList(String text, String multiPartBoundary) 
        {
            StringReader sr;
            var sb = new StringBuilder();
            var startOfBoundary = "--" + multiPartBoundary;
            var endOfBoundary = "--" + multiPartBoundary + "--";
            var list = new List<string>();
            var isBegin = false;

            using (sr = new StringReader(text))
            {
                while (true)
                {
                    var currentLine = sr.ReadLine();
                    if (currentLine == null)
                    { break; }
                    if (isBegin == false)
                    {
                        if (currentLine == startOfBoundary)
                        {
                            isBegin = true;
                            sb.Length = 0;
                        }
                        continue;
                    }
                    if (currentLine == startOfBoundary ||
                        currentLine == endOfBoundary)
                    {
                        if (sb.Length > 0)
                        {
                            list.Add(sb.ToString());
                        }
                        sb.Length = 0;
                        if (currentLine == endOfBoundary)
                        { break; }
                    }
                    else
                    {
                        sb.Append(currentLine);
                        sb.Append(MailParser.NewLine);
                    }
                    if (sr.Peek() == -1)
                    {
                        list.Add(sb.ToString());
                        break; 
                    }
                }
            }
            return list;
        }
    }
}
