using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Mail.Common
{
    /// Represent message defined RFC822,RFC2045-2049 as class.
    /// <summary>
    /// Represent message defined RFC822,RFC2045-2049 as class.
    /// </summary>
	public class InternetTextMessage 
	{
        private struct RegexList
        {
            public static readonly ICollection<Regex> ContentEncodingCharset = new List<Regex>();
            public static readonly Regex HeaderParse = new Regex("^(?<key>[^:]*):[\\s]*(?<value>.*)");
            public static readonly Regex HeaderParse1 = new Regex("(?<value>[^;]*)[;]*");
            public static readonly Regex Attachment = new Regex("^attachment.*$");
            public static readonly Regex Image = new Regex("^image/.*$");
        }

        
        private List<Field> _header;
        /// Header string data (US-ASCII).

		/// <summary>
		/// Field for header data (encoded by US-ASCII)
        /// Header string data (US-ASCII)
		/// </summary>
		private String _headerData = "";
        /// Body Text Data (US-ASCII). 
        /// <summary>
        /// Field for body data (encoded by US-ASCII)
        /// Body Text Data (US-ASCII).
        /// </summary>
        private String _bodyData = "";

        private Boolean _decodeHeaderText = true;
        private Encoding _contentEncoding = Encoding.UTF8;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public String this[String key]
		{
			get 
			{
				var f = Field.FindField(_header, key);
				if (f == null)
				{
					return "";
				}
			    if (_decodeHeaderText)
			    {
			        return MailParser.DecodeFromMailHeaderLine(f.Value);
			    }
			    return f.Value;
			}
			set 
			{
				var f = Field.FindField(_header, key);
				if (f == null)
				{
					f = new Field(key, value);
					_header.Add(f);
				}
				else
				{
					f.Value = value;
				}
			}
		}

        /// Gets or sets the From.

        /// <summary>
        /// Get from value.
        /// Gets or sets the From.
        /// </summary>
        public String From
        {
            get { return this["From"]; }
            set { this["From"] = value; }
        }

        /// Reply - to get or set the To. 
        /// <summary>
        /// Reply - to get or set the To. 
        /// </summary>
        public String ReplyTo
        {
            get { return this["Reply-To"]; }
            set { this["Reply-To"] = value; }
        }

        /// In-Reply - to get or set the To. 
        /// <summary>
        /// In-Reply - to get or set the To. 
        /// </summary>
        public String InReplyTo
        {
            get { return this["In-Reply-To"]; }
            set { this["In-Reply-To"] = value; }
        }

        /// Gets or sets the subject. 
        /// <summary>
        /// Gets or sets the subject. 
        /// </summary>
        public String Subject
        {
            get { return this["Subject"]; }
            set { this["Subject"] = value; }
        }

        /// Gets or sets the Date.
        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        public DateTimeOffset Date
        {
            get { return MailParser.ToDateTimeOffset(this["Date"]); }
            set { this["Date"] = MailParser.DateTimeOffsetString(value); }
        }

        /// Gets or sets the MessageID. 
        /// <summary>
        /// Gets or sets the MessageID. 
        /// </summary>
        public String MessageId
        {
            get { return this["Message-ID"]; }
            set { this["Message-ID"] = value; }
        }

        /// Gets or sets the References.
        /// <summary>
        /// Gets or sets the References.
        /// </summary>
        public String References
        {
            get { return this["References"]; }
            set { this["References"] = value; }
        }

        /// To obtain the ContentType. 
        /// <summary>
        /// To obtain the ContentType. 
        /// </summary>
        public ContentType ContentType
        {
            get 
            {
                var contentType = Field.FindField(_header, "content-type") as ContentType;
                if (contentType == null)
                {
                    contentType = new ContentType("text/plain");
                    _header.Add(contentType);
                }
                return contentType;
            }
        }

        /// To obtain the Encoding. 
        /// <summary>
        /// To obtain the Encoding. 
        /// </summary>
        public Encoding ContentEncoding
        {
            get { return _contentEncoding; }
            set { _contentEncoding = value; }
        }

        ///  You get ContentDisposition.
        /// <summary>
        ///  You get ContentDisposition.

        /// </summary>
        public ContentDisposition ContentDisposition
        {
            get
            {
                var ff = Field.FindField(_header, "content-disposition") as ContentDisposition;
                if (ff == null)
                {
                    ff = new ContentDisposition("");
                    _header.Add(ff);
                }
                return ff;
            }
        }

        /// The MultiPartBoundary gets or sets the string.
        /// <summary>
        /// The MultiPartBoundary gets or sets the string.
        /// </summary>
        public String MultiPartBoundary
        {
            get { return ContentType.Boundary; }
            set { ContentType.Boundary = value; }
        }

        /// In some Body MIME is configured to retrieve value to indicate whether or not
        /// <summary>
        /// In some Body MIME is configured to retrieve value to indicate whether or not
        /// </summary>
        public Boolean IsMultiPart
        {
            get { return Regex.IsMatch(ContentType.Value, ".*multipart/.*", RegexOptions.IgnoreCase); }
        }

        /// This instance is to represent data in text format, returns True.
        /// <summary>
        /// This instance is to represent data in text format, returns True.
        /// </summary>
        public Boolean IsText
        {
            get
            {
                return (ContentType.Value.StartsWith("text/", StringComparison.OrdinalIgnoreCase)) ||
                    (ContentType.Value.Equals("application/xml", StringComparison.OrdinalIgnoreCase)) ||
                    (ContentType.Value.Equals("application/json", StringComparison.OrdinalIgnoreCase));
            }
        }

        /// This instance is HTML format that represents the data of the text if the returns True. 
        /// <summary>
        /// This instance is HTML format that represents the data of the text if the returns True. 
        /// </summary>
        public Boolean IsHtml
        {
            get
            {
                return (ContentType.Value.StartsWith("text/html", StringComparison.OrdinalIgnoreCase));
            }
        }

        /// This instance is the True attachment data. 
        /// <summary>
        /// This instance is the True attachment data. 
        /// </summary>
        public Boolean IsAttachment
        {
            get
            {
                if (String.IsNullOrEmpty(ContentDisposition.Value) == false)
                {
                    return RegexList.Attachment.Match(ContentDisposition.Value).Success ||
                        RegexList.Image.Match(ContentType.Value).Success;
                }
                return false;
            }
        }

        /// Gets or sets the ContentDisposition. 
        /// <summary>
        /// Gets or sets the ContentDisposition. 
        /// </summary>
        public String ContentDescription
        {
            get { return this["Content-Description"]; }
            set { this["Content-Description"] = value; }
        }

        /// Get or set the value of ContentTransferEncoding.
        /// <summary>
        /// Get or set the value of ContentTransferEncoding.
        /// </summary>
        public TransferEncoding ContentTransferEncoding
        {
            get { return MailParser.ToTransferEncoding(this["Content-Transfer-Encoding"]); }
            set { this["Content-Transfer-Encoding"] = MailParser.ToTransferEncoding(value); }
        }

        /// To get the value of CharSet. 
        /// <summary>
        /// To get the value of CharSet. 
        /// </summary>
        public String CharSet
        {
            get { return ContentEncoding.HeaderName; }
        }

        /// Get a collection of the header. 
        /// <summary>
        /// Get a collection of the header. 
        /// </summary>
        public List<Field> Header
        {
            get { return _header; }
        }

        /// The portion of the header field value value to indicate whether or not you want to decode. 
        /// <summary>
        /// The portion of the header field value value to indicate whether or not you want to decode. 
        /// </summary>
        public Boolean DecodeHeaderText
        {
            get { return _decodeHeaderText; }
            set { _decodeHeaderText = value; }
        }

        /// The Header portion of the data is retrieved.
		/// <summary>
        /// The Header portion of the data is retrieved.
		/// </summary>
		protected String HeaderData
		{
			get { return _headerData; }
			set { _headerData = value; }
		}

        /// The Body portion of the data is retrieved.
        /// <summary>
        /// The Body portion of the data is retrieved.
        /// </summary>
        protected String BodyData
        {
            get { return _bodyData; }
            set { _bodyData = value; }
        }

        /// This instance is used to generate a string was to retrieve the data
        /// <summary>
        /// Get text data used to create this instance.
        /// This instance is used to generate a string was to retrieve the data
        /// </summary>
        public string Data { get; protected set; }

        static InternetTextMessage()
		{
			Initialize();
		}

		private static void Initialize()
		{
			RegexList.ContentEncodingCharset.Add(new Regex(".*charset ?= ?[\"]*(?<Value>[^\";]*)[;\n\r]", RegexOptions.IgnoreCase));
			RegexList.ContentEncodingCharset.Add(new Regex(".*charset ?= ?[\"]*(?<Value>[^\"]*).*", RegexOptions.IgnoreCase));
		}

		/// <summary>
		/// 
		/// </summary>
        public InternetTextMessage()
        {
            Initialize("");
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
        public InternetTextMessage(String text)
        {
            Initialize(text);
        }

        /// Initialization processing.
        /// <summary>
        /// Initialization processing.
        /// </summary>
        /// <param name="text"></param>
        private void Initialize(String text)
		{
			_header = new List<Field>();
            
            Date = DateTime.Now;
            _header.Add(new Field("From", ""));
            _header.Add(new Field("Subject", ""));
            ContentType.Value = "text/plain";
            ContentTransferEncoding = TransferEncoding.SevenBit;
            ContentDisposition.Value = "inline";
            SetDefaultContentEncoding();

            Parse(text);
        }

        /// The default Content-Encoding to set the value of.
        /// <summary>
        /// The default Content-Encoding to set the value of.
        /// </summary>
        private void SetDefaultContentEncoding()
        {
            if (System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja")
            {
                ContentEncoding = Encoding.GetEncoding("iso-2022-jp");
            }
        }

        /// To analyze the text. 
        /// <summary>
        /// To analyze the text. 
        /// </summary>
        /// <param name="text"></param>
        protected void Parse(String text)
        {
            StringReader sr;
            //In the case of multi-line fields for the second and subsequent lines of the line list
            var stringlist = new List<String>();
            var firstLine = "";
            var isConcating = false;
            var sb = new StringBuilder(512);

            using (sr = new StringReader(text))
            {
                while (true)
                {
                    var currentLine = sr.ReadLine();
					sb.Append(currentLine);
					sb.Append(MailParser.NewLine);
					if (isConcating)
                    {
                        stringlist.Add(currentLine);
                    }
                    else
                    {
                        stringlist.Clear();
                        firstLine = currentLine;
                        //Some of the header and body to go separate check
                        if (firstLine == "")
                        {
							_headerData = sb.ToString();
                            //Since some of the data in the Data Body
							sb = new StringBuilder(text.Length - _headerData.Length);
                            while (true)
                            {
                                currentLine = sr.ReadLine();
                                if (currentLine == null) { break; }
                                if (currentLine == ".") { break; }
                                if (currentLine.StartsWith(".."))
                                { currentLine = currentLine.Substring(1, currentLine.Length - 1); }
                                sb.Append(currentLine);
                                if (sr.Peek() == -1) { break; }
                                //Not append new line char after last line
                                //In the last line you do not add a line feed code. 
                                sb.Append(MailParser.NewLine);
                            }
                            BodyData = sb.ToString();
                            return;
                        }
                    }
                    //The first character of the next line to get
                    var c = sr.Peek();
                    //If you don't have the line: End
					if (c == -1)
					{
						_headerData = sb.ToString();
						break;
					}
                    //The first character of the line: a tab character or space, as more than one field in a row to be concatenated
                    if (c == 9 || c == 32)
                    {
                        isConcating = true;
                        continue;
                    }
                    ParseHeaderField(firstLine, stringlist);
                    stringlist.Clear();
                    isConcating = false;
                }
            }
        }

        /// Line, the string to parse the Generated Instances of the field.
        /// <summary>
        /// Line, the string to parse the Generated Instances of the field.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        private void ParseHeaderField(String line, List<String> lines)
        {
            var m = RegexList.HeaderParse.Match(line);
            var rx = RegexList.HeaderParse1;
            var l = lines;
			var size = 0;
			for (var i = 0; i < lines.Count; i++)
			{
				size += line.Length;
			}
            var sb = new StringBuilder(size);

            if (String.IsNullOrEmpty(m.Groups["key"].Value) == false)
            {
                var m1 = rx.Match(m.Groups["value"].Value);
                if (m.Groups["key"].Value.ToLower() == "content-type" ||
                    m.Groups["key"].Value.ToLower() == "content-disposition")
                {
                    sb.Append(line);
                    foreach (var t in l)
                    {
                        sb.Append(t.TrimStart('\t'));
                    }
                    ParseContentEncoding(sb.ToString());

                    if (m.Groups["key"].Value.ToLower() == "content-type")
                    {
                        MailParser.ParseContentType(ContentType, sb.ToString());
                        ContentType.Value = m1.Groups["value"].Value;
                    }
                    else if (m.Groups["key"].Value.ToLower() == "content-disposition")
                    {
                        MailParser.ParseContentDisposition(ContentDisposition, sb.ToString());
                        ContentDisposition.Value = m1.Groups["value"].Value;
                    }
                }
                else
                {
                    var f = Field.FindField(_header, m.Groups["key"].Value);
                    if (f == null)
                    {
                        f = new Field(m.Groups["key"].Value, m.Groups["value"].Value);
                        Header.Add(f);
                    }
                    else
                    {
                        f.Value = m.Groups["value"].Value;
                    }
                    foreach (var t in l)
                    {
                        f.Value += t.TrimStart('\t');
                    }
                }
            }
        }

        /// Content-Encoding's analysis.
        /// <summary>
        /// Content-Encoding's analysis.
        /// </summary>
        /// <param name="line"></param>
        private void ParseContentEncoding(String line)
        {
            //charset=???;
            foreach (var rx in RegexList.ContentEncodingCharset)
            {
                var m = rx.Match(line);
                if (String.IsNullOrEmpty(m.Groups["Value"].Value) == false)
                {
                    _contentEncoding = MailParser.GetEncoding(m.Groups["Value"].Value, ContentEncoding);
                    break;
                }
            }
        }

        /// To decode the binary data to a physical path that you specify.
        /// <summary>
        /// Decode binary data and output as file to specify file path.
        /// To decode the binary data to a physical path that you specify.
        /// </summary>
        /// <param name="filePath"></param>
        public void DecodeData(String filePath)
        {
            using (var filestm = new FileStream(filePath, FileMode.Create))
            {
                DecodeData(filestm, true);
            }
        }

        /// To decode the binary data to a specified stream output.
        /// <summary>
        /// Decode binary data and output to specify stream.
        /// To decode the binary data to a specified stream output.
        /// </summary>
        /// <param name="stream">The stream to which you want to write.</param>
        /// <param name="isClose">After writing to the stream stream to Value to indicate whether or not to close the set.</param>
        public void DecodeData(Stream stream, Boolean isClose)
        {
            Byte[] bb = null;
            BinaryWriter binaryWriter = null;

            if (String.IsNullOrEmpty(ContentDisposition.Value))
            { return; }

            if (ContentTransferEncoding == TransferEncoding.Base64)
            {
                bb = Convert.FromBase64String(BodyData.Replace("\n", "").Replace("\r", ""));
            }
            else if (ContentTransferEncoding == TransferEncoding.QuotedPrintable)
            {
                bb = MailParser.FromQuotedPrintableText(BodyData);
            }
            else if (ContentTransferEncoding == TransferEncoding.SevenBit)
            {
                bb = Encoding.UTF8.GetBytes(BodyData);
            }
            try
            {
                binaryWriter = new BinaryWriter(stream);
                if (bb != null) binaryWriter.Write(bb);
                binaryWriter.Flush();
            }
            finally
            {
                if (isClose )
                {
                    if (binaryWriter != null) binaryWriter.Dispose();
                }
                if (binaryWriter != null) binaryWriter.Close();
            }
        }

        /// KEY and VALUE which consists of a set of classes that represent field.
        /// <summary>
        /// KEY and VALUE which consists of a set of classes that represent field.

        /// RFC 822 defined.
        /// </summary>
        public class Field
        {
            private String _key = "";
            private String _value = "";
			/// <summary>
			/// 
			/// </summary>
            public String Key
            {
                get { return _key ?? ""; }
                set { _key = value; }
            }
			/// <summary>
			/// 
			/// </summary>
            public String Value
            {
                get { return _value ?? ""; }
                set { _value = value; }
            }
			/// <summary>
			/// 
			/// </summary>
			/// <param name="key"></param>
			/// <param name="value"></param>
            public Field(String key, String value)
            {
                _key = key;
                _value = value;
            }
			/// <summary>
			/// 
			/// </summary>
			/// <param name="fields"></param>
			/// <param name="key"></param>
			/// <returns></returns>
            public static Field FindField(List<Field> fields, String key)
            {
                var list = fields.FindAll(f => String.Equals(f.Key, key, StringComparison.OrdinalIgnoreCase));
                if (list.Count > 0)
                {
                    return list[list.Count - 1];
                }
                return null;
            }
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
            public override string ToString()
            {
                return String.Format("{0}: {1}", Key,Value);
            }
        }
	}
}
