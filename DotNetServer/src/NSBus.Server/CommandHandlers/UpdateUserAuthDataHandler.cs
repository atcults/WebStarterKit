using Common.Enumerations;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class UpdateUserAuthDataHandler : MessageHandler<UserLoginPostCommand>
    {
        private readonly IRepository<AppUser> _appUserRepository;

        private const int MaxFailedAttempts = 5;

        public UpdateUserAuthDataHandler(IRepository<AppUser> appUserRepository, IUnitOfWork unitOfWork, IBus bus) : base(unitOfWork, bus)
        {
            _appUserRepository = appUserRepository;
        }

        public override void HandleMessage(UserLoginPostCommand command)
        {
            var member = _appUserRepository.GetById(command.UserId);
            if (command.IsAuthSuccess)
            {
                member.FailedAttemptCount = 0;
                member.PasswordRetrievalToken = null;
                member.LastLoginTime = command.LoginDateTime;
                _appUserRepository.Update(member);
            }
            else
            {
                member.FailedAttemptCount++;
                if (member.FailedAttemptCount >= MaxFailedAttempts) member.UserStatus = UserStatus.Disabled;
                _appUserRepository.Update(member);
            }

            Bus.SendLocal(new SendNotificationCommand
            {
                UserId = command.UserId,
                NotificationTypeValue = command.IsAuthSuccess ? NotificationType.UserLoginSuccess.Value : NotificationType.UserLoginFailed.Value
            });
        }
    }
}
