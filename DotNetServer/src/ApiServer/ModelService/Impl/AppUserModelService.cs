using AutoMapper;
using Core.ViewOnly;
using Core.ViewOnly.Base;
using Core.Views;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.AppUserResponses;

namespace WebApp.ModelService.Impl
{
    public class AppUserModelService : IAppUserModelService
    {
        private readonly IViewRepository<AppUserView> _appUserViewRepository;

        public AppUserModelService(IViewRepository<AppUserView> appUserRepository)
        {
            _appUserViewRepository = appUserRepository;
        }

        public PageResponse<AppUserLine> GetPageBySpecification(SearchSpecification specification = null)
        {
            if (specification == null)
            {
                specification = new SearchSpecification
                {
                    ColumnName = "Name",
                    ColumnValue = "%",
                    FilterType = "like"
                };
            }

            var memberViewPage = _appUserViewRepository.SearchBySpecification(SimpleSearch.FromDepricated(specification));
            var response = Mapper.Map<Page<AppUserView>, PageResponse<AppUserLine>>(memberViewPage);
            return response;
        }
    }
}