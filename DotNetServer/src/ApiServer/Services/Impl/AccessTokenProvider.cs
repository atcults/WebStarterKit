using System;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Service;
using Core.ViewOnly;
using Core.Views;
using Microsoft.Owin.Security.Infrastructure;
using NSBus.Dto.Commands;
using NSBus.Dto.Messages;
using NServiceBus;

namespace WebApp.Services.Impl
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly IViewRepository<TokenStoreView> _tokenStoreViewRepository;
        private readonly ICryptographer _cryptographer;
        private readonly IBus _bus;

        public AccessTokenProvider(IViewRepository<TokenStoreView> tokenStoreViewRepository, IBus bus, ICryptographer cryptographer)
        {
            _bus = bus;
            _cryptographer = cryptographer;
            _tokenStoreViewRepository = tokenStoreViewRepository;
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];
            var tokenId = Guid.Parse(context.Ticket.Properties.Dictionary["as:token_id"]);
            var userId = Guid.Parse(context.Ticket.Properties.Dictionary["as:user_id"]);
            var userName = context.Ticket.Properties.Dictionary["as:user_name"];

            var accessToken = Guid.NewGuid().ToString("n");

            _bus.Send<UserGrantAccessCommand>(m =>
            {
                m.TokenId = tokenId;
                m.Client = clientid;
                m.UserId = userId;
                m.UserName = userName;
                m.AccessTokenHash = _cryptographer.ComputeHash(accessToken);
                m.TimeUtc = SystemTime.Now();
                m.ProtectedTicket = context.SerializeTicket();
            });

          //   .Register(asyncResult =>
            //   {
            //       var completionResult = asyncResult.AsyncState as CompletionResult;
            //       if (completionResult == null || completionResult.Messages.Length <= 0) return;
            //       foreach (var msg in completionResult.Messages)
            //       {
                           
            //       }
            //       // Always expecting one IMessage as reply
            //       //var response = completionResult.Messages[0];
            //   }, null);

            //synchronousHandle.AsyncWaitHandle.WaitOne();
           // .Register(completionResult => (GenericResponseMessage) completionResult.Messages[0]);

            context.SetToken(accessToken);

            return Task.FromResult<object>(null);
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var tokenHash = _cryptographer.ComputeHash(context.Token);
            var tokenStoreView = _tokenStoreViewRepository.GetByKey(Property.Of<TokenStoreView>(x => x.AccessTokenHash), tokenHash);

            if (tokenStoreView != null)
            {
                context.DeserializeTicket(tokenStoreView.AccessTicket);
                
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

    class DataResponseMessageHandler : IHandleMessages<GenericResponseMessage>
    {
        public void Handle(GenericResponseMessage message)
        {
            Console.WriteLine("Response received with description: {0}", message.IsSuccess);
        }
    }
}