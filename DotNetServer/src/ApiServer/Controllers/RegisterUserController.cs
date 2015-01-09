using System.Net.Http;
using Common.Base;
using Dto.ApiRequests.AppUserForms;
using Dto.ApiResponses;
using NSBus.Dto.Commands;
using NServiceBus;
using WebApp.ModelService;

namespace WebApp.Controllers
{
    public class RegisterUserController : SmartApiController
    {
        private readonly IModelService _modelService;
        public IBus Bus { get; set; }
        public RegisterUserController(IModelService modelService)
        {
            _modelService = modelService;
        }

        public HttpResponseMessage Post(RegistrationForm form)
        {
            var errorResult = new ValidationResult();

            if (!_modelService.CheckAppUserRegistrationForm(form, errorResult))
                return Content(new WebApiResponseBase(errorResult));

            Bus.Send<RegisterNewAppUserCommand>(x =>
            {
                x.Name = form.Name;
                x.Email = form.Email;
                x.Mobile = form.Mobile;
            });

            var response = new WebApiResponseBase();

            return Content(response);
        }
    }
}