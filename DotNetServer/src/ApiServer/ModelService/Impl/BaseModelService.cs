using AutoMapper;
using WebApp.Services;

namespace WebApp.ModelService.Impl
{
    public class BaseModelService
    {
        protected IUserSession UserSession { get; private set; }
        protected IMappingEngine MappingEngine { get; private set; }

        public BaseModelService(IUserSession userSession, IMappingEngine mappingEngine)
        {
            UserSession = userSession;
            MappingEngine = mappingEngine;
        }
    }
}