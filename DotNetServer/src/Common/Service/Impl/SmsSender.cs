using System;
using Common.Base;
using Common.Net.Http;

namespace Common.Service.Impl
{
    public class SmsSender : ServiceBase, ISmsSender
    {
        public bool SendShortMessage(string body, string mobile)
        {
            mobile = mobile.Trim();

            var config = ConfigProvider.GetSmsConfig();

            try
            {
                if (string.IsNullOrEmpty(mobile) || mobile.Length != 10) throw new Exception(string.Format("Invalid mobile number: {0}", mobile));
                var reqUrl = string.Format(config.ServiceUrl, config.SenderName, mobile, body);
                var client = new HttpClient();
                var response = client.GetBodyText(reqUrl);
                Logger.Log(LogType.Info, this, response);
                //NOTE: Add parser to check response from server.
                //if(!res.ToLower().Contains("success")) throw new Exception(res);
            }
            catch (Exception exception)
            {
                Exception = exception;
                Logger.Log(LogType.Error, this, "SmsSender encountered an error.", exception);
                return false;
            }

            return true;
        }
    }
}
