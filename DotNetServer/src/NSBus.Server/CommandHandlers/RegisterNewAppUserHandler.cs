using System.Collections.Generic;
using System.Linq;
using Common.Enumerations;
using Common.Helpers;
using Common.Service;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class RegisterNewAppUserHandler : MessageHandler<RegisterNewAppUserCommand>
    {
        private readonly ICryptographer _cryptographer;
        private readonly IRepository<Contact> _contactRepository; 
        private readonly IRepository<AppUser> _userRepository;

        public RegisterNewAppUserHandler(IRepository<Contact> contactRepository, IRepository<AppUser> userRepository, IUnitOfWork unitOfWork, ICryptographer cryptographer, IBus bus) : base(unitOfWork, bus)
        {
            _contactRepository = contactRepository;
            _userRepository = userRepository;
            _cryptographer = cryptographer;
        }

        public override void HandleMessage(RegisterNewAppUserCommand command)
        {
            EnsureUniqueness(command);

            var serverhost = _contactRepository.GetByKey(Property.Of<Contact>(x => x.Name), "serverhost");

            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = command.Name,
                Email = command.Email,
                Mobile = command.Mobile,
                CreatedBy = serverhost.Id
            };

            var token = _cryptographer.CreateTemp();
            
            var user = new AppUser
            {
                Id = contact.Id,
                PasswordRetrievalToken = _cryptographer.ComputeHash(token),
                PasswordRetrievalTokenExpirationDate = SystemTime.Now().AddDays(1),
                UserStatus = UserStatus.Disabled
            };

            _contactRepository.Add(contact);
            _userRepository.Add(user);

            Bus.SendLocal<SendNotificationCommand>(m =>
            {
                m.UserId = contact.Id;
                m.NotificationTypeValue = NotificationType.UserRegistered.Value;
                m.StaticData = new Dictionary<string, object> { { "Token", token } };
            });
        }

        private void EnsureUniqueness(RegisterNewAppUserCommand command)
        {
            var alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Email), command.Email).SingleOrDefault();

            if (alradyAddedContact != null) throw new DomainProcessException("User with same email already exists");

            alradyAddedContact = _contactRepository.GetAllFor(Property.Of<Contact>(x => x.Mobile), command.Mobile).SingleOrDefault();

            if (alradyAddedContact != null) throw new DomainProcessException("User with same mobile already exists");
        }
    }
}
