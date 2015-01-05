using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Mail.Common
{
    /// Represent mailaddress when sending by smtp.
    /// <summary>
    /// Represent mailaddress when sending by smtp.
    /// </summary>
    public class MailAddress
    {
		private struct RegexList
		{
			public static readonly Regex DisplayNameMailAddress = new Regex("(?<DisplayName>.*)<(?<MailAddress>[^>]*)>");
			public static readonly Regex MailAddressWithBracket = new Regex("<(?<MailAddress>[^>]*)>");
		}

        private String _value = "";
        private String _displayName = "";
		private String _userName = "";
		private String _domainName = "";
        private Boolean _isDoubleQuote;
        private Encoding _encoding = Encoding.UTF8;
        private TransferEncoding _transferEncoding = TransferEncoding.Base64;

        /// Get or set the value of an email address.
        /// <summary>
        /// Get or set mailaddress value.
        /// Get or set the value of an email address.
        /// </summary>
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// Gets or sets the display name.
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public String DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        /// Gets or sets the user name.
		/// <summary>
        /// Gets or sets the user name.
		/// </summary>
		public String UserName
		{
			get { return _userName; }
			set { _userName = value; }
		}

        /// Gets or sets the domain name.
		/// <summary>
        /// Gets or sets the domain name.

		/// </summary>
		public String DomainName
		{
			get { return _domainName; }
			set { _domainName = value; }
		}

        /// Display name enclosed in double-coded annotations to indicate whether to get or set the value.
        /// <summary>
        /// Display name enclosed in double-coded annotations to indicate whether to get or set the value.
        /// </summary>
        public Boolean IsDoubleQuote
        {
            get { return _isDoubleQuote; }
            set { _isDoubleQuote = value; }
        }

        /// The display name is used in the encoding to get or set the Encoding.
        /// <summary>
        /// The display name is used in the encoding to get or set the Encoding.
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        /// The display name is used in the encoding to get or set the TransferEncoding.
        /// <summary>
        /// The display name is used in the encoding to get or set the TransferEncoding.
        /// </summary>
        public TransferEncoding TransferEncoding
        {
            get { return _transferEncoding; }
            set { _transferEncoding = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public X509Certificate2 SigningCertificate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public X509Certificate2 EncryptionCertificate { get; set; }
        /// <summary>
		/// 
		/// </summary>
		/// <param name="mailAddress"></param>
        public MailAddress(String mailAddress)
        {
            if (String.IsNullOrEmpty(mailAddress))
            { throw new FormatException(); }
			if (mailAddress.Contains("@") == false)
			{ throw new FormatException("Mail address must be contain @ char."); }

			var m = RegexList.MailAddressWithBracket.Match(mailAddress);
			_value = m.Success ? m.Groups["MailAddress"].Value : mailAddress;
			var ss = _value.Split('@');
			_userName = ss[0];
			_domainName = ss[1];
            InitializeProperty();
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mailAddress"></param>
		/// <param name="displayName"></param>
        public MailAddress(String mailAddress, String displayName) : this(mailAddress)
        {
            _displayName = displayName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="displayName"></param>
        /// <param name="encryptionCert"></param>
        /// <param name="signingCert"></param>
        public MailAddress(String mailAddress, String displayName, X509Certificate2 encryptionCert, X509Certificate2 signingCert) : this(mailAddress, displayName)
        {
            EncryptionCertificate = encryptionCert;
            if (signingCert != null && !signingCert.HasPrivateKey)
            {
                throw new InvalidOperationException("The specified signing certificate doesn't contain a private key.");
            }
            SigningCertificate = signingCert;
        }

        private void InitializeProperty()
        {
            if (CultureInfo.CurrentCulture.Name.StartsWith("ja"))
            {
                Encoding = Encoding.GetEncoding("iso-2022-jp");
                TransferEncoding = TransferEncoding.Base64;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(_displayName))
            {
                return String.Format("<{0}>", _value);
            }
            if (_isDoubleQuote)
            {
                return String.Format("\"{0}\" <{1}>", _displayName, _value);
            }
		    return String.Format("{0} <{1}>", _displayName, _value);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public String ToEncodeString()
        {
            return ToMailAddressText(_encoding,_transferEncoding
                , _value, _displayName, _isDoubleQuote);
        }

        /// In the encoding specified encoding display name to retrieve the string.
        /// <summary>
        /// Get mail address text encoded by specify encoding.
        /// In the encoding specified encoding display name to retrieve the string.
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="displayName"></param>
        /// <param name="doubleQuote"></param>
        /// <returns></returns>
        public static String ToMailAddressText(String mailAddress, String displayName, Boolean doubleQuote)
        {
            if (CultureInfo.CurrentCulture.Name.StartsWith("ja"))
            {
                return ToMailAddressText(Encoding.GetEncoding("iso-2022-jp"), TransferEncoding.Base64
                    , mailAddress, displayName, doubleQuote);
            }
            return ToMailAddressText(Encoding.UTF8, TransferEncoding.Base64, mailAddress, displayName, doubleQuote);
        }

        /// In the encoding specified encoding display name to retrieve the string.
        /// <summary>
        /// Get mail address text encoded by specify encoding.
        /// In the encoding specified encoding display name to retrieve the string.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="transferEncoding"></param>
        /// <param name="mailAddress"></param>
        /// <param name="displayName"></param>
        /// <param name="doubleQuote"></param>
        /// <returns></returns>
        public static String ToMailAddressText(Encoding encoding, TransferEncoding transferEncoding
            , String mailAddress, String displayName, Boolean doubleQuote)
        {
            if (String.IsNullOrEmpty(displayName))
            {
                return mailAddress;
            }
            if (doubleQuote)
            {
                return String.Format("\"{0}\" <{1}>", displayName, mailAddress);
            }
            return String.Format("{0} <{1}>"
                                 , MailParser.EncodeToMailHeaderLine(displayName, transferEncoding, encoding, MailParser.MaxCharCountPerRow - mailAddress.Length - 3)
                                 , mailAddress);
        }

        /// String MailAddress to the original instance of the generated.
        /// <summary>
        /// Create MailAddress object by mail address text.
        /// String MailAddress to the original instance of the generated.
        /// Display Name the format to set the values to MailAddress DisplayName and the instance.
        /// If the MailAddress email address in the to set the value of the instance.
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        public static MailAddress Create(String mailAddress)
        {
            var rx = RegexList.DisplayNameMailAddress;

            var m = rx.Match(mailAddress);

            if (String.IsNullOrEmpty(m.Value))
            {
                rx = RegexList.MailAddressWithBracket;
                m = rx.Match(mailAddress);
                return String.IsNullOrEmpty(m.Value) ? new MailAddress(mailAddress) : new MailAddress(m.Groups["MailAddress"].Value);
            }

            return String.IsNullOrEmpty(m.Groups["DisplayName"].Value) ? new MailAddress(mailAddress) : new MailAddress(m.Groups["MailAddress"].Value, m.Groups["DisplayName"].Value.TrimEnd(' '));
        }

        /// String to the original instance of MailAddress attempt to generate.
		/// <summary>
		/// Try to create MailAddress object by mail address text.
        /// String to the original instance of MailAddress attempt to generate.
        /// Email address cannot be converted to a string, the return value is null.
		/// </summary>
		/// <param name="mailAddress"></param>
		/// <returns></returns>
		public static MailAddress TryCreate(String mailAddress)
		{
			try
			{
				if (String.IsNullOrEmpty(mailAddress)) return null;
				return mailAddress.Contains("@") == false ? null : Create(mailAddress);
			}
			catch { }
			return null;
		}

        /// Email address cannot be converted to a string, the return value is null.
        /// <summary>
        /// Get mailaddress list from mail address list text.
        /// Email address cannot be converted to a string, the return value is null.
        /// </summary>
        /// <param name="mailAddressListText"></param>
        /// <param name="separators"></param>
        /// <returns></returns>
        public static List<MailAddress> CreateMailAddressList(String mailAddressListText, params Char[] separators)
        {
            if (separators == null || separators.Length == 0)
            {
                separators = new []{',', ';'};
            }

            var mailAddresses = new List<MailAddress>();
            var index = 0;

            for (var i = 0; i < mailAddressListText.Length; i++)
            {
                MailAddress mailAddress;
                string str;
                if (i == mailAddressListText.Length - 1)
                {
                    str = mailAddressListText.Substring(index, mailAddressListText.Length - index);
                    mailAddress = TryCreate(str.Trim());
                    if (mailAddress != null)
                    {
                        mailAddresses.Add(mailAddress);
                    }
                    break;
                }

                var isSeparator = separators.Any(t => mailAddressListText[i] == t);

                if (!isSeparator) continue;
                
                str = mailAddressListText.Substring(index, i - index);
                mailAddress = TryCreate(str.Trim());
                if (mailAddress != null)
                {
                    mailAddresses.Add(mailAddress);
                }
                index = i + 1;
            }
            return mailAddresses;
        }
    }
}
