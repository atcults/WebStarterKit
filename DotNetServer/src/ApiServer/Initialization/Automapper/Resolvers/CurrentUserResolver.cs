using AutoMapper;
using WebApp.Services;

namespace WebApp.Initialization.Automapper.Resolvers
{
    public class CurrentUserResolver : IValueResolver
    {
        private readonly IUserSession _userSession;

        public CurrentUserResolver(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public ResolutionResult Resolve(ResolutionResult source)
        {
            return source.New(_userSession.GetCurrentUser().Id);
        }
    }
}