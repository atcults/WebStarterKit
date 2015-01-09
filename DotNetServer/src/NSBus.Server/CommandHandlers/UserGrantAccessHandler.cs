using Common.Enumerations;
using Common.Helpers;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NSBus.Dto.Messages;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class UserGrantAccessHandler : MessageHandler<UserGrantAccessCommand>
    {
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IRepository<AppClient> _appClientRepository;
        private readonly IRepository<TokenStore> _tokenStoreRepository;

        public UserGrantAccessHandler(IRepository<AppUser> appUserRepository, IUnitOfWork unitOfWork, IBus bus, IRepository<AppClient> appClientRepository, IRepository<TokenStore> tokenStoreRepository) : base(unitOfWork, bus)
        {
            _appUserRepository = appUserRepository;
            _appClientRepository = appClientRepository;
            _tokenStoreRepository = tokenStoreRepository;
        }

        public override void HandleMessage(UserGrantAccessCommand command)
        {
            var member = _appUserRepository.GetById(command.UserId);

            member.FailedAttemptCount = 0;
            member.PasswordRetrievalToken = null;
            member.LastLoginTime = command.TimeUtc;
            _appUserRepository.Update(member);

            var client = _appClientRepository.GetByKey(Property.Of<AppClient>(x => x.Name), command.Client);

            var from = command.TimeUtc;
            var tillAccess = from.AddMinutes(client.AccessTokenLifeTime);

            var token = _tokenStoreRepository.GetById(command.TokenId);

            if (token == null)
            {
                token = new TokenStore
                {
                    Id = command.TokenId,
                    ClientName = command.Client,
                    UserId = command.UserId,
                    UserName = command.UserName,
                    AccessTokenHash = command.AccessTokenHash,
                    AccessTicket = command.ProtectedTicket,
                    AccessTokenIssuedUtc = from,
                    AccessTokenExpiresUtc = tillAccess
                };
                _tokenStoreRepository.Add(token);
            }
            else
            {
                token.AccessTokenHash = command.AccessTokenHash;
                token.AccessTicket = command.ProtectedTicket; 
                token.AccessTokenIssuedUtc = from;
                token.AccessTokenExpiresUtc = tillAccess;
                _tokenStoreRepository.Update(token);
            }
            
            Bus.SendLocal(new SendNotificationCommand
            {
                UserId = command.UserId,
                NotificationTypeValue = NotificationType.UserLoginSuccess.Value
            });

            //Bus.Reply(new GenericResponseMessage
            //{
            //    IsSuccess = true
            //});
        }
    }
}
