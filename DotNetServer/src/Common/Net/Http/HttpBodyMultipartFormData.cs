using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpBodyMultipartFormData
    {
        private readonly List<HttpBodyFormData> _formDataList = new List<HttpBodyFormData>();
        /// <summary>
        /// 
        /// </summary>
        public String Boundary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<HttpBodyFormData> FormDataList
        {
            get { return _formDataList; }
        }

        /// <summary>
        /// 
        /// </summary>
        public HttpBodyMultipartFormData()
        {
            Boundary = "----------------------" + Guid.NewGuid().ToString("N").Substring(0, 12);
            Encoding = HttpClient.DefaultEncoding;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Byte[] CreateBodyData()
        {
            var l = new List<byte>();
            var sb = new StringBuilder(256);

            foreach (var fd in _formDataList)
            {
                sb.Length = 0;
                sb.AppendLine("--" + Boundary);
                sb.AppendFormat("Content-Disposition: {0}; name=\"{1}\";", fd.ContentDisposition, fd.Name);
                if (fd.FileName != null)
                {
                    sb.AppendFormat(" filename=\"{0}\";", fd.FileName);
                }
                sb.AppendLine();

                if (String.IsNullOrEmpty(fd.ContentType) == false)
                {
                    sb.AppendFormat("Content-Type: {0};", fd.ContentType);
                    if (String.IsNullOrEmpty(fd.Charset) == false)
                    {
                        sb.Append("charset=" + fd.Charset);
                    }
                    sb.AppendLine();
                }
                
                if (String.IsNullOrEmpty(fd.ContentTransferEncoding) == false)
                {
                    sb.AppendLine("Content-Transfer-Encoding: " + fd.ContentTransferEncoding);
                }
                sb.AppendLine();
                l.AddRange(Encoding.GetBytes(sb.ToString()));

                if (fd.Data != null)
                {
                    l.AddRange(fd.Data);
                }
            }
            sb.AppendLine("--" + Boundary + "--");

            return l.ToArray();
        }
    }
}
