using System;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Net.Core;
using Common.Service;
using Core.ViewOnly;
using Core.Views;
using Microsoft.Owin.Security.Infrastructure;
using NSBus.Dto.Commands;
using NServiceBus;

namespace WebApp.Services.Impl
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly IViewRepository<AppClientView> _clientViewRepository;
        private readonly IViewRepository<TokenStoreView> _tokenStoreViewRepository;
        private readonly ICryptographer _cryptographer;
        private readonly IBus _bus;

        public RefreshTokenProvider(IViewRepository<TokenStoreView> tokenStoreViewRepository, IViewRepository<AppClientView> clientViewRepository, IBus bus, ICryptographer cryptographer)
        {
            _bus = bus;
            _cryptographer = cryptographer;
            _tokenStoreViewRepository = tokenStoreViewRepository;
            _clientViewRepository = clientViewRepository;
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var now = DateTime.UtcNow;
            var till = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime));

            context.Ticket.Properties.IssuedUtc = now;
            context.Ticket.Properties.ExpiresUtc = till;

            context.SetToken(refreshTokenId);

            _bus.Send<AddRefreshTokenCommand>(m =>
            {
                m.ClientId = clientid;
                m.Name = context.Ticket.Identity.Name;
                m.TokenHash = _cryptographer.ComputeHash(refreshTokenId);
                m.TicketHash = context.SerializeTicket(); 
                m.IssuedUtc = now;
                m.ExpiresUtc = till;
            });
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var hashedTokenId = _cryptographer.ComputeHash(context.Token);

            var refreshToken = _tokenStoreViewRepository.GetByKey(Property.Of<TokenStoreView>(x => x.TokenHash), hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.TicketHash);
                //TODO: Remove refreshtoken
                
            }
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