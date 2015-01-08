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
using NSBus.Dto.Commands;
using NServiceBus;

namespace WebApp.Services.Impl
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider, IAuthorizationServerProvider
    {
        private readonly IViewRepository<AppClientView> _appClientViewRepository;
        private readonly ICryptographer _cryptographer;
        private readonly IViewRepository<AppUserView> _appUserViewRepository;
        private readonly IBus _bus;

        private const int PasswordValidPeriod = 6; // Months
        private const string UserDoesNotExistMssg = "User does not exist.";
        private const string LockedAccountMssg = "Your account has been locked due to too many invalid log-on attempts. Please reset your password by clicking \"Forgot Password?\"";
        private const string ExpiredPasswordMssg = "Your current password has expired. Please reset your password by clicking \"Forgot Password?\"";
        private const string ExpiringPasswordMssgTemplate = "Your password will expire in {0} day(s). Please change your password.";


        public AuthorizationServerProvider(ICryptographer cryptographer, IViewRepository<AppClientView> appClientViewRepository, IBus bus, IViewRepository<AppUserView> appUserViewRepository)
        {
            _cryptographer = cryptographer;
            _appClientViewRepository = appClientViewRepository;
            _bus = bus;
            _appUserViewRepository = appUserViewRepository;
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
                //context.Validated();
                context.SetError("invalid_clientId", "ClientId should be sent."); // To restrict using clientId use this line.
                return Task.FromResult<object>(null);
            }

            var client = _appClientViewRepository.GetByKey(Property.Of<AppClientView>(x => x.Name), context.ClientId);

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

            context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var user = Formatter.EmailId(context.UserName) ? _appUserViewRepository.GetByKey(Property.Of<AppUserView>(x => x.Email), context.UserName) : _appUserViewRepository.GetByKey(Property.Of<AppUserView>(x => x.Mobile), context.UserName);

            if (user == null)
            {
                context.SetError("invalid_grant", UserDoesNotExistMssg);
                return Task.FromResult<object>(null);
            }

            var message = string.Empty;
            if (!CheckGrant(user, context.Password, ref message))
            {
                context.SetError("invalid_grant", message);

                _bus.Send<UserLoginFailedCommand>(m =>
                {
                    m.UserId = user.Id;
                });

                return Task.FromResult<object>(null);
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, user.Id.ToString()));

            var tokenId = GuidComb.New();
            
            var propserties = new AuthenticationProperties(new Dictionary<string, string>
            {
                { "as:client_id", context.ClientId },
                { "as:token_id", tokenId.ToString() },
                { "as:user_id", user.Id.ToString()},
                { "as:user_name", user.Name }
            });

            var ticket = new AuthenticationTicket(identity, propserties);

            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

        private bool CheckGrant(AppUserView user, string password, ref string message)
        {
            if (user.UserStatus != null && user.UserStatus.Equals(UserStatus.Disabled))
            {
                message = LockedAccountMssg;
                return false;
            }

            if (!_cryptographer.GetPasswordHash(password, user.PasswordSalt).Equals(user.PasswordHash))
            {
                message = UserDoesNotExistMssg;
                return false;
            }

            if (!user.LastPasswordChangedDate.HasValue) return true;
            
            var passwordExpirationDate = user.LastPasswordChangedDate.Value.AddMonths(PasswordValidPeriod);
            var days = (passwordExpirationDate - SystemTime.Now()).Days;
            if (days <= 0)
            {
                message = ExpiredPasswordMssg;
                return false;
            }

            message = string.Format(ExpiringPasswordMssgTemplate, days);
            
            return true;
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

    }
}