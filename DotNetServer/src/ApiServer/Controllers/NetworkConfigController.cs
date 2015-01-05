using System.Net.Http;
using AutoMapper;
using Common.Service.Impl;
using Common.SystemSettings;
using Dto.ApiRequests.ConfigForms;
using Dto.ApiResponses;
using Dto.ApiResponses.ConfigResponses;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class NetworkConfigController : SmartApiController
    {
        public NetworkConfigController(IUserSession userSession, IMappingEngine mappingEngine) : base(userSession, mappingEngine)
        {
        }

        public HttpResponseMessage Get()
        {
            var networkConfig = ConfigProvider.GetNetworkConfig();
            var response = Mapper.Map<NetworkConfig, NetworkConfigResponse>(networkConfig);
            return Content(response);
        }

        public HttpResponseMessage Put(NetworkConfigForm form)
        {
            var config = Mapper.Map<NetworkConfigForm, NetworkConfig>(form);
            ConfigProvider.SetNetworkConfig(config, true);

            var response = new WebApiResponseBase();
            return Content(response);
        }
    }
}
