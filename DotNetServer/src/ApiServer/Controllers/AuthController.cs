using System.Net.Http;
using AutoMapper;
using Core.Commands.AppUserCommands;
using Dto.ApiRequests.AppUserForms;
using Dto.ApiResponses;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AuthController : SmartApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IUserSession userSession, IMappingEngine mappingEngine, IAuthenticationService authenticationService)
            : base(userSession, mappingEngine)
        {
            _authenticationService = authenticationService;
        }

        public HttpResponseMessage Post(LoginForm form)
        {
            var response = new WebApiResponseBase();
            var message = string.Empty;

            if (UserSession.Login(form, ref message))
            {
                return Content(WebApiResponseBase.Create("/", true));
            }

            response.AddError("Username or Password", message);
            return Content(response);
        }

        public HttpResponseMessage Put(ChangePasswordForm form)
        {
            var response = new WebApiResponseBase();

            var user = GetCurrentUser();
            var success = user != null && _authenticationService.PasswordMatches(user, form.OldPassword);

            if (!success)
            {
                response.AddError("OldPassword or NewPassword or ConfirmPassword", "is Invalid");
                return Content(response);
            }

            var command = new ChangePassword
            {
                Id = user.Id,
                OldPassword = form.OldPassword,
                NewPassword = form.NewPassword,
                ConfirmPassword = form.ConfirmPassword,
            };

            return ExecuteCommand(command);
        }
    }
}