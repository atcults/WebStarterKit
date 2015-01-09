using System;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Core.Commands.LeadCommands;
using Dto.ApiRequests;
using Dto.ApiRequests.LeadForms;
using NServiceBus;
using WebApp.ModelService;

namespace WebApp.Controllers
{
    public class LeadController : SmartApiController
    {
        public IBus Bus { get; set; }
        private readonly ILeadModelService _leadModelService;

        public LeadController(ILeadModelService leadModelService)
        {
            _leadModelService = leadModelService;
        }

        public HttpResponseMessage Get([FromUri] SearchSpecification form)
        {
            var response = _leadModelService.GetPageBySpecification(form);
            return Content(response);
        }
        
        public HttpResponseMessage Post(AddLeadForm form)
        {
            var command = Mapper.Map<AddLeadForm, AddLead>(form);
            return ExecuteCommand(command);
        }

        public HttpResponseMessage Put(UpdateLeadForm form)
        {
            var command = Mapper.Map<UpdateLeadForm, UpdateLead>(form);
            return ExecuteCommand(command);
        }

        public HttpResponseMessage Delete(Guid id)
        {
            var command = new DeleteLead(id);
            return ExecuteCommand(command);
        }
    }
}
