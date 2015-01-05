using System;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.LeadResponses;

namespace WebApp.ModelService
{
    public interface ILeadModelService
    {
        PageResponse<LeadLine> GetPageBySpecification(SearchSpecification specification = null);
        LeadDetailResponse  GetLeadDetailById(Guid? id = null);
    }
}