using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.AppUserResponses;

namespace WebApp.ModelService
{
    public interface IAppUserModelService
    {
        PageResponse<AppUserLine> GetPageBySpecification(SearchSpecification specification = null);
    }
}