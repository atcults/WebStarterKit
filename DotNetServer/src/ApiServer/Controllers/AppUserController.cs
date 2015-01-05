using System;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Core.Commands.AppUserCommands;
using Dto.ApiRequests;
using Dto.ApiRequests.AppUserForms;
using NServiceBus;
using WebApp.ModelService;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AppUserController : SmartApiController
    {
        private readonly IAppUserModelService _appUserModelService;
        public IBus Bus { get; set; }
        public AppUserController(IUserSession userSession, IMappingEngine mappingEngine, IAppUserModelService appUserModelService) : base(userSession, mappingEngine)
        {
            _appUserModelService = appUserModelService;
        }

        [Authorize]
        public HttpResponseMessage Get([FromUri] SearchSpecification form)
        {
            var response = _appUserModelService.GetPageBySpecification(form);
            return Content(response);
        }

        public HttpResponseMessage Post(AddAppUserForm form)
        {
            var command = Mapper.Map<AddAppUserForm, AddAppUser>(form);
            return ExecuteCommand(command);
        }

        [Authorize]
        public HttpResponseMessage Put(UpdateAppUserForm form)
        {
            var command = MappingEngine.Map<UpdateAppUserForm, UpdateAppUser>(form);
            return ExecuteCommand(command);
        }

        [Authorize]
        public HttpResponseMessage Delete(Guid id)
        {
            var command = new DeleteAppUser(id);
            return ExecuteCommand(command);
        }
    }
}