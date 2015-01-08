using Common.Enumerations;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class UserLoginFailedHandler : MessageHandler<UserLoginFailedCommand>
    {
        private readonly IRepository<AppUser> _appUserRepository;

        private const int MaxFailedAttempts = 5;

        public UserLoginFailedHandler(IRepository<AppUser> appUserRepository, IUnitOfWork unitOfWork, IBus bus)
            : base(unitOfWork, bus)
        {
            _appUserRepository = appUserRepository;
        }

        public override void HandleMessage(UserLoginFailedCommand command)
        {
            var member = _appUserRepository.GetById(command.UserId);

            member.FailedAttemptCount++;
            if (member.FailedAttemptCount >= MaxFailedAttempts) member.UserStatus = UserStatus.Disabled;
            _appUserRepository.Update(member);

            Bus.SendLocal(new SendNotificationCommand
            {
                UserId = command.UserId,
                NotificationTypeValue = NotificationType.UserLoginFailed.Value
            });
        }
    }
}