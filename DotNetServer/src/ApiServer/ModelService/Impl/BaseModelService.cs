using AutoMapper;
using WebApp.Initialization;

namespace WebApp.ModelService.Impl
{
    public class BaseModelService
    {
        protected IMappingEngine MappingEngine { get; private set; }

        public BaseModelService()
        {
            MappingEngine = ClientEndPoint.Container.GetInstance<IMappingEngine>();
        }
    }
}