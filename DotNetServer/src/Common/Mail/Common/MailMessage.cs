using System;
using System.Collections.Generic;
using Common.Mail.Smtp;

namespace Common.Mail.Common
{
	/// <summary>
	/// Represent mail message with attachment as MailContent.
	/// </summary>
    public class MailMessage : InternetTextMessage
    {
	    private const Boolean _InvalidFormat = false;

        /// In the body of the decoded string data
        /// <summary>
        /// Field for decoded body data.
        /// In the body of the decoded string data
        /// </summary>
        private String _bodyText;

	    private MailContent _bodyContent ;
        private readonly List<MailContent> _contents = new List<MailContent>();
        private readonly Int64? _index = 0;
        private Int32 _size;
        /// The email messages in the mailbox to get the value of Index.
        /// <summary>
        /// Get mail index of this mailbox.
        /// The email messages in the mailbox to get the value of Index.
        /// </summary>
        public Int64? Index
        {
            get { return _index; }
        }

        /// To get the value of To.
        /// <summary>
        /// Get TO value of this mail.
        /// To get the value of To.
        /// </summary>
        public String To
        {
            get { return this["To"]; }
        }

        /// To get the value of Cc.
        /// <summary>
        /// Get CC value of this mail.
        /// To get the value of Cc.
        /// </summary>
        public String Cc
        {
            get { return this["Cc"]; }
        }

        /// To get the value of Bcc.
        /// <summary>
        /// Get BCC value of this mail.
        /// To get the value of Bcc.
        /// </summary>
        public String Bcc
        {
            get { return this["Bcc"]; }
        }

        /// Some of Body retrieve the text of the message.
        /// <summary>
        /// Get body text message of this mail.
        /// Some of Body retrieve the text of the message.
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

        /// The Header portion of the data is retrieved.
		/// <summary>
		/// Get header text data of this mail.
        /// The Header portion of the data is retrieved.
		/// </summary>
		public new String HeaderData
		{
			get { return base.HeaderData; }
		}

        /// The Body portion of the data is retrieved.
		/// <summary>
		/// Get body text data of this mail.
        /// The Body portion of the data is retrieved.
		/// </summary>
		public new String BodyData
		{
			get { return base.BodyData; }
		}

        /// To get the size of the e-mails.
        /// <summary>
        /// Get mail size of this mail.
        /// To get the size of the e-mails.
        /// </summary>
        public Int32 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// The MailContent Body portion.
        /// <summary>
        /// Get content of this mail message.
        /// The MailContent Body portion.
        /// </summary>
        public MailContent BodyContent
        {
            get
            {
                EnsureBodyContent(_contents);
                return _bodyContent;
            }
        }

        /// Gets the collection of MailContent.
        /// <summary>
        /// Get mail content collection of this mail.
        /// Gets the collection of MailContent.
        /// </summary>
        public List<MailContent> Contents
        {
            get { return _contents; }
        }

        /// Format of the message is the correct value to indicate whether or not you get.
        /// <summary>
        /// Get a value that specify this mail format is valid or invalid.
        /// Format of the message is the correct value to indicate whether or not you get.
        /// </summary>
        public Boolean InvalidFormat
        {
            get { return _InvalidFormat; }
        }

        /// Body portion of the text is generated or the same value to indicate whether or not you get.
	    /// <summary>
	    /// Get value that indicate body text is created or not.
        /// Body portion of the text is generated or the same value to indicate whether or not you get.
	    /// </summary>
	    protected bool BodyTextCreated { get; set; }

	    /// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
        public MailMessage(String text) : 
            base(text)
        {
            Initialize(text);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="index"></param>
        public MailMessage(String text, Int64 index) :
            base(text)
        {
            _index = index;
            Initialize(text);
        }

        private void Initialize(String text)
        {
            Data = text;
            _size = text.Length;
            if (IsMultiPart)
            {
                var list = MimeContent.ParseToContentTextList(BodyData, MultiPartBoundary);
                foreach (var length in list)
                {
                    _contents.Add(new MailContent(this, length));
                }
            }
        }
        /// Some of the data is set Body check that the set, if you do not set the data.
        /// <summary>
        /// Ensure that body data is set or not,and set body data if body data is not set.
        /// Some of the data is set Body check that the set, if you do not set the data.
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        private Boolean EnsureBodyContent(List<MailContent> contents)
        {
            foreach (var content in contents)
            {
                if (content.IsBody)
                {
                    _bodyContent = content;
                    return true;
                }
                if (EnsureBodyContent(content.Contents) )
                { return true; }
            }
            return false;
        }

        /// Get a collection of all of the MailContent.
        /// <summary>
        /// Get all mail content collection.
        /// Get a collection of all of the MailContent.
        /// </summary>
        /// <returns></returns>
        public static List<MailContent> GetAllContents(MailMessage pop3Message)
        {
            if (pop3Message == null)
            { throw new ArgumentNullException("pop3Message"); }
	        var l = GetAttachedContents(pop3Message.Contents, delegate { return true; });
            return l;
        }

        /// The MailContent IsAttachment is true to get the collection of.
        /// <summary>
        /// Get mail content collection that IsAttachment property is true.
        /// The MailContent IsAttachment is true to get the collection of.
        /// </summary>
        /// <returns></returns>
        public static List<MailContent> GetAttachedContents(MailMessage pop3Message)
        {
            if (pop3Message == null)
            { throw new ArgumentNullException("pop3Message"); }
            var l = GetAttachedContents(pop3Message.Contents, c => c.IsAttachment);
            return l;
        }

        /// In predicate given MailContent that meet the criteria you have to get the collection of.
        /// <summary>
        /// Get mail content collection that specify predicate is true.
        /// In predicate given MailContent that meet the criteria you have to get the collection of.
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<MailContent> GetAttachedContents(List<MailContent> contents, Predicate<MailContent> predicate)
        {
            var listMailContent = new List<MailContent>();
            foreach (var t in contents)
            {
                if (predicate(t))
                {
                    listMailContent.Add(t);
                }
                listMailContent.AddRange(GetAttachedContents(t.Contents, predicate).ToArray());
            }
            return listMailContent;
        }

        /// Some of the text Body is set, it is set to check if it is not a string of Body part of the set.
        /// <summary>
        /// Ensure that body text is set or not,and set body text if body text is not set.
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
                else if (IsMultiPart)
                {
                    BodyText = BodyContent == null ? "" : BodyContent.BodyText;
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

        /// The value of an instance of the class to the original SmtpMessage, to generate the instance.
        /// <summary>
        /// Create SmtpMessage instance with this instance value.
        /// The value of an instance of the class to the original SmtpMessage, to generate the instance.
        /// </summary>
        /// <returns></returns>
        public SmtpMessage CreateSmtpMessage()
        {
            var mg = new SmtpMessage();
            Field field;

            mg.To.AddRange(MailAddress.CreateMailAddressList(To));
			mg.Cc.AddRange(MailAddress.CreateMailAddressList(Cc));
            foreach (var headerfield in Header)
            {
                field = headerfield;
                if (String.IsNullOrEmpty(field.Value))
                { continue; }
                if (field.Key.ToLower() == "to" ||
                    field.Key.ToLower() == "cc")
                { continue; }
                mg[field.Key] = MailParser.DecodeFromMailHeaderLine(field.Value);
            }
            for (var i = 0; i < ContentType.Fields.Count; i++)
            {
                field = ContentType.Fields[i];
                mg.ContentType.Fields.Add(new Field(field.Key, MailParser.DecodeFromMailHeaderLine(field.Value)));
            }
            for (var i = 0; i < ContentDisposition.Fields.Count; i++)
            {
                field = ContentDisposition.Fields[i];
                mg.ContentDisposition.Fields.Add(new Field(field.Key, MailParser.DecodeFromMailHeaderLine(field.Value)));
            }
            foreach (var t in Contents)
            {
                mg.Contents.Add(t.CreateSmtpContent());
            }
            return mg;
        }
    }
}
