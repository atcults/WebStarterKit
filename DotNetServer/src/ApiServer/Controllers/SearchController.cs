using System.Net.Http;
using AutoMapper;
using WebApp.ModelService;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class SearchController : SmartApiController
    {
        private readonly IAttachmentModelService _attachmentModelService;

        public SearchController(IUserSession userSession, IMappingEngine mappingEngine, IAttachmentModelService attachmentModelService)
            : base(userSession, mappingEngine)
        {
            _attachmentModelService = attachmentModelService;
        }

        public HttpResponseMessage Get(string id)
        {
            var response = _attachmentModelService.GetAttachmentsBySearchValue(id);
            return Content(response);
        }
    }
}
