using System;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Service;
using Core.ViewOnly;
using Core.Views;
using Microsoft.Owin.Security.Infrastructure;
using NSBus.Dto.Commands;
using NServiceBus;

namespace WebApp.Services.Impl
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IViewRepository<TokenStoreView> _tokenStoreViewRepository;
        private readonly ICryptographer _cryptographer;
        private readonly IBus _bus;

        public RefreshTokenProvider(IViewRepository<TokenStoreView> tokenStoreViewRepository, ICryptographer cryptographer, IBus bus)
        {
            _cryptographer = cryptographer;
            _bus = bus;
            _tokenStoreViewRepository = tokenStoreViewRepository;
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];
            var tokenId = Guid.Parse(context.Ticket.Properties.Dictionary["as:token_id"]);

            if (string.IsNullOrEmpty(clientid))
            {
                return Task.FromResult<object>(null);
            }
            
            var refreshToken = Guid.NewGuid().ToString("n");

            _bus.Send<UserGrantRefreshCommand>(m =>
            {
                m.TokenId = tokenId;
                m.Client = clientid;
                m.RefreshTokenHash = _cryptographer.ComputeHash(refreshToken);
                m.ProtectedTicket = context.SerializeTicket();
                m.TimeUtc = SystemTime.Now();
            });

            context.SetToken(refreshToken);

            return Task.FromResult<object>(null);
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var tokenHash = _cryptographer.ComputeHash(context.Token);
            var tokenStoreView = _tokenStoreViewRepository.GetByKey(Property.Of<TokenStoreView>(x => x.RefreshTokenHash), tokenHash);

            if (tokenStoreView != null)
            {
                context.DeserializeTicket(tokenStoreView.RefreshTicket);
            }

            return Task.FromResult<object>(null);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}