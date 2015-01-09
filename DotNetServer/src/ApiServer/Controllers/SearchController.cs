using System.Net.Http;
using WebApp.ModelService;

namespace WebApp.Controllers
{
    public class SearchController : SmartApiController
    {
        private readonly IAttachmentModelService _attachmentModelService;

        public SearchController(IAttachmentModelService attachmentModelService)
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
