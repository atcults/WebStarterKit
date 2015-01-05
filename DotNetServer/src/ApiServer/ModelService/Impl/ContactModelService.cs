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
using Dto.ApiResponses.NoteResponses;
using WebApp.Services;

namespace WebApp.ModelService.Impl
{
    public class ContactModelService : BaseModelService, IContactModelService
    {
        private readonly IViewRepository<ContactView> _contactViewRepository;
        private readonly IViewRepository<AssignedContactView> _assignedContactViewRepository;
        private readonly IViewRepository<NoteView> _noteViewRepository;
        private readonly IViewRepository<AttachmentView> _attachmentViewRepository;

        public ContactModelService(IUserSession userSession, IMappingEngine mappingEngine, IViewRepository<ContactView> contactViewRepository, IViewRepository<NoteView> noteViewRepository, IViewRepository<AttachmentView> attachmentViewRepository, IViewRepository<AssignedContactView> assignedContactViewRepository)
            : base(userSession, mappingEngine)
        {
            _contactViewRepository = contactViewRepository;
            _noteViewRepository = noteViewRepository;
            _attachmentViewRepository = attachmentViewRepository;
            _assignedContactViewRepository = assignedContactViewRepository;
        }

        public PageResponse<ContactLine> GetPageBySpecification(SearchSpecification specification)
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

            var contactViewPage = _contactViewRepository.SearchBySpecification(SimpleSearch.FromDepricated(specification));
            var response = Mapper.Map<Page<ContactView>, PageResponse<ContactLine>>(contactViewPage);
            return response;
        }

        public ContactDetailResponse GetContactDetailById(Guid? id)
        {
            var response = new ContactDetailResponse();

            if (!id.HasValue) return response;

            var contactView = _contactViewRepository.GetById(id.Value);
            var allContacts = _contactViewRepository.FetchAll();
            var contactViews = _assignedContactViewRepository.GetAllFor(Property.Of<AssignedContactView>(x => x.ReferenceId), id);
            var noteViews = _noteViewRepository.GetAllFor(Property.Of<NoteView>(x => x.ReferenceId), id);
            var attachmentViews = _attachmentViewRepository.GetAllFor(Property.Of<AttachmentView>(x => x.ReferenceId), id);

            response.Detail = Mapper.Map<ContactView, ContactResponse>(contactView);
            response.AllContacts = allContacts.Select(Mapper.Map<ContactView, ContactLine>).ToArray();
            response.ContactLines = contactViews.Select(Mapper.Map<AssignedContactView, AssignedContactLine>).ToArray();
            response.Notes = noteViews.Select(Mapper.Map<NoteView, NoteLine>).ToArray();
            response.Attachments = attachmentViews.Select(Mapper.Map<AttachmentView, AttachmentLine>).ToArray();

            return response;
        }
    }
}
