using System;

namespace Common.SystemSettings
{
    [Serializable]
    public class GeneralConfig
    {
        public string ApplicationBaseUrl { get; set; }
        public string ApplicationHealthUrl { get; set; }
        public string RunMode { get; set; }
        public bool StoreConfigInDatabase { get; set; }

        /// <summary>
        /// Log Information object. Used for debug purpose.
        /// </summary>
        public bool DoNotLogInfo { get; set; }
        //folders path
        public string UploadFilePath { get; set; }
        public string DownloadFilePath { get; set; }
        /// <summary>
        /// Output to Console, File, Database
        /// </summary>
        public string LogOutputTo { get; set; }

        public override string ToString()
        {
            return string.Format("StoredConfigInDatabase: {0}, ApplicationBaseUrl: {1}, ApplicationHealthUrl: {2}, DoNotLogInfo: {3}, LogOutputTo: {4}, RunMode: {5}", StoreConfigInDatabase, ApplicationBaseUrl, ApplicationHealthUrl, DoNotLogInfo, LogOutputTo, RunMode);
        }
    }
}