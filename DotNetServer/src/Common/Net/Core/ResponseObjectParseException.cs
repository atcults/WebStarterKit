using System;

namespace Common.Net.Core
{
    /// <summary>
    /// This class Represent Response object parsing exception
    /// </summary>
    [Serializable]
    public class ResponseObjectParseException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public String Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public ResponseObjectParseException(String key)
        {
            Key = key;
        }
    }
}
