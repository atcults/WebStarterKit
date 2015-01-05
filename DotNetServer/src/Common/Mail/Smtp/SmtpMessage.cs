using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
#if !NETFX_CORE
#endif
using Common.Mail.Common;

namespace Common.Mail.Smtp
{
    /// Represent smtp message.
    /// <summary>
    /// Represent smtp message.
    /// </summary>
    public class SmtpMessage : InternetTextMessage
    {
        private List<SmtpContent> _contents;
        private readonly List<String> _encodeHeaderKeys = new List<String>();
        private readonly List<MailAddress> _to = new List<MailAddress>();
        private readonly List<MailAddress> _cc = new List<MailAddress>();
        private readonly List<MailAddress> _bcc = new List<MailAddress>();
        private MailPriority _mailPriority = MailPriority.Normal;
        private String _bodyText = "";
        private Encoding _headerEncoding = Encoding.UTF8;
        private TransferEncoding _headerTransferEncoding = TransferEncoding.SevenBit;

        /// Get the source of the email address.
        /// <summary>
        /// Get the source of the email address.
        /// </summary>
        public new MailAddress From { get; set; }

        /// To get a list of email address for the destination.
        /// <summary>
        /// To get a list of email address for the destination.
        /// </summary>
        public List<MailAddress> To
        {
            get { return _to; }
        }

        /// To get a list of the email addresses of CC.
        /// <summary>
        /// To get a list of the email addresses of CC.
        /// </summary>
        public List<MailAddress> Cc
        {
            get { return _cc; }
        }

        /// To get a list of the email addresses of BCC.
        /// <summary>
        /// To get a list of the email addresses of BCC.
        /// </summary>
        public List<MailAddress> Bcc
        {
            get { return _bcc; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MailPriority Priority
        {
            get { return _mailPriority; }
            set { _mailPriority = value; }
        }

        /// The Encoding Header to get or set.
        /// <summary>
        /// The Encoding Header to get or set.
        /// </summary>
        public Encoding HeaderEncoding
        {
            get { return _headerEncoding; }
            set { _headerEncoding = value; }
        }

        /// The TransferEncoding Header to get or set.
        /// <summary>
        /// The TransferEncoding Header to get or set.
        /// </summary>
        public TransferEncoding HeaderTransferEncoding
        {
            get { return _headerTransferEncoding; }
            set { _headerTransferEncoding = value; }
        }

        /// <summary>
        /// The message is signed to indicate whether or not the value of the get or set.
        /// </summary>
        public Boolean IsSigned { get; set; }
        /// <summary>
        /// Whether a message is encrypted gets or sets a value that indicates.
        /// </summary>
        public Boolean IsEncrypted { get; set; }
        /// body portion of the text to get or set the text string.
        /// <summary>
        /// body portion of the text to get or set the text string.
        /// </summary>
        public String BodyText
        {
            get { return _bodyText; }
            set { _bodyText = value; }
        }

        /// HTML format value to indicate whether or not the mail to get or set.
        /// <summary>
        /// HTML format value to indicate whether or not the mail to get or set.
        /// </summary>
        public new Boolean IsHtml
        {
            get { return base.IsHtml; }
            set { ContentType.Value = "text/html"; }
        }

        /// Gets the collection of SmtpContent.
        /// <summary>
        /// Gets the collection of SmtpContent.
        /// </summary>
        public List<SmtpContent> Contents
        {
            get { return _contents; }
        }

		/// <summary>
		/// 
		/// </summary>
        public SmtpMessage()
		{
		    From = null;
		    Initialize();
		}

        /// <summary>
		/// 
		/// </summary>
		/// <param name="mailFrom"></param>
		/// <param name="to"></param>
		/// <param name="cc"></param>
		/// <param name="subject"></param>
		/// <param name="bodyText"></param>
        public SmtpMessage(String mailFrom, String to, String cc, String subject, String bodyText)
        {
            Initialize();
            From = new MailAddress(mailFrom);
            if (String.IsNullOrEmpty(to) == false)
            {
                To.AddRange(MailAddress.CreateMailAddressList(to));
            }
            if (String.IsNullOrEmpty(cc) == false)
            {
                Cc.AddRange(MailAddress.CreateMailAddressList(cc));
            }
            Subject = subject;
            BodyText = bodyText;
        }

        /// Initialization processing.
        /// <summary>
        /// Initialization processing.
        /// </summary>
        private void Initialize()
        {
            _contents = new List<SmtpContent>();

            this["MIME-Version"] = "1.0";
            
            if (CultureInfo.CurrentCulture.Name.StartsWith("ja"))
            {
                HeaderEncoding = Encoding.GetEncoding("iso-2022-jp");
                HeaderTransferEncoding = TransferEncoding.Base64;
                ContentEncoding = Encoding.GetEncoding("iso-2022-jp");
                ContentTransferEncoding = TransferEncoding.Base64;
            }
            _encodeHeaderKeys.Add("subject");
        }

        /// A string that is actually sent to retrieve the data.
        /// <summary>
        /// A string that is actually sent to retrieve the data.
        /// </summary>
        /// <returns></returns>
        public String GetDataText()
        {
            var sb = new StringBuilder(1024);
            Field f;
            String line;

            if (From != null)
            {
                var from = Field.FindField(Header, "From");
                from.Value = From.ToEncodeString().Trim();
            }

            foreach (var t in Header)
            {
                f = t;
                if (String.IsNullOrEmpty(f.Value)) { continue; }
                
                if (_encodeHeaderKeys.Contains(f.Key.ToLower()))
                {
                    sb.AppendFormat("{0}: {1}{2}", f.Key
                                    , MailParser.EncodeToMailHeaderLine(f.Value, HeaderTransferEncoding, HeaderEncoding
                                                                        , MailParser.MaxCharCountPerRow - f.Key.Length - 2), MailParser.NewLine);
                }
                else if(f.Key.ToLower() != "content-type")
                {
                    //Content-Type CreateBodyText method is set.
                    sb.AppendFormat("{0}: {1}{2}", f.Key, f.Value, MailParser.NewLine);
                }
            }
            //Header if it is not set to Set to only
            //Priority
            f = Field.FindField(Header, "X-Priority");
            if (f == null)
            {
                sb.AppendFormat("X-Priority: {0}{1}", ((byte)Priority).ToString(CultureInfo.InvariantCulture), MailParser.NewLine);
            }
            //TO
            f = Field.FindField(Header, "To");
            if (f == null)
            {
                line = CreateMailAddressListText(_to);
                if (String.IsNullOrEmpty(line) == false)
                {
                    sb.Append("To: ");
                    sb.Append(line);
                }
            }
            //CC
            f = Field.FindField(Header, "Cc");
            if (f == null)
            {
                line = CreateMailAddressListText(_cc);
                if (String.IsNullOrEmpty(line) == false)
                {
                    sb.Append("Cc: ");
                    sb.Append(line);
                }
            }
            //BODY
            var bodyText = CreateBodyText();

            if (IsSigned == false && IsEncrypted == false)
            {
                sb.AppendLine(bodyText);
                sb.Append(MailParser.NewLine);
            }
            else
            {
                throw new InvalidOperationException("Library does not support S/MIME in WinRT.");
            }
            sb.Append(MailParser.NewLine);

            return sb.ToString();
        }

        private String CreateBodyText()
        {
            var body = new StringBuilder(256);
            
            if (Contents.Count > 0)
            {
                if (String.IsNullOrEmpty(MultiPartBoundary))
                {
                    MultiPartBoundary = MailParser.GenerateBoundary();
                }
                //Multipartboundary
                body.AppendFormat("Content-Type: multipart/mixed; boundary=\"{0}\"", MultiPartBoundary);
                body.Append(MailParser.NewLine);
                body.AppendFormat("Content-Transfer-Encoding: {0}", ContentTransferEncoding);
                body.Append(MailParser.NewLine);
                body.Append(MailParser.NewLine);
                //This is multi-part message in MIME format.
                body.Append(MailParser.ThisIsMultiPartMessageInMimeFormat);
                body.Append(MailParser.NewLine);
                //Add BodyText Content if IsBody contents does not exist
                if (String.IsNullOrEmpty(BodyText) == false)
                {
                    var ct = new SmtpContent();
                    if (IsHtml)
                    {
                        ct.LoadHtml(BodyText);
                    }
                    else
                    {
                        ct.LoadText(BodyText);
                    }
                    ct.ContentEncoding = ContentEncoding;
                    ct.ContentTransferEncoding = ContentTransferEncoding;

                    body.Append("--");
                    body.Append(MultiPartBoundary);
                    body.Append(MailParser.NewLine);
                    body.Append(ct.Data);
                    body.Append(MailParser.NewLine);
                }

                body.Append(CreateDataText(Contents));
                body.Append(MailParser.NewLine);
                body.AppendFormat("--{0}--", MultiPartBoundary);
                body.Append(MailParser.NewLine);
            }
            else
            {
                body.AppendFormat("Content-Type: {0}; charset=\"{1}\"", ContentType.Value, ContentEncoding.WebName);
                body.Append(MailParser.NewLine);
                body.AppendFormat("Content-Transfer-Encoding: {0}", ContentTransferEncoding);
                body.Append(MailParser.NewLine);
                body.Append(MailParser.NewLine);
                var bodyText = MailParser.EncodeToMailBody(BodyText, ContentTransferEncoding, ContentEncoding);
                if (ContentTransferEncoding == TransferEncoding.SevenBit)
                {
                    body.Append(bodyText);
                }
                else
                {
                    for (var i = 0; i < bodyText.Length; i++)
                    {
                        if (i > 0 && i % 76 == 0)
                        {
                            body.Append(MailParser.NewLine);
                        }
                        //Is current index is first char of line
                        if (i == 0 || (i > 2 && bodyText[i - 2] == '\r' && bodyText[i - 1] == '\n'))
                        {
                            if (bodyText[i] == '.')
                            {
                                body.Append(".");
                            }
                        }
                        body.Append(bodyText[i]);
                    }
                }
                body.Append(MailParser.NewLine);
            }
            return body.ToString();
        }

        private String CreateDataText(IEnumerable<SmtpContent> contents)
        {
            var sb = new StringBuilder(1024);

            foreach (var content in contents)
            {
                //Skip empty SmtpContent instance
                if (content.Contents.Count > 0)
                {
                    sb.Append(CreateDataText(content.Contents));
                }
                else
                {
                    sb.Append("--");
                    sb.Append(MultiPartBoundary);
                    sb.Append(MailParser.NewLine);
                    sb.Append(content.GetDataText());
                    sb.Append(MailParser.NewLine);
                }
            }
            return sb.ToString();
        }

        /// User name and email address to generate a string.
        /// <summary>
        /// User name and email address to generate a string.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="mailAddress"></param>
        public static String CreateFromMailAddress(String userName, String mailAddress)
        {
            return String.Format("\"{0}\" <{1}>", userName, mailAddress);
        }

        /// A list of email addresses from the data to generate a string for the email address.
        /// <summary>
        /// A list of email addresses from the data to generate a string for the email address.
        /// </summary>
        /// <param name="mailAddressList"></param>
        /// <returns></returns>
        private String CreateMailAddressListText(List<MailAddress> mailAddressList)
        {
            var sb = new StringBuilder();
            var l = mailAddressList;
            var s = "";

            foreach (var t in l)
            {
                sb.AppendFormat("{0}{1}", s, t.ToEncodeString().Trim());
                sb.Append(MailParser.NewLine);

                s = "\t, ";
            }
            return sb.ToString();
        }
    }
}
