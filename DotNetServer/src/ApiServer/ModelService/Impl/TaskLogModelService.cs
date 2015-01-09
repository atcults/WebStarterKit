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
using Dto.ApiResponses.TaskLogResponses;

namespace WebApp.ModelService.Impl
{
    public class TaskLogModelService : BaseModelService, ITaskLogModelService
    {
        private readonly IViewRepository<TaskLogView> _taskLogViewRepository;
        private readonly IViewRepository<ContactView> _contactViewRepository;
        private readonly IViewRepository<AssignedContactView> _assignedContactViewRepository;
        private readonly IViewRepository<NoteView> _noteViewRepository;
        private readonly IViewRepository<AttachmentView> _attachmentViewRepository;

        public TaskLogModelService(IViewRepository<TaskLogView> taskLogViewRepository, IViewRepository<AssignedContactView> assignedContactViewRepository, IViewRepository<NoteView> noteViewRepository, IViewRepository<AttachmentView> attachmentViewRepository, IViewRepository<ContactView> contactViewRepository)
        {
            _taskLogViewRepository = taskLogViewRepository;
            _assignedContactViewRepository = assignedContactViewRepository;
            _noteViewRepository = noteViewRepository;
            _attachmentViewRepository = attachmentViewRepository;
            _contactViewRepository = contactViewRepository;
        }

        PageResponse<TaskLogLine> ITaskLogModelService.GetPageBySpecification(SearchSpecification specification)
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

            var taskLogViewPage = _taskLogViewRepository.SearchBySpecification(SimpleSearch.FromDepricated(specification));
            var response = Mapper.Map<Page<TaskLogView>, PageResponse<TaskLogLine>>(taskLogViewPage);
            return response;
        }

        public TaskLogDetailResponse GetTaskLogDetailById(Guid? id = null)
        {
            var response = new TaskLogDetailResponse();

            if (!id.HasValue) return response;

            var taskLogView = _taskLogViewRepository.GetById(id.Value);
            var allContacts = _contactViewRepository.FetchAll();
            var contactViews = _assignedContactViewRepository.GetAllFor(Property.Of<AssignedContactView>(x => x.ReferenceId), id);
            var noteViews = _noteViewRepository.GetAllFor(Property.Of<NoteView>(x => x.ReferenceId), id);
            var attachmentViews = _attachmentViewRepository.GetAllFor(Property.Of<AttachmentView>(x => x.ReferenceId), id);

            response.Detail = Mapper.Map<TaskLogView, TaskLogResponse>(taskLogView);
            response.AllContacts = allContacts.Select(Mapper.Map<ContactView, ContactLine>).ToArray();
            response.ContactLines = contactViews.Select(Mapper.Map<AssignedContactView, AssignedContactLine>).ToArray();
            response.Notes = noteViews.Select(Mapper.Map<NoteView, NoteLine>).ToArray();
            response.Attachments = attachmentViews.Select(Mapper.Map<AttachmentView, AttachmentLine>).ToArray();

            return response;
        }
    }
}
