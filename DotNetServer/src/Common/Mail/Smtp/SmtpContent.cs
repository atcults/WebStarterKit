using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Mail.Common;

namespace Common.Mail.Smtp
{
    /// Represent smtp content.
    /// <summary>
    /// Represent smtp content.
    /// </summary>
    public class SmtpContent : MimeContent
    {
        private static readonly List<String> ExceptHeaderKeyList = new List<string>();
        private readonly List<String> _encodeHeaderKeys = new List<String>();
        private FieldParameterEncoding _fieldParameterEncoding = FieldParameterEncoding.Rfc2047;
        private readonly List<SmtpContent> _Contents;
        private static readonly Dictionary<String, String> FileExtensionContentType = new Dictionary<String, String>();
        private String _bodyText = "";
        /// The Header Field Parameter of Encoding of you to get or set.
        /// <summary>
        /// The Header Field Parameter of to get or set the Encoding.
        /// </summary>
        public FieldParameterEncoding FieldParameterEncoding
        {
            get { return _fieldParameterEncoding; }
            set { _fieldParameterEncoding = value; }
        }

        /// Get or set the value of Name.
        /// <summary>
        /// Get or set the value of Name.
        /// </summary>
        public String Name
        {
            get { return ContentType.Name; }
            set { ContentType.Name = value; }
        }

        /// Get or set the value of FileName.
        /// <summary>
        /// Get or set the value of FileName.
        /// </summary>
        public String FileName
        {
            get { return ContentDisposition.FileName; }
            set { ContentDisposition.FileName = value; }
        }

        /// body portion of the text to get or set the text string.
        /// <summary>
        /// body portion of the text to get or set the text string.
        /// </summary>
        public String BodyText
        {
            get { return _bodyText; }
            private set { _bodyText = value; }
        }

        /// Gets the collection of SmtpContent.
        /// <summary>
        /// Gets the collection of SmtpContent.
        /// </summary>
        public new List<SmtpContent> Contents
        {
            get { return _Contents; }
        }

        static SmtpContent()
        {
            InitializeExceptHeaderKeyList();
            InitializeFileExtenstionContentType();
        }

		/// <summary>
		/// 
		/// </summary>
		public SmtpContent()
		{
            ContentTransferEncoding = TransferEncoding.Base64;
            _Contents = new List<SmtpContent>();
            _encodeHeaderKeys.Add("subject");
        }

        private static void InitializeExceptHeaderKeyList()
        {
            var l = ExceptHeaderKeyList;
            l.Add("Content-Type");
            l.Add("Content-Disposition");
            l.Add("Date");
        }

        private static void InitializeFileExtenstionContentType()
        {
            var l = FileExtensionContentType;
            l.Add("txt", "text/plain");
            l.Add("css", "text/css");
            l.Add("htm", "text/html");
            l.Add("html", "text/html");
            l.Add("jpg", "Image/jpeg");
            l.Add("gif", "Image/gif");
            l.Add("bmp", "image/x-ms-bmp");
            l.Add("png", "Image/png");
            l.Add("wav", "Audio/wav");
            l.Add("doc", "application/msword");
            l.Add("mdb", "application/msaccess");
            l.Add("xls", "application/vnd.ms-excel");
            l.Add("ppt", "application/vnd.ms-powerpoint");
            l.Add("mpeg", "video/mpeg");
            l.Add("mpg", "video/mpeg");
            l.Add("avi", "video/x-msvideo");
            l.Add("zip", "application/zip");
        }

        /// On to the original extension Content-Type can retrieve the string.
        /// <summary>
        /// On to the original extension Content-Type can retrieve the string.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static String GetContentType(String extension)
        {
            var s = extension.Replace(".", "").ToLower();
            if (FileExtensionContentType.ContainsKey(s.ToLower()))
            {
                return FileExtensionContentType[s.ToLower()];
            }
            return "application/octet-stream";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void LoadBodyData(String data)
        {
            SetHeaderData();
            BodyData = data;
            Data = HeaderData + MailParser.NewLine + BodyData;
        }

        /// The specified range of text data.
        /// <summary>
        /// The specified range of text data.
        /// </summary>
        /// <param name="text"></param>
        public void LoadText(String text)
        {
            ContentType.Value = "text/plain";
            BodyText = text;
            SetData();
        }

        /// HTML format to set the text.
        /// <summary>
        /// HTML format to set the text.
        /// </summary>
        /// <param name="html"></param>
        public void LoadHtml(String html)
        {
            ContentType.Value = "text/html";
            BodyText = html;
            SetData();
        }

        /// Specified file path of the file data to the original data set.
        /// <summary>
        /// Specified file path of the file data to the original data set.
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadFileData(String filePath)
        {
            var fi = new FileInfo(filePath);

            var extension = Path.GetExtension(filePath);
            if (extension != null)
                ContentType.Value = GetContentType(extension.Replace(".", ""));
            ContentType.Name = fi.Name;
            ContentDisposition.FileName = fi.Name;
            ContentDisposition.Value = "attachment";
            ContentTransferEncoding = TransferEncoding.Base64;
            var bb = File.ReadAllBytes(filePath);
            BodyText = Convert.ToBase64String(bb);
            SetData();
        }

        /// <summary>
        /// data bytes to the original data set.
        /// </summary>
        /// <param name="bytes"></param>
        public void LoadData(Byte[] bytes)
        {
            LoadData("application/octet-stream", bytes);
        }

        /// <summary>
        /// data bytes to the original data set.
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="bytes"></param>
        public void LoadData(String contentType, Byte[] bytes)
        {
            ContentType.Value = contentType;
            ContentDisposition.Value = "attachment";
            ContentTransferEncoding = TransferEncoding.Base64;
            BodyText = Convert.ToBase64String(bytes);
            SetData();
        }

        /// <summary>
        /// When you send an email message to retrieve the text of the actual life.
        /// </summary>
        /// <returns></returns>
        public String GetDataText()
        {
            SetData();
            return Data;
        }

        private void SetData()
        {
            SetHeaderData();
            SetBodyData();
            Data = HeaderData + MailParser.NewLine + BodyData;
        }

        private void SetHeaderData()
        {
            var sb = new StringBuilder(1024);

            if (IsMultiPart == false &&
                Contents.Count > 0)
            {
                ContentType.Value = "multipart/mixed";
            }
            if (IsText)
            {
                sb.AppendFormat("Content-Type: {0}; charset=\"{1}\"", ContentType.Value, ContentEncoding.WebName);
                sb.Append(MailParser.NewLine);
            }
            else
            {
                sb.AppendFormat("Content-Type: {0};", ContentType.Value);
                sb.Append(MailParser.NewLine);
                if (String.IsNullOrEmpty(ContentType.Name) == false)
                {
                    if (_fieldParameterEncoding == FieldParameterEncoding.Rfc2047)
                    {
                        sb.AppendFormat(" name=\"{0}\"", MailParser.EncodeToMailHeaderLine(ContentType.Name
                            , ContentTransferEncoding, ContentEncoding, MailParser.MaxCharCountPerRow - 8));
                    }
                    else if (_fieldParameterEncoding == FieldParameterEncoding.Rfc2231)
                    {
                        sb.AppendFormat(MailParser.EncodeToMailHeaderLineByRfc2231("name", ContentType.Name
                            , ContentEncoding, MailParser.MaxCharCountPerRow - 8));
                    }
                    sb.Append(MailParser.NewLine);
                }
            }
            if (String.IsNullOrEmpty(ContentDisposition.Value) == false)
            {
                sb.AppendFormat("Content-Disposition: {0};", ContentDisposition.Value);
                sb.Append(MailParser.NewLine);
                if (String.IsNullOrEmpty(ContentDisposition.FileName) == false)
                {
                    if (_fieldParameterEncoding == FieldParameterEncoding.Rfc2047)
                    {
                        sb.AppendFormat(" filename=\"{0}\"", MailParser.EncodeToMailHeaderLine(ContentDisposition.FileName
                            , ContentTransferEncoding, ContentEncoding, MailParser.MaxCharCountPerRow - 12));
                    }
                    else if (_fieldParameterEncoding == FieldParameterEncoding.Rfc2231)
                    {
                        sb.AppendFormat(MailParser.EncodeToMailHeaderLineByRfc2231("filename", ContentDisposition.FileName
                            , ContentEncoding, MailParser.MaxCharCountPerRow - 12));
                    }
                    sb.Append(MailParser.NewLine);
                }
            }
            foreach (var f in from f in Header where String.IsNullOrEmpty(f.Value) != true let f1 = f where ExceptHeaderKeyList.Exists(key => String.Equals(key, f1.Key, StringComparison.OrdinalIgnoreCase)) != true select f)
            {
                if (_encodeHeaderKeys.Contains(f.Key.ToLower()))
                {
                    sb.AppendFormat("{0}: {1}{2}", f.Key
                                    , MailParser.EncodeToMailHeaderLine(f.Value, ContentTransferEncoding, ContentEncoding
                                                                        , MailParser.MaxCharCountPerRow - f.Key.Length - 2), MailParser.NewLine);
                }
                else
                {
                    sb.AppendFormat("{0}: {1}{2}", f.Key, f.Value, MailParser.NewLine);
                }
            }

            HeaderData = sb.ToString();
        }

        private void SetBodyData()
        {
            var sb = new StringBuilder(1024);

            if (IsMultiPart)
            {
                foreach (var t in _Contents)
                {
                    sb.Append("--");
                    sb.Append(MultiPartBoundary);
                    sb.Append(MailParser.NewLine);
                    sb.Append(t.Data);
                    sb.Append(MailParser.NewLine);
                }
                sb.AppendFormat("--{0}--", MultiPartBoundary);
            }
            else
            {
                var bodyText = IsAttachment ? BodyText : MailParser.EncodeToMailBody(BodyText, ContentTransferEncoding, ContentEncoding);
                if (ContentTransferEncoding == TransferEncoding.SevenBit)
                {
                    sb.Append(bodyText);
                }
                else
                {
                    for (var i = 0; i < bodyText.Length; i++)
                    {
                        if (i > 0 &&
                            i % 76 == 0)
                        {
                            sb.Append(MailParser.NewLine);
                        }
                        sb.Append(bodyText[i]);
                    }
                }
            }
            BodyData = sb.ToString();
        }
    }
}
