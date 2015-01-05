using System;

namespace Common.SystemSettings
{
    [Serializable]
    public class SmsConfig
    {
        public SmsConfig()
        {
            ServiceUrl = "";
            SenderName = "";
        }

        public string ServiceUrl { get; set; }
        public string SenderName { get; set; }
    }
}