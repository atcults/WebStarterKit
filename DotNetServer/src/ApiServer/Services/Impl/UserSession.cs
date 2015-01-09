using System.Web;
using Common.Helpers;
using Core.ViewOnly;
using Core.Views;

namespace WebApp.Services.Impl
{
    public class UserSession : IUserSession
    {
        private readonly IViewRepository<AppUserView> _appUserViewRepository;

        public UserSession(IViewRepository<AppUserView> appUserViewRepository)
        {
            _appUserViewRepository = appUserViewRepository;
        }

        public AppUserView GetCurrentUser()
        {
            if (HttpContext.Current == null) return null;
            var identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated) return null;
            return Formatter.EmailId(identity.Name) ? _appUserViewRepository.GetByKey(Property.Of<AppUserView>(x => x.Email), identity.Name) : _appUserViewRepository.GetByKey(Property.Of<AppUserView>(x => x.Mobile), identity.Name);
        }
    }
}