using System;
using AutoMapper;
using Common.Base;
using Core.Commands.NewsLetterCommands;
using Dto.ApiResponses;
using NSBus.Dto.Commands;
using NServiceBus;
using WebApp.ModelService;
using WebApp.Services;
using System.Net.Http;
using Dto.ApiRequests.NewsLetter;

namespace WebApp.Controllers
{
	public class NewsLetterController : SmartApiController
	{
        private readonly IModelService _modelService;
        public IBus Bus { get; set; }

		public NewsLetterController(IUserSession userSession, IMappingEngine mappingEngine, IModelService modelService) : base(userSession, mappingEngine)
		{
		    _modelService = modelService;
		}

	    public HttpResponseMessage Post(AddNewsLetterForm form)
		{
            var validationResult = new ValidationResult();

	        if (!_modelService.CheckNewsLetterForm(form, validationResult))
	        {
	            return Content(new WebApiResponseBase(validationResult));
	        }

            Bus.Send<AddNewsLetterCommand>(x =>
            {
                x.Email = form.Email;
                x.InsertedDate = DateTime.Now;
            });

            var response = new WebApiResponseBase();

            return Content(response);
		}

		public HttpResponseMessage Put(UpdateNewsLetterForm form)
		{
			var command = Mapper.Map<UpdateNewsLetterForm, UpdateNewsLetter>(form);
			return ExecuteCommand(command);
		}

		public HttpResponseMessage Delete(Guid id)
		{
			var command = new DeleteNewsLetter(id);
			return ExecuteCommand(command);
		}
	}
}
