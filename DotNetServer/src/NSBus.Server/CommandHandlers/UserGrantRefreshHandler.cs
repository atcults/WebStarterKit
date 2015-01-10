using Common.Helpers;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class UserGrantRefreshHandler : MessageHandler<UserGrantRefreshCommand>
    {
        private readonly IRepository<AppClient> _appClientRepository;
        private readonly IRepository<TokenStore> _tokenStoreRepository;

        public UserGrantRefreshHandler(IUnitOfWork unitOfWork, IBus bus, IRepository<AppClient> appClientRepository, IRepository<TokenStore> tokenStoreRepository) : base(unitOfWork, bus)
        {
            _appClientRepository = appClientRepository;
            _tokenStoreRepository = tokenStoreRepository;
        }

        public override void HandleMessage(UserGrantRefreshCommand command)
        {
            var client = _appClientRepository.GetByKey(Property.Of<AppClient>(x => x.Name), command.Client);

            var from = command.TimeUtc;
            var tillRefresh = from.AddMinutes(client.RefreshTokenLifeTime);

            var token = _tokenStoreRepository.GetById(command.TokenId);

            token.RefreshTokenHash = command.RefreshTokenHash;
            token.RefreshTicket = command.ProtectedTicket;
            token.RefreshTokenIssuedUtc = from;
            token.RefreshTokenExpiresUtc = tillRefresh;
            token.TimesTokenGiven = token.TimesTokenGiven.GetValueOrDefault(0) + 1;

            _tokenStoreRepository.Update(token);
        }
    }
}