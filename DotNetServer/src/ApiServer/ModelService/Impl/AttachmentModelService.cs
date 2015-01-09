using System;
using System.Linq;
using AutoMapper;
using Core.ViewOnly;
using Core.ViewOnly.Base;
using Core.Views;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.AttachmentResponses;

namespace WebApp.ModelService.Impl
{
    public class AttachmentModelService : BaseModelService, IAttachmentModelService
    {
        private readonly IViewRepository<AttachmentView> _attachmentViewRepository;
        public AttachmentModelService(IViewRepository<AttachmentView> attachmentViewRepository)
        {
            _attachmentViewRepository = attachmentViewRepository;
        }

        public PageResponse<AttachmentLine> GetPageBySpecification(SearchSpecification specification = null)
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

            var accountViewPage = _attachmentViewRepository.SearchBySpecification(SimpleSearch.FromDepricated(specification));
            var response = Mapper.Map<Page<AttachmentView>, PageResponse<AttachmentLine>>(accountViewPage);
            return response;
        }

        public AttachmentResponse GetAttachmentById(Guid? id = null)
        {
            var response = new AttachmentResponse();

            if (!id.HasValue) return response;

            var attachmentView = _attachmentViewRepository.GetById((Guid)id);

            response = Mapper.Map<AttachmentView, AttachmentResponse>(attachmentView);

            return response;
        }

        public AttachmentSearchResponse GetAttachmentsBySearchValue(string value)
        {
            var attachmentViews = string.IsNullOrEmpty(value) ? _attachmentViewRepository.FetchAll() : _attachmentViewRepository.FetchAll().Where(x => x.Tags.ToLower().Contains(value.ToLower()) || x.Name.ToLower().Contains(value.ToLower()));
            return new AttachmentSearchResponse { AttachmentLines = attachmentViews.Select(Mapper.Map<AttachmentView, AttachmentLine>).ToArray() };
        }
    }
}
