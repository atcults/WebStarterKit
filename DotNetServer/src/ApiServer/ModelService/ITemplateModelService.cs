using System;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.TemplateResponses;

namespace WebApp.ModelService
{
    public interface ITemplateModelService
    {
        PageResponse<TemplateLine> GetPageBySpecification(SearchSpecification specification = null);
        TemplateResponse GetTemplateById(Guid id);
    }
}