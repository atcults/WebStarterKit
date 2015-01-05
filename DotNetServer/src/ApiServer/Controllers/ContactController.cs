using System;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Core.Commands.ContactCommands;
using Dto.ApiRequests;
using Dto.ApiRequests.ContactForms;
using NServiceBus;
using WebApp.ModelService;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class ContactController : SmartApiController
    {
        public IBus Bus { get; set; }
        private readonly IContactModelService _contactModelService;

        public ContactController(IUserSession userSession, IMappingEngine mappingEngine, IContactModelService contactModelService)
            : base(userSession, mappingEngine)
        {
            _contactModelService = contactModelService;
        }

        public HttpResponseMessage Get([FromUri] SearchSpecification form)
        {
            var response = _contactModelService.GetPageBySpecification(form);
            return Content(response);
        }

        public HttpResponseMessage Post(AddContactForm form)
        {
            var command = Mapper.Map<AddContactForm, AddContact>(form);
            return ExecuteCommand(command);
        }

        public HttpResponseMessage Put(UpdateContactForm form)
        {
            var command = Mapper.Map<UpdateContactForm, UpdateContact>(form);
            return ExecuteCommand(command);
        }

        public HttpResponseMessage Delete(Guid id)
        {
            var command = new DeleteContact(id);
            return ExecuteCommand(command);
        }
    }
}
