using System.Web;
using Common.Helpers;
using Core.ViewOnly;
using Core.Views;

namespace WebApp.Services.Impl
{
    public class UserSession : IUserSession
    {
        private readonly IViewRepository<AppUserView> _appUserRepository;

        public UserSession(IViewRepository<AppUserView> appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public AppUserView GetCurrentUser()
        {
            if (HttpContext.Current == null) return null;
            var identity = HttpContext.Current.User.Identity;
            return !identity.IsAuthenticated ? null : Formatter.EmailId(identity.Name) ? _appUserRepository.GetByKey(Property.Of<AppUserView>(x => x.Email), identity.Name)
               : _appUserRepository.GetByKey(Property.Of<AppUserView>(x => x.Mobile), identity.Name);
        }
    }
}