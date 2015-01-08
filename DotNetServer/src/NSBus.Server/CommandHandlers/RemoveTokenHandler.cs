using Common.Helpers;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class RemoveTokenHandler : MessageHandler<RemoveTokenCommand>
    {
        private readonly IRepository<TokenStore> _tokenStoreRepository;

        public RemoveTokenHandler(IUnitOfWork unitOfWork, IBus bus, IRepository<TokenStore> tokenStoreRepository) : base(unitOfWork, bus)
        {
            _tokenStoreRepository = tokenStoreRepository;
        }

        public override void HandleMessage(RemoveTokenCommand command)
        {
            var token = _tokenStoreRepository.GetByKey(Property.Of<TokenStore>(x => x.AccessTokenHash), command.TokenHash);
            if(token != null) _tokenStoreRepository.Delete(token.Id);
        }
    }
}