using System;
using System.Linq;
using Common.Helpers;
using Core.Commands;
using Core.Commands.ContactCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.ContactProcessors
{
    public class UpdateContactProcessor : ICommandProcessor<UpdateContact>
    {
        private readonly IRepository<Contact> _contactRepository;

        public UpdateContactProcessor(IRepository<Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public void Process(UpdateContact command, Guid userId, out IWebApiResponse response)
        {
            EnsureUniqueness(command);

            var contact = _contactRepository.GetById(command.Id);

            contact.Name = command.Name;
            contact.Mobile = command.Mobile;
            contact.Email = command.Email;
            contact.ContactType = command.ContactType;
            contact.PrimaryLanguage = command.PrimaryLanguage;
            contact.SecondaryLanguage = command.SecondaryLanguage;
            contact.Gender = command.Gender;
            contact.Description = command.Description;
            contact.ImageData = command.ImageData;
            contact.ModifiedBy = userId;

            _contactRepository.Update(contact);

            response = new WebApiResponseBase();
        }

        private void EnsureUniqueness(UpdateContact command)
        {
            var alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Email), command.Email).SingleOrDefault();

            if (alradyAddedContact != null && alradyAddedContact.Id != command.Id) throw new DomainProcessException("Contact with same email already exists");

            alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Mobile), command.Email).SingleOrDefault();

            if (alradyAddedContact != null && alradyAddedContact.Id != command.Id) throw new DomainProcessException("Contact with same mobile already exists");
        }
    }
}