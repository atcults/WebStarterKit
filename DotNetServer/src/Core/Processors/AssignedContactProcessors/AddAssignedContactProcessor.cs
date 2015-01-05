using System;
using System.Linq;
using Common.Helpers;
using Core.Commands;
using Core.Commands.ContactCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.ContactResponses;

namespace Core.Processors.AssignedContactProcessors
{
    public class AddAssignedContactProcessor : ICommandProcessor<AddAssignedContact>
    {
        private readonly IRepository<AssignedContact> _assignedcontactRepository;
        private readonly IRepository<Contact> _contactRepository;

        public AddAssignedContactProcessor(IRepository<AssignedContact> assignedcontactRepository,
            IRepository<Contact> contactRepository)
        {
            _assignedcontactRepository = assignedcontactRepository;
            _contactRepository = contactRepository;
        }

        public void Process(AddAssignedContact command, Guid userId, out IWebApiResponse response)
        {
            EnsureDuplicateContact(command);

            if (command.ReferenceId == command.ContactId)
                throw new DomainProcessException("Does not assign contact itself.");

            var contact = _contactRepository.GetById(command.ContactId);
            if (contact == null) throw new DomainProcessException("Invalid Contact");

            var assignedcontact = new AssignedContact
            {
                Id = command.Id,
                Name = contact.Name,
                ContactId = command.ContactId,
                EntityType = command.EntityType,
                ReferenceId = command.ReferenceId,
                ReferenceName = command.ReferenceName,
                Description = command.Description,
                ImageData = command.ImageData,
                CreatedBy = userId
            };
            _assignedcontactRepository.Add(assignedcontact);

            response = new AssignedContactResponse
            {
                Id = command.Id,
                ContactId = command.ContactId,
                Name = contact.Name,
                EntityTypeValue = command.EntityType.Value,
                Mobile = contact.Mobile,
                Email = contact.Email,
                Description = command.Description
            };
        }

        private void EnsureDuplicateContact(AddAssignedContact command)
        {
            var alradyAddedContact =
                _assignedcontactRepository.GetAllFor(Property.Of<AssignedContact>(x => x.ReferenceId),
                    command.ReferenceId).SingleOrDefault(x => x.ContactId == command.ContactId);

            if (alradyAddedContact == null) return;
            throw new DomainProcessException("Contact already assign.");
        }
    }
}