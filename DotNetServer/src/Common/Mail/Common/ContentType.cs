using System;
using System.Collections.Generic;

namespace Common.Mail.Common
{
    /// Represent Content-Type as class.
    /// <summary>
    /// Represent Content-Type as class.
    /// </summary>
    public class ContentType : InternetTextMessage.Field
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
        /// Get or set name.
        /// </summary>
        public String Name
        {
            get
            {
                var name = FindField(_fields, "Name");
                if (name == null)
                {
                    return "";
                }
				return MailParser.DecodeFromMailHeaderLine(name.Value);
			}
            set
            {
                var name = FindField(_fields, "Name");
                if (name == null)
                {
                    name = new InternetTextMessage.Field("Name", value);
                    _fields.Add(name);
                }
                else
                {
                    name.Value = value;
                }
            }
        }

        /// <summary>
        /// Get or set boundary.
        /// </summary>
        public String Boundary
        {
            get
            {
                var boundary = FindField(_fields, "Boundary");
                if (boundary == null)
                {
                    return "";
                }
                return boundary.Value;
            }
            set
            {
                var boundary = FindField(_fields, "Boundary");
                if (boundary == null)
                {
                    boundary = new InternetTextMessage.Field("Boundary", value);
                    _fields.Add(boundary);
                }
                else
                {
                    boundary.Value = value;
                }
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
        public ContentType(String value) : 
            base("Content-Type", value)
        {
            Value = value;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="fields"></param>
        public ContentType(String value, IEnumerable<InternetTextMessage.Field> fields) :
            base("Content-Type", value)
		{
		    Value = value;
		    foreach (var textmsg in fields)
		    {
		        _fields.Add(textmsg);
		    }
		}
    }
}
