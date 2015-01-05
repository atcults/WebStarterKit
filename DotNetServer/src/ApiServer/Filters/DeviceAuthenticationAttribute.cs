using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace WebApp.Filters
{
    public class DeviceAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get { return true; } }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // 1. Look for credentials in the request.
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            // 2. If there are no credentials, do nothing.
            if (authorization == null)
            {
                return Task.FromResult(0);
            }

            // 3. If there are credentials but the filter does not recognize the 
            //    authentication scheme, do nothing.
            if (authorization.Scheme != "Basic")
            {
                return Task.FromResult(0);
            }

            // 4. If there are credentials that the filter understands, try to validate them.
            // 5. If the credentials are bad, set the error result.
            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return Task.FromResult(0);
            }

            var encoding = Encoding.GetEncoding("iso-8859-1");
            var credentials = encoding.GetString(Convert.FromBase64String(authorization.Parameter));
            var parts = credentials.Split(':');
            var userId = parts[0].Trim();
            var password = parts[1].Trim();

            if (string.IsNullOrEmpty(userId))
            {
                context.ErrorResult = new AuthenticationFailureResult("User does not supplied", request);
                return Task.FromResult(0);
            }

            if (!userId.Equals(password)) // Just a dumb check
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
                return Task.FromResult(0);
            }

            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "info@sanelib.com")
                };

            var id = new ClaimsIdentity(claims, "Basic");
            var principal = new ClaimsPrincipal(new[] { id });
            context.Principal = principal;

            //IPrincipal principal = new ClaimsPrincipal(new FormsIdentity(new FormsAuthenticationTicket("admin", true, 60)));
            //context.Principal = principal;

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Basic");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }

    }
}