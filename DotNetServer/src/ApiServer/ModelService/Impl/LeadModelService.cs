using System;
using System.Linq;
using AutoMapper;
using Common.Helpers;
using Core.ViewOnly;
using Core.ViewOnly.Base;
using Core.Views;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.AttachmentResponses;
using Dto.ApiResponses.ContactResponses;
using Dto.ApiResponses.LeadResponses;
using Dto.ApiResponses.NoteResponses;
using WebApp.Services;

namespace WebApp.ModelService.Impl
{
    public class LeadModelService : BaseModelService, ILeadModelService
    {
        private readonly IViewRepository<LeadView> _leadViewRepository;
        private readonly IViewRepository<ContactView> _contactViewRepository;
        private readonly IViewRepository<NoteView> _noteViewRepository;
        private readonly IViewRepository<AssignedContactView> _assignedContactViewRepository;
        private readonly IViewRepository<AttachmentView> _attachmentViewRepository;

        public LeadModelService(IUserSession userSession, IMappingEngine mappingEngine, IViewRepository<LeadView> leadViewRepository, IViewRepository<NoteView> noteViewRepository, IViewRepository<AttachmentView> attachmentViewRepository, IViewRepository<AssignedContactView> assignedContactViewRepository, IViewRepository<ContactView> contactViewRepository)
            : base(userSession, mappingEngine)
        {
            _leadViewRepository = leadViewRepository;
            _noteViewRepository = noteViewRepository;
            _attachmentViewRepository = attachmentViewRepository;
            _assignedContactViewRepository = assignedContactViewRepository;
            _contactViewRepository = contactViewRepository;
        }

        public PageResponse<LeadLine> GetPageBySpecification(SearchSpecification specification)
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

            var leadViewPage = _leadViewRepository.SearchBySpecification(SimpleSearch.FromDepricated(specification));
            var response = Mapper.Map<Page<LeadView>, PageResponse<LeadLine>>(leadViewPage);
            return response;
        }

        public LeadDetailResponse GetLeadDetailById(Guid? id)
        {
            var response = new LeadDetailResponse();

            if (!id.HasValue) return response;

            var leadView = _leadViewRepository.GetById(id.Value);
            var allContacts = _contactViewRepository.FetchAll();
            var contactViews = _assignedContactViewRepository.GetAllFor(Property.Of<AssignedContactView>(x => x.ReferenceId), id);
            var noteViews = _noteViewRepository.GetAllFor(Property.Of<NoteView>(x => x.ReferenceId), id);
            var attachmentViews = _attachmentViewRepository.GetAllFor(Property.Of<AttachmentView>(x => x.ReferenceId), id);

            response.Detail = Mapper.Map<LeadView, LeadResponse>(leadView);
            response.AllContacts = allContacts.Select(Mapper.Map<ContactView, ContactLine>).ToArray();
            response.ContactLines = contactViews.Select(Mapper.Map<AssignedContactView, AssignedContactLine>).ToArray();
            response.Notes = noteViews.Select(Mapper.Map<NoteView, NoteLine>).ToArray();
            response.Attachments = attachmentViews.Select(Mapper.Map<AttachmentView, AttachmentLine>).ToArray();

            return response;
        }
    }
}