using System;
using System.IO;
using Common.Helpers;
using Common.Service.Impl;
using Common.SystemSettings;

namespace Common.Base
{
    public static class Logger
    {
        private static readonly object Obj = new object();

        private static StreamWriter _writter;

        public static void Log(LogType logType, object source, string message, Exception exception = null)
        {
            Log(logType, source.GetType(), message, exception);
        }

        public static void Log(LogType logType, Type source, string message, Exception exception = null)
        {
            if (logType == LogType.Info && ConfigProvider.GetGeneralConfig().DoNotLogInfo)
            {
                return;
            }

            lock (Obj)
            {

                if (_writter == null)
                {
                    _writter = File.AppendText(Globals.LogFolder + SystemTime.Now().ToString("yyyyMMdd-HHmmss") + ".log");
                }

                _writter.Write("{0}-{1}-{2}{3}{4}{3}{5}{3}{3}", SystemTime.Now(), source, logType, Environment.NewLine, message, exception);
                _writter.Flush();
            }
        }
    }
}