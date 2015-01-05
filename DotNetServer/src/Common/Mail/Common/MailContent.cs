using System;
using System.Collections.Generic;
using Common.Mail.Smtp;

namespace Common.Mail.Common
{
    /// Represent mail content.
    /// <summary>
    /// Represent mail content.
    /// </summary>
    public class MailContent : MimeContent
    {
        private MailMessage _message;

        /// In the body of the decoded string data

        /// <summary>
        /// Field for decoded body data.
        /// In the body of the decoded string data

        /// </summary>
        private String _bodyText;

        private List<MailContent> _Contents = new List<MailContent>();

        /// To get or set the parent Content.
        /// <summary>
        /// Get or set parent content object.
        /// To get or set the parent Content.
        /// </summary>
        public MailContent ParentContent { get; private set; }

        /// To get the value of Name.

        /// <summary>
        /// Get name value.
        /// To get the value of Name.

        /// </summary>
        public String Name
        {
            get { return ContentType.Name; }
        }

        /// To get the value of FileName.
        /// <summary>
        /// Get filename value.
        /// To get the value of FileName.
        /// </summary>
        public String FileName
        {
            get { return ContentDisposition.FileName; }
        }

        /// This instance is to represent the data of the body part, if returns True.
        /// <summary>
        /// This instance is to represent the data of the body part, if returns True.
        /// </summary>
        public Boolean IsBody
        {
            get
            {
                return (ContentType.Value.StartsWith("text/", StringComparison.OrdinalIgnoreCase));
            }
        }

        /// Some of Body to retrieve the string.
        /// <summary>
        /// Get body text of this mail.
        /// Some of Body to retrieve the string.
        /// </summary>
        public String BodyText
        {
            get
            {
                EnsureBodyText();
                return _bodyText;
            }
            set { _bodyText = value; }
        }

        /// Gets the collection of MailContent.
        /// <summary>
        /// Get mail content collection of this mail.
        /// Gets the collection of MailContent.

        /// </summary>
        public new List<MailContent> Contents
        {
            get { return _Contents; }
        }

        /// Part of the text has been previously generated Body value to indicate whether or not you get.
        /// <summary>
        /// Get value that indicate body text is created or not.
        /// Part of the text has been previously generated Body value to indicate whether or not you get.
        /// </summary>
        protected bool BodyTextCreated { get; set; }

        /// <summary>
		/// Define text message of mail
		/// </summary>
		/// <param name="message"></param>
		/// <param name="text"></param>
        public MailContent(MailMessage message, String text) : 
            base(text)
        {
            Initialize(message, text);
        }

        /// Initialization processing.
        /// <summary>
        /// Initialization processing.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="text"></param>
        private void Initialize(MailMessage message, String text)
        {
            _message = message;
            _Contents = new List<MailContent>();
            Data = text;
            _bodyText = "";
            if (IsMultiPart)
            {
                var l=  ParseToContentTextList(BodyData,MultiPartBoundary);
                foreach (var t in l)
                {
                    var ct = new MailContent(_message, t) {ParentContent = this};
                    _Contents.Add(ct);
                }
            }
        }

        /// Some of the text Body is set, it is set to check if it is not a string of Body part of the set.
        /// <summary>
        /// Some of the text Body is set, it is set to check if it is not a string of Body part of the set.
        /// </summary>
        /// <returns></returns>
        protected virtual void EnsureBodyText()
        {
            if (BodyTextCreated == false)
            {
                if (ContentType.Value.IndexOf("message/rfc822", StringComparison.Ordinal) > -1)
                {
                    BodyText = BodyData;
                }
                else if (IsText)
                {
                    BodyText = MailParser.DecodeFromMailBody(BodyData, ContentTransferEncoding, ContentEncoding);
                }
                else
                {
                    BodyText = BodyData;
                }
            }
            BodyTextCreated = true;
        }

        /// The value of an instance of the class to the original SmtpContent, to generate the instance.
        /// <summary>
        /// Create SmtpContent instance with this instance value.
        /// The value of an instance of the class to the original SmtpContent, to generate the instance.
        /// </summary>
        /// <returns></returns>
        public SmtpContent CreateSmtpContent()
        {
            var content = new SmtpContent();
            Field field;

            foreach (var count in Header)
            {
                field = count;
                if (String.IsNullOrEmpty(field.Value))
                { continue; }
                content[field.Key] = MailParser.DecodeFromMailHeaderLine(field.Value);
            }
            for (var i = 0; i < ContentType.Fields.Count; i++)
            {
                field = ContentType.Fields[i];
                content.ContentType.Fields.Add(new Field(field.Key, MailParser.DecodeFromMailHeaderLine(field.Value)));
            }
            for (var i = 0; i < ContentDisposition.Fields.Count; i++)
            {
                field = ContentDisposition.Fields[i];
                content.ContentDisposition.Fields.Add(new Field(field.Key, MailParser.DecodeFromMailHeaderLine(field.Value)));
            }
            content.LoadBodyData(BodyData);
            foreach (var t in Contents)
            {
                content.Contents.Add(t.CreateSmtpContent());
            }
            return content;
        }
    }
}
