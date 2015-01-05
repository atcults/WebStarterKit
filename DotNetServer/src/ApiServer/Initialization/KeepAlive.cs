using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using Common.Base;
using Common.Service.Impl;

namespace WebApp.Initialization
{
    /// <summary>
    /// KeppAlive application in iis
    /// </summary>
    public class KeepAlive
    {
        private static KeepAlive _instance;
        private static readonly object Sync = new object();
        private readonly string _cacheKey;

        private KeepAlive()
        {
            _cacheKey = Guid.NewGuid().ToString();
            _instance = this;
        }

        public static bool IsKeepingAlive
        {
            get
            {
                lock (Sync)
                {
                    Logger.Log(LogType.Debug, typeof(KeepAlive), "IsKeppingAlive Fuction it return bool value");
                    return _instance != null;
                }
            }
        }

        public static void Start()
        {
            if (IsKeepingAlive)
            {
                Logger.Log(LogType.Debug, typeof(KeepAlive), "Check out IsKeppingAlive or not");
                return;
            }
            lock (Sync)
            {
                _instance = new KeepAlive();
                _instance.Insert();
                Logger.Log(LogType.Debug, typeof(KeepAlive), "Start Keep Alive");
            }
        }

        public static void Stop()
        {
            lock (Sync)
            {
                HttpRuntime.Cache.Remove(_instance._cacheKey);
                _instance = null;
                Logger.Log(LogType.Debug, typeof(KeepAlive), "Stop Keep Alive");
            }
        }

        private void Callback(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason != CacheItemRemovedReason.Expired) return;
            Logger.Log(LogType.Debug, typeof(KeepAlive), "KeepAlive cache is expired.");
            FetchApplicationUrl();
            Insert();
        }

        private void Insert()
        {
            HttpRuntime.Cache.Add(_cacheKey,
                                  this,
                                  null,
                                  Cache.NoAbsoluteExpiration,
                                  new TimeSpan(0, 5, 0),
                                  CacheItemPriority.Normal,
                                  Callback);
        }

        private static void FetchApplicationUrl()
        {
            
            var buf = new byte[8192];
            try
            {
                var config = ConfigProvider.GetGeneralConfig();

                Logger.Log(LogType.Info, typeof(KeepAlive), "Request :" + config.ApplicationHealthUrl);

                var request = (HttpWebRequest)WebRequest.Create(config.ApplicationHealthUrl);

                request.Timeout = 10000;
                request.ReadWriteTimeout = 10000;

                var response = (HttpWebResponse)request.GetResponse();

                var resStream = response.GetResponseStream();

                if (resStream == null) throw new Exception("Response is null");

                int count;

                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                }
                while (count > 0); // any more data to read?

                Logger.Log(LogType.Info, typeof(KeepAlive), "Request success for KeepAlive");
            }
            catch (Exception exception)
            {
                Logger.Log(LogType.Info, typeof(KeepAlive), exception.Message);
            }
        }
    }
}
