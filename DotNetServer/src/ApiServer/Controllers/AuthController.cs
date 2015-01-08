using System.Net.Http;
using AutoMapper;
using Common.Service;
using Core.Commands.AppUserCommands;
using Dto.ApiRequests.AppUserForms;
using Dto.ApiResponses;
using NSBus.Dto.Commands;
using NServiceBus;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AuthController : SmartApiController
    {
        private readonly ICryptographer _cryptographer;
        private readonly IBus _bus;

        public AuthController(IUserSession userSession, IMappingEngine mappingEngine, ICryptographer cryptographer, IBus bus) : base(userSession, mappingEngine)
        {
            _cryptographer = cryptographer;
            _bus = bus;
        }

        public HttpResponseMessage Put(ChangePasswordForm form)
        {
            var response = new WebApiResponseBase();

            var user = GetCurrentUser();

            var success = user != null && _cryptographer.GetPasswordHash(form.OldPassword, user.PasswordSalt).Equals(user.PasswordHash);

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

        public HttpResponseMessage Delete(string token)
        {
            var response = new WebApiResponseBase();

            _bus.Send<RemoveTokenCommand>(c =>
            {
                c.TokenHash = _cryptographer.ComputeHash(token);
            });
            
            return Content(response);
        }
    }
}