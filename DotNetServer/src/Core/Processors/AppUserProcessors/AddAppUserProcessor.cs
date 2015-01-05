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
    public class AddAppUserProcessor : ICommandProcessor<AddAppUser>
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<AppUser> _userRepository;

        public AddAppUserProcessor(IRepository<Contact> contactRepository, IRepository<AppUser> userRepository)
        {
            _contactRepository = contactRepository;
            _userRepository = userRepository;
        }

        public void Process(AddAppUser command, Guid userId, out IWebApiResponse response)
        {
            EnsureUniqueness(command);

            var contact = new Contact
            {
                Id = command.Id,
                Name = command.Name,
                Email = command.Email,
                Mobile = command.Mobile,
                Description = command.Description,
                ImageData = command.ImageData,
                CreatedBy = userId
            };

            var user = new AppUser
            {
                Id = command.Id,
                Role = command.Role,
                UserStatus = command.UserStatus
            };

            _contactRepository.Add(contact);
            _userRepository.Add(user);

            response = new AppUserResponse
            {
                Id = command.Id,
                Name = command.Name,
                Mobile = command.Mobile,
                Email = command.Email,
                RoleName = command.Role.DisplayName,
                UserStatusName = command.UserStatus.DisplayName,
                Description = command.Description,
                ImageData = command.ImageData
            };
        }

        private void EnsureUniqueness(AddAppUser command)
        {
            var alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Email), command.Email).SingleOrDefault();

            if (alradyAddedContact != null) throw new DomainProcessException("User with same email already exists");

            alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Mobile), command.Mobile).SingleOrDefault();

            if (alradyAddedContact != null) throw new DomainProcessException("User with same mobile already exists");
        }
    }
}