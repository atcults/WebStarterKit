using System;
using Common.Helpers;
using Core.Commands;
using Core.Commands.ContactCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.ContactProcessors
{
    public class DeleteContactProcessor : ICommandProcessor<DeleteContact>
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<AssignedContact> _assignedContactRepository;

        public DeleteContactProcessor(IRepository<Contact> contactRepository, IRepository<AssignedContact> assignedContactRepository)
        {
            _contactRepository = contactRepository;
            _assignedContactRepository = assignedContactRepository;
        }

        public void Process(DeleteContact command, Guid userId, out IWebApiResponse response)
        {
            var contact = _contactRepository.GetById(command.Id);
            var contactUsed = false;

            var assignedContact = _assignedContactRepository.GetAllFor(Property.Of<AssignedContact>(x => x.ContactId), command.Id);
            if (assignedContact.Count > 0) contactUsed = true;

            if (!contactUsed)
            {
                _contactRepository.Delete(contact.Id);
                response = new WebApiResponseBase();
            }
            else
            {
                throw new DomainProcessException("Contact already used");
            }
        }
    }
}