using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Dto.ApiRequests;
using Dto.ApiResponses;
using NSBus.Dto.Commands;
using NServiceBus;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize]
    public class ReportRequestController : SmartApiController
    {
        public IBus Bus { get; set; }

        public ReportRequestController(IUserSession userSession, IMappingEngine mappingEngine)
            : base(userSession, mappingEngine)
        {
        }

        public HttpResponseMessage Post(ReportRequestForm form)
        {
            var response = new WebApiResponseBase();
            
            Bus.Send<GenerateSimpleExportCommand>(c =>
            {
                c.UserId = GetCurrentUser().Id;
                c.SearchSpecification = SimpleSearch.FromDepricated(form);
                c.ViewType = form.EntityTypeValue;
            });

            return Content(response);
        }
    }
}