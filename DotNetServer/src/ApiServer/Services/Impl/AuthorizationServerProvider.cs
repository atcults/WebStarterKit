using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Enumerations;
using Common.Helpers;
using Common.Service;
using Core.ViewOnly;
using Core.Views;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace WebApp.Services.Impl
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IViewRepository<AppClientView> _appClientViewRepository; 
        private readonly IViewRepository<AppUserView> _appUserViewRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICryptographer _cryptographer;

        public AuthorizationServerProvider(IViewRepository<AppUserView> appUserViewRepository, IAuthenticationService authenticationService, ICryptographer cryptographer, IViewRepository<AppClientView> appClientViewRepository)
        {
            _appUserViewRepository = appUserViewRepository;
            _authenticationService = authenticationService;
            _cryptographer = cryptographer;
            _appClientViewRepository = appClientViewRepository;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.Validated();
                // context.SetError("invalid_clientId", "ClientId should be sent."); // To restrict using clientId use this line.
                return Task.FromResult<object>(null);
            }

            var client = _appClientViewRepository.GetByKey(Property.Of<AppClientView>(x=>x.Name), context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));

                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType.Equals(ApplicationType.NativeConfidential))
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");

                    return Task.FromResult<object>(null);
                }

                if (client.Secret != _cryptographer.ComputeHash(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");

                    return Task.FromResult<object>(null);
                }
            }

            if (!client.IsActive)
            {
                context.SetError("invalid_clientId", "Client is inactive.");

                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var user =  _appUserViewRepository.GetByKey(Property.Of<AppUserView>(x=>x.Name), context.UserName);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (_authenticationService.PasswordMatches(user, context.Password))
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", user.ProfileName));

            var props = CreateProperties(context.UserName, context.ClientId);

            var ticket = new AuthenticationTicket(identity, props);

            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");

                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);

            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string clientId = null)
        {
            return new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "as:client_id", clientId ?? string.Empty },
                    { "userName", userName }
                });
        }

    }
}