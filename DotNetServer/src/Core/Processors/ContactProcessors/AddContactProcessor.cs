using System;
using System.Linq;
using AutoMapper;
using Common.Helpers;
using Core.Commands;
using Core.Commands.ContactCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.ContactResponses;

namespace Core.Processors.ContactProcessors
{
    public class AddContactProcessor : ICommandProcessor<AddContact>
    {
        private readonly IRepository<Contact> _contactRepository;

        public AddContactProcessor(IRepository<Contact> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public void Process(AddContact command, Guid userId, out IWebApiResponse response)
        {
            EnsureUniqueness(command);

            var contact = new Contact
            {
                Id = command.Id,
                Name = command.Name,
                Mobile = command.Mobile,
                Email = command.Email,
                ContactType = command.ContactType,
                PrimaryLanguage = command.PrimaryLanguage,
                SecondaryLanguage = command.SecondaryLanguage,
                Gender = command.Gender,
                Description = command.Description,
                ImageData = command.ImageData,
                CreatedBy = userId
            };

            _contactRepository.Add(contact);

            response = Mapper.Map<Contact, ContactResponse>(contact);

        }

        private void EnsureUniqueness(AddContact command)
        {
            var alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Email), command.Email).SingleOrDefault();

            if (alradyAddedContact != null) throw new DomainProcessException("Contact with same email already exists");

            alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Mobile), command.Mobile).SingleOrDefault();

            if (alradyAddedContact != null) throw new DomainProcessException("Contact with same mobile already exists");
        }
    }
}