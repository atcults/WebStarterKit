using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using AutoMapper;
using Core.ViewOnly;
using Core.Views;
using Dto.ApiResponses;
using Dto.ApiResponses.HealthResponses;
using NServiceBus;

namespace WebApp.Controllers
{
    public class HealthController : SmartApiController
    {
        public IBus Bus { get; set; }

        private readonly IHealthViewRepository _healthViewRepository;

        public HealthController(IHealthViewRepository healthViewRepository)
        {
            _healthViewRepository = healthViewRepository;
        }

        public HttpResponseMessage Get()
        {
            var healthView = _healthViewRepository.HeathData();

            var response = new HealthResponse
            {
                Value = healthView.Select(Mapper.Map<HealthView, HealthResponse.HealthLine>).ToArray()
            };

            return Content(response);
        }

        public HttpResponseMessage Post()
        {
            var proc = Process.GetCurrentProcess();

            //Bus.Send("NSBus.Server", new HealthDataAdded
            //{
            //    Type = "Physical Memory",
            //    Value = proc.PrivateMemorySize64.ToString(CultureInfo.InvariantCulture)
            //});
            
            //Bus.Send("NSBus.Server", new HealthDataAdded
            //{
            //    Type = "Virtual Memory",
            //    Value = proc.VirtualMemorySize64.ToString(CultureInfo.InvariantCulture)
            //});
            
            //Bus.Send("NSBus.Server", new HealthDataAdded
            //{
            //    Type = "Non Paged System Memory",
            //    Value = proc.NonpagedSystemMemorySize64.ToString(CultureInfo.InvariantCulture)
            //});
            
            //Bus.Send("NSBus.Server", new HealthDataAdded
            //{
            //    Type = "Paged System Memory",
            //    Value = proc.PagedSystemMemorySize64.ToString(CultureInfo.InvariantCulture)
            //});
            
            //Bus.Send("NSBus.Server", new HealthDataAdded
            //{
            //    Type = "Peak Paged Memory",
            //    Value = proc.PeakPagedMemorySize64.ToString(CultureInfo.InvariantCulture)
            //});

            //Bus.Send("NSBus.Server", new HealthDataAdded
            //{
            //    Type = "Peak Virtual Memory",
            //    Value = proc.PeakVirtualMemorySize64.ToString(CultureInfo.InvariantCulture)
            //});

            var response = new WebApiResponseBase();
            
            return Content(response);
        }
    }
}
