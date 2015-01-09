using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace WebApp.Services.Impl
{
    public class BearerAuthenticationProvider : IBearerAuthenticationProvider
    {
        // This validates the identity based on the issuer of the claim.
        // The issuer is set in the API endpoint that logs the user in
        public Task RequestToken(OAuthRequestTokenContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization")) return Task.FromResult<object>(null);
            var data = context.Request.Headers.Get("Authorization").Split(' ');
            if (data.Length != 2) return Task.FromResult<object>(null);

            if (data[0].Equals("bearer", StringComparison.InvariantCultureIgnoreCase))
            {
                context.Token = data[1];
            }
            return Task.FromResult<object>(null);
        }

        public Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
           // var claims = context.Ticket.Identity.Claims;
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public Task ApplyChallenge(OAuthChallengeContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}