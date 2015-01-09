using System;
using System.Net.Http;
using Common.Base;
using Dto.ApiRequests.LeadForms;
using Dto.ApiResponses;
using NSBus.Dto.Commands;
using NServiceBus;
using WebApp.ModelService;

namespace WebApp.Controllers
{
    public class AnonymousLeadController : SmartApiController
    {
        private readonly IModelService _modelService;
        public IBus Bus { get; set; }

        public AnonymousLeadController(IModelService modelService)
        {
            _modelService = modelService;
        }

        public HttpResponseMessage Post(AddAnonymousLeadForm form)
        {            
            var validationResult = new ValidationResult();

            if (!_modelService.CheckAnonymousLeadForm(form, validationResult)) return Content(new WebApiResponseBase(validationResult));

            Bus.Send<AddLeadCommand>(x =>
            {
                x.Name = form.Name;
                x.CompanyName = form.CompanyName;
                x.Email = form.Email;
                x.Phone = form.Phone;
                x.Description = form.Description;
                x.CreatedOn = DateTime.Now;
            });

            var response = new WebApiResponseBase();

            return Content(response);

        }
    }
}
