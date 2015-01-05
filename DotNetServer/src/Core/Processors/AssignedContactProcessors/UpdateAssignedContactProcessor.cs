using System;
using Core.Commands;
using Core.Commands.ContactCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.ContactResponses;

namespace Core.Processors.AssignedContactProcessors
{
    public class UpdateAssignedContactProcessor : ICommandProcessor<UpdateAssignedContact>
    {
        private readonly IRepository<AssignedContact> _assignedcontactRepository;
        private readonly IRepository<Contact> _contactRepository;

        public UpdateAssignedContactProcessor(IRepository<AssignedContact> assignedcontactRepository,
            IRepository<Contact> contactRepository)
        {
            _assignedcontactRepository = assignedcontactRepository;
            _contactRepository = contactRepository;
        }

        public void Process(UpdateAssignedContact command, Guid userId, out IWebApiResponse response)
        {
            var contact = _contactRepository.GetById(command.ContactId);
            if (contact == null) throw new DomainProcessException("Invalid Contact");

            var assignedcontact = _assignedcontactRepository.GetById(command.Id);

            assignedcontact.ContactId = command.ContactId;
            assignedcontact.Name = contact.Name;
            assignedcontact.EntityType = command.EntityType;
            assignedcontact.ReferenceId = command.ReferenceId;
            assignedcontact.Description = command.Description;
            assignedcontact.ImageData = command.ImageData;
            assignedcontact.ModifiedBy = userId;

            _assignedcontactRepository.Update(assignedcontact);

            response = new AssignedContactResponse
            {
                Id = command.Id,
                ContactId = command.ContactId,
                Name = contact.Name,
                Description = ""
            };
        }
    }
}