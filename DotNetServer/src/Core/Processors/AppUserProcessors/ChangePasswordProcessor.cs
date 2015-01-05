using System;
using Common.Enumerations;
using Common.Helpers;
using Common.Service;
using Core.Commands;
using Core.Commands.AppUserCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using NSBus.Dto.Commands;
using NServiceBus;

namespace Core.Processors.AppUserProcessors
{
    public class ChangePasswordProcessor : ICommandProcessor<ChangePassword>
    {
        private readonly IRepository<AppUser> _userRepository;
        
        private readonly ICryptographer _cryptographer;
        private readonly IBus _bus;

        public ChangePasswordProcessor(IRepository<AppUser> userRepository, ICryptographer cryptographer, IBus bus)
        {
            _userRepository = userRepository;
            _cryptographer = cryptographer;
            _bus = bus;
        }

        public void Process(ChangePassword command, Guid userId, out IWebApiResponse response)
        {
            var user = _userRepository.GetById(command.Id);

            var salt = _cryptographer.CreateSalt();
            var hash = _cryptographer.ComputeHash(command.NewPassword + salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.LastPasswordChangedDate = SystemTime.Now();
            user.PasswordRetrievalToken = "";
            user.PasswordRetrievalTokenExpirationDate = SystemTime.Now();

            _userRepository.Update(user);

            _bus.Send("ApiServerService", new SendNotificationCommand
            {
                UserId = user.Id,
                NotificationTypeValue = NotificationType.ChangePassword.Value
            });

            response = new WebApiResponseBase();
        }
    }
}