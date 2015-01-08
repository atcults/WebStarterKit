using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace WebApp.Services
{
    public interface IAuthorizationServerProvider : IOAuthAuthorizationServerProvider
    {
        
    }

    public interface IBearerAuthenticationProvider : IOAuthBearerAuthenticationProvider
    {
        
    }

    public interface IRefreshTokenProvider : IAuthenticationTokenProvider
    {
        
    }

    public interface IAccessTokenProvider : IAuthenticationTokenProvider
    {

    }
}