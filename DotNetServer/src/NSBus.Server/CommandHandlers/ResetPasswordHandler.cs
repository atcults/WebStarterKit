using Common.Enumerations;
using Common.Helpers;
using Common.Service;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class ResetPasswordHandler : MessageHandler<ResetPasswordCommand>
    {
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly ICryptographer _cryptographer;
        public ResetPasswordHandler(IRepository<AppUser> appUserRepository, ICryptographer cryptographer, IUnitOfWork unitOfWork, IBus bus) : base(unitOfWork, bus)
        {
            _appUserRepository = appUserRepository;
            _cryptographer = cryptographer;
        }

        public override void HandleMessage(ResetPasswordCommand command)
        {
            var token = _cryptographer.ComputeHash(command.Token);
            var member = _appUserRepository.GetByKey(Property.Of<AppUser>(x => x.PasswordRetrievalToken), token);

            if (member == null) return;

            var salt = _cryptographer.CreateSalt();
            var hash = _cryptographer.ComputeHash(command.NewPassword + salt);

            member.PasswordHash = hash;
            member.PasswordSalt = salt;
            member.LastPasswordChangedDate = SystemTime.Now();
            member.PasswordRetrievalToken = "";
            member.PasswordRetrievalTokenExpirationDate = null;
            member.FailedAttemptCount = 0;
            member.UserStatus = UserStatus.Active;

            _appUserRepository.Update(member);

            Bus.SendLocal<SendNotificationCommand>(m =>
            {
                m.UserId = member.Id;
                m.NotificationTypeValue = NotificationType.ResetPasswordSuccess.Value;
            });
        }
    }
}
