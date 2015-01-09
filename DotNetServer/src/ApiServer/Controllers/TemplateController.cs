using System;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Core.Commands.TemplateCommands;
using Core.ViewOnly;
using Core.ViewOnly.Base;
using Core.Views;
using Dto.ApiRequests;
using Dto.ApiRequests.TemplateForms;
using Dto.ApiResponses;
using Dto.ApiResponses.TemplateResponses;

namespace WebApp.Controllers
{
    public class TemplateController : SmartApiController
    {
        private readonly IViewRepository<TemplateView> _templateViewRepository;

        public TemplateController(IViewRepository<TemplateView> templateViewRepository)
        {
            _templateViewRepository = templateViewRepository;
        }

        public HttpResponseMessage Get([FromUri] SearchSpecification form)
        {
            var templateViewPage = _templateViewRepository.SearchBySpecification(SimpleSearch.FromDepricated(form));
            var response = Mapper.Map<Page<TemplateView>, PageResponse<TemplateLine>>(templateViewPage);

            return Content(response);
        }

        public HttpResponseMessage Post(AddTemplateForm form)
        {
            var command = Mapper.Map<AddTemplateForm, AddTemplate>(form);
            return ExecuteCommand(command);
        }

        public HttpResponseMessage Put(UpdateTemplateForm form)
        {
            var command = Mapper.Map<UpdateTemplateForm, UpdateTemplate>(form);
            return ExecuteCommand(command);
        }

        public HttpResponseMessage Delete(Guid id)
        {
            var command = new DeleteTemplate(id);
            return ExecuteCommand(command);
        }
    }
}
