using System;

namespace Common.Service.Impl
{
    public class ServiceBase : IServiceCommon
    {
        protected Exception Exception;
        public Exception GetLastException()
        {
            return Exception;
        }
    }
}