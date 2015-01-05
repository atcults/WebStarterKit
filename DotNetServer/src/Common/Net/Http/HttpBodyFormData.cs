using System;
using System.Text;

namespace Common.Net.Http
{
    /// <summary>
    /// This class defines http body form data.
    /// </summary>
    public class HttpBodyFormData
    {
        private Byte[] _data;
        /// <summary>
        /// Define name of the form.
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Define name of filename.
        /// </summary>
        public String FileName { get; set; }
        /// <summary>
        ///Define type of content. 
        /// </summary>
        public String ContentType { get; set; }
        /// <summary>
        /// Define encoding for tranfer content.
        /// </summary>
        public String ContentTransferEncoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String ContentDisposition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Charset { get; set; }
        /// <summary>
        /// Data of file content
        /// </summary>
        public Byte[] Data
        {
            get { return _data; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public HttpBodyFormData(String name)
        {
            if (name == null) { throw new ArgumentNullException("name"); }
            Name = name;
            ContentDisposition = "form-data";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetData(Byte[] data)
        {
            _data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="text"></param>
        public void SetData(Encoding encoding, String text)
        {
            _data = encoding.GetBytes(text);
        }
    }
}
