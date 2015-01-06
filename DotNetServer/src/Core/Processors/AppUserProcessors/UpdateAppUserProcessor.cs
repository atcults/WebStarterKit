using System;
using System.Linq;
using Common.Helpers;
using Core.Commands;
using Core.Commands.AppUserCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.AppUserResponses;

namespace Core.Processors.AppUserProcessors
{
    public class UpdateAppUserProcessor : ICommandProcessor<UpdateAppUser>
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<AppUser> _userRepository;

        public UpdateAppUserProcessor(IRepository<Contact> contactRepository, IRepository<AppUser> userRepository)
        {
            _contactRepository = contactRepository;
            _userRepository = userRepository;
        }

        public void Process(UpdateAppUser command, Guid userId, out IWebApiResponse response)
        {
            EnsureUniqueness(command);

            var contact = _contactRepository.GetById(command.Id);

            if(contact == null) throw new DomainProcessException("No user exist");

            contact.Name = command.Name;
            contact.Email = command.Email;
            contact.Mobile = command.Mobile;
            contact.Description = command.Description;
            contact.ImageData = command.ImageData;
            contact.ModifiedBy = userId;

            _contactRepository.Update(contact);

            var user = _userRepository.GetById(command.Id);
            if (user == null) throw new DomainProcessException("No user exist");
            
            user.UserStatus = command.UserStatus;

            _userRepository.Update(user);

            response = new AppUserResponse
            {
                Id = command.Id,
                Name = command.Name,
                Description = command.Description,
                ImageData = command.ImageData
            };
        }

        private void EnsureUniqueness(AddAppUser command)
        {
            var alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Email), command.Email).SingleOrDefault();

            if (alradyAddedContact != null && alradyAddedContact.Id != command.Id) throw new DomainProcessException("User with same email already exists");

            alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Mobile), command.Email).SingleOrDefault();

            if (alradyAddedContact != null && alradyAddedContact.Id != command.Id) throw new DomainProcessException("User with same mobile already exists");
        }
    }
}