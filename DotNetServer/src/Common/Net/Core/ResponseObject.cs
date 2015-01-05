using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Common.Net.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ResponseObject
    {
        /// <summary>
        /// 
        /// </summary>
        public String JsonText { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public XElement XElement { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<String, Object> _data = new Dictionary<string,object>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Object this[String name]
        {
            get
            {
                if (_data.ContainsKey(name) == false) { return null; }
                return _data[name];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, Object>.KeyCollection Keys
        {
            get { return _data.Keys; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonText"></param>
        public virtual void SetProperty(String jsonText)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public virtual void SetProperty(XElement element)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        protected Dictionary<String, Object> SetData(String jsonText)
        {
            if (String.IsNullOrEmpty(jsonText))
            {
                 JsonText = "";
                _data = new Dictionary<string, object>();
            }
            else
            {
                JsonText = jsonText;
                _data = JsonConvert.DeserializeObject<Dictionary<String, Object>>(jsonText);
            }
            return _data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected Dictionary<String, Object> SetElements(XElement element)
        {
            XElement = element;
            foreach (var d in element.Elements())
            {
                _data[d.Name.LocalName] = d.Value;
            }
            foreach (var d in element.Attributes())
            {
                _data[d.Name.LocalName] = d.Value;
            }
            return _data;
        }
    }
}
