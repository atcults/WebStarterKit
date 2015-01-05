using System;
using Common.Helpers;
using Core.Commands;
using Core.Commands.AppUserCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AppUserProcessors
{
    public class DeleteAppUserProcessor : ICommandProcessor<DeleteAppUser>
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<AppUser> _userRepository;

        public DeleteAppUserProcessor(IRepository<Contact> contactRepository, IRepository<AppUser> userRepository)
        {
            _contactRepository = contactRepository;
            _userRepository = userRepository;
        }

        public void Process(DeleteAppUser command, Guid userId, out IWebApiResponse response)
        {
            var admin = _contactRepository.GetByKey(Property.Of<Contact>(x => x.Name), "admin");
            if (admin != null && admin.Id == command.Id)
            {
                throw new DomainProcessException("Admin user can not be deleted");
            }

            var guest = _contactRepository.GetByKey(Property.Of<Contact>(x => x.Name), "guest");
            if (guest != null && guest.Id == command.Id)
            {
                throw new DomainProcessException("Guest user can not be deleted");
            }

            _userRepository.Delete(command.Id);

            response = new WebApiResponseBase();
        }
    }
}