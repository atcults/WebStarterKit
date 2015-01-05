using System;
using AutoMapper;
using Core.ViewOnly;
using Core.ViewOnly.Base;
using Core.Views;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.TemplateResponses;
using WebApp.Services;

namespace WebApp.ModelService.Impl
{
    public class TemplateModelService : BaseModelService, ITemplateModelService
    {
        private readonly IViewRepository<TemplateView> _templateViewRepository;
        public TemplateModelService(IUserSession userSession, IMappingEngine mappingEngine, IViewRepository<TemplateView> templateViewRepository)
            : base(userSession, mappingEngine)
        {
            _templateViewRepository = templateViewRepository;
        }

        public PageResponse<TemplateLine> GetPageBySpecification(SearchSpecification specification = null)
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

            var templateView = _templateViewRepository.SearchBySpecification(SimpleSearch.FromDepricated(specification));
            var response = Mapper.Map<Page<TemplateView>, PageResponse<TemplateLine>>(templateView);
            return response;
        }      

        public TemplateResponse GetTemplateById(Guid id)
        {
            var templateView = _templateViewRepository.GetById(id);
            var response = Mapper.Map<TemplateView, TemplateResponse>(templateView);
            return response;
        }
    }
}
