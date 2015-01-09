using System;
using System.Net.Http;
using System.Web.Http;
using Core.Commands.TaskLogCommands;
using Dto.ApiRequests;
using NServiceBus;
using WebApp.ModelService;

namespace WebApp.Controllers
{
    public class TaskLogController : SmartApiController
    {
        private readonly ITaskLogModelService _taskLogModelService;
        public IBus Bus { get; set; }
        public TaskLogController(ITaskLogModelService taskLogModelService)
        {
            _taskLogModelService = taskLogModelService;
        }

        [ActionName("BySpecification")]
        public HttpResponseMessage Get([FromUri]SearchSpecification specification)
        {
            var response = _taskLogModelService.GetPageBySpecification(specification);
            return Content(response);
        }

        public HttpResponseMessage Delete(Guid id)
        {
            var command = new DeleteTaskLog(id);
            return ExecuteCommand(command);
        }
    }
}