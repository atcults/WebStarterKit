using System.Collections.Generic;
using Common.Enumerations;
using Common.Helpers;
using Common.Service;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class PasswordRecoveryTokenHandler : MessageHandler<PasswordRecoveryTokenCommand>
    {
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly ICryptographer _cryptographer;

        public PasswordRecoveryTokenHandler(IRepository<AppUser> appUserRepository, ICryptographer cryptographer, IUnitOfWork unitOfWork, IBus bus) : base(unitOfWork, bus)
        {
            _appUserRepository = appUserRepository;
            _cryptographer = cryptographer;
        }

        public override void HandleMessage(PasswordRecoveryTokenCommand command)
        {
            var member = _appUserRepository.GetById(command.UserId);

            var token = _cryptographer.CreateTemp();
            member.PasswordRetrievalToken = _cryptographer.ComputeHash(token);
            member.PasswordRetrievalTokenExpirationDate = SystemTime.Now().AddDays(1);

            _appUserRepository.Update(member);

            Bus.SendLocal<SendNotificationCommand>(m =>
            {
                m.UserId = member.Id;
                m.NotificationTypeValue = NotificationType.ResetPasswordRequest.Value;
                m.StaticData = new Dictionary<string, object>{{"TokenHash", token}};
            });
        }
    }
}
