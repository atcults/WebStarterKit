using System.Net.Http;
using AutoMapper;
using Common.Extensions;
using Common.Helpers;
using Common.Service;
using Core.Domain.Model;
using Core.ViewOnly;
using Core.Views;
using Dto.ApiRequests.AppUserForms;
using Dto.ApiResponses;
using NSBus.Dto.Commands;
using NServiceBus;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AuthRecoveryController : SmartApiController
    {
        private readonly IViewRepository<AppUserView> _appUserViewRepository;
        private readonly ICryptographer _cryptographer;

        private readonly IBus _bus;

        public AuthRecoveryController(IUserSession userSession, IMappingEngine mappingEngine, IViewRepository<AppUserView> appUserViewRepository, ICryptographer cryptographer, IBus bus)
            : base(userSession, mappingEngine)
        {
            _appUserViewRepository = appUserViewRepository;
            _cryptographer = cryptographer;
            _bus = bus;
        }

        public HttpResponseMessage Post(RecoverPasswordForm form)
        {
            var response = new WebApiResponseBase();

            if (string.IsNullOrEmpty(form.UserNameOrEmailAddress))
            {
                response.AddError("UserName or EmailAddress", "Empty or Invalid");
                return Content(response);
            }

            var property = form.UserNameOrEmailAddress.Contains("@")
                ? Property.Of<AppUserView>(x => x.Email)
                : Property.Of<AppUserView>(x => x.Mobile);

            var member = _appUserViewRepository.GetByKey(property, form.UserNameOrEmailAddress);

            if (member == null)
            {
                response.AddError("UserName or EmailAddress", "is not correct");
                return Content(response);
            }

            _bus.Send<PasswordRecoveryTokenCommand>(c =>
            {
                c.UserId = member.Id;
            });

            return Content(response);
        }

        public HttpResponseMessage Put(ResetPasswordForm form)
        {
            var response = new WebApiResponseBase();

            if (form.Token != null || form.NewPassword != null || form.ConfirmPassword != null)
            {
                if (!form.Token.IsNotEmpty())
                    response.AddError("TokenHash", "is empty.");
                else if (form.Token.IsNotEmpty())
                {
                    var encryptToken = _cryptographer.ComputeHash(form.Token);
                    var member = _appUserViewRepository.GetByKey(Property.Of<AppUser>(x => x.PasswordRetrievalToken), encryptToken);

                    if (member == null)
                        response.AddError("TokenHash", "is not valid");
                    else
                    {
                        if (!form.NewPassword.IsNotEmpty() || !form.ConfirmPassword.IsNotEmpty())
                            response.AddError("NewPassword and ConfirmPassword", "are Required.");
                        else if (form.NewPassword != null && form.NewPassword.Length < 8)
                            response.AddError("NewPassword", "Must be 8 characters long");
                        else if (!Formatter.HasAtLeast1Lowercase(form.NewPassword))
                            response.AddError("NewPassword", "Must contains at least one LowerCase letter");
                        else if (!Formatter.HasAtLeast1Number(form.NewPassword))
                            response.AddError("NewPassword", "Must contains at least one Number");
                        else if (!Formatter.HasAtLeast1SpecialChar(form.NewPassword))
                            response.AddError("NewPassword", "Must contains at least one Special Character from  : _ # $ % ");
                        else if (!Formatter.HasAtLeast1Uppercase(form.NewPassword))
                            response.AddError("NewPassword", "Must contains at least one UpperCase letter");
                        else if (form.NewPassword != form.ConfirmPassword)
                            response.AddError("Newpassword and confirmpassword", "are not same.");
                        else
                        {
                            _bus.Send<ResetPasswordCommand>(c =>
                            {
                                c.Token = form.Token;
                                c.NewPassword = form.NewPassword;
                                c.ConfirmPassword = form.ConfirmPassword;
                            });

                            return Content(response);
                        }
                    }
                }
            }
            response.AddError("TokenHash or NewPassword or ConfirmPassword", "is Empty or Invalid");
            return Content(response);
        }
    }
}