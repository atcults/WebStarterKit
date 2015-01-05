using System;
using System.Security.Authentication;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Common.Enumerations;
using Common.Helpers;
using Core.ViewOnly;
using Core.Views;
using Dto.ApiRequests.AppUserForms;
using NSBus.Dto.Commands;
using NServiceBus;

namespace WebApp.Services.Impl
{
    public class UserSession : IUserSession
    {
        private const int PasswordValidPeriod = 6; // Months
        private const string UserDoesNotExistMssg = "User does not exist.";
        private const string LockedAccountMssg = "Your account has been locked due to too many invalid log-on attempts. Please reset your password by clicking \"Forgot Password?\"";
        private const string ExpiredPasswordMssg = "Your current password has expired. Please reset your password by clicking \"Forgot Password?\"";
        private const string ExpiringPasswordMssgTemplate = "Your password will expire in {0} day(s). Please change your password.";


        private readonly IViewRepository<AppUserView> _appUserRepository;
        private readonly IAuthenticationService _authenticationService;

        private readonly IBus _bus;


        public UserSession(IViewRepository<AppUserView> appUserRepository, IAuthenticationService authenticationService, IBus bus)
        {
            _appUserRepository = appUserRepository;
            _authenticationService = authenticationService;
            _bus = bus;
        }

        public AppUserView GetCurrentUser()
        {
            if (HttpContext.Current == null) return null;
            var identity = HttpContext.Current.User.Identity;
            return !identity.IsAuthenticated ? null : GetUserByEmailOrMobile(identity.Name);
        }

        public AppUserView GetAnonymousUser()
        {
            var currentUser = _appUserRepository.GetByKey(Property.Of<AppUserView>(x => x.Name), "guest");

            if (currentUser == null)
                throw new InvalidCredentialException("That user doesn't exist or is not valid.");

            return currentUser;
        }

        private AppUserView GetUserByEmailOrMobile(string username)
        {
            return Formatter.EmailId(username)
               ? _appUserRepository.GetByKey(Property.Of<AppUserView>(x => x.Email), username)
               : _appUserRepository.GetByKey(Property.Of<AppUserView>(x => x.Mobile), username);

        }

        public bool Login(LoginForm form, ref string message)
        {
            var user = GetUserByEmailOrMobile(form.Username);

            if (user == null)
            {
                message = UserDoesNotExistMssg;
                return false;
            }

            if (user.UserStatus != null && user.UserStatus.Equals(UserStatus.Disabled))
            {
                message = LockedAccountMssg;
                return false;
            }

            if (_authenticationService.PasswordMatches(user, form.Password))
            {
                if (user.LastPasswordChangedDate.HasValue)
                {
                    var passwordExpirationDate = user.LastPasswordChangedDate.Value.AddMonths(PasswordValidPeriod);
                    var days = (passwordExpirationDate - SystemTime.Now()).Days;
                    if (days <= 0)
                    {
                        message = ExpiredPasswordMssg;
                        return false;
                    }

                    message = string.Format(ExpiringPasswordMssgTemplate, days);
                }

                _bus.Send<UserLoginPostCommand>(m =>
                {
                    m.UserId = user.Id;
                    m.IsAuthSuccess = true;
                    m.LoginDateTime = DateTime.Now;
                });

                FormsAuthentication.SetAuthCookie(form.Username, false);
                return true;
            }

            message = "Invalid UserName/Password";

            _bus.Send<UserLoginPostCommand>(m =>
            {
                m.UserId = user.Id;
                m.IsAuthSuccess = false;
                m.LoginDateTime = DateTime.Now;
            });

            return false;
        }

        public void LogOff()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
        }
    }
}