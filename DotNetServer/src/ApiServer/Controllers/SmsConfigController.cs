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
    public class SmsConfigController : SmartApiController
    {

        public SmsConfigController(IUserSession userSession, IMappingEngine mappingEngine)
            : base(userSession, mappingEngine)
        {
        }

        public HttpResponseMessage Get()
        {
            var smsConfig = ConfigProvider.GetSmsConfig();
            var response = Mapper.Map<SmsConfig, SmsConfigResponse>(smsConfig);
            return Content(response);
        }

        public HttpResponseMessage Put(SmsConfigForm form)
        {
            var config = Mapper.Map<SmsConfigForm, SmsConfig>(form);
            ConfigProvider.SetSmsConfig(config, true);

            var response = new WebApiResponseBase();
            return Content(response);
        }
    }
}
