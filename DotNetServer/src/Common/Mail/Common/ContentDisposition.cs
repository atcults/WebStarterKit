using System;
using System.Collections.Generic;

namespace Common.Mail.Common
{
	/// Represent Content-Disposition as class.
	/// <summary>
	/// Represent Content-Disposition as class.
	/// </summary>
	public class ContentDisposition : InternetTextMessage.Field
	{
		private readonly List<InternetTextMessage.Field> _fields = new List<InternetTextMessage.Field>();
		/// <summary>
		/// Get field collection.
		/// </summary>
		public List<InternetTextMessage.Field> Fields
		{
			get { return _fields; }
		}

		/// <summary>
		/// Get or set filename.
		/// </summary>
		public String FileName
		{
			get
			{
				var field = FindField(_fields, "FileName");
				if (field == null)
				{
					return "";
				}
				return MailParser.DecodeFromMailHeaderLine(field.Value);
			}
			set
			{
				var filename = FindField(_fields, "FileName");
				if (filename == null)
				{
					filename = new InternetTextMessage.Field("FileName", value);
					_fields.Add(filename);
				}
				else
				{
					filename.Value = value;
				}
				Value = "attachment";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public ContentDisposition(String value) :
			base("Content-Disposition", value)
		{
			Value = value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="fields"></param>
		public ContentDisposition(String value, IEnumerable<InternetTextMessage.Field> fields) :
			base("Content-Disposition", value)
		{
		    Value = value;
		    foreach (var textmsg in fields)
		    {
		        _fields.Add(textmsg);
		    }
		}
	}
}
