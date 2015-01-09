using System.Net.Http;
using AutoMapper;
using Common.Service.Impl;
using Common.SystemSettings;
using Dto.ApiRequests.ConfigForms;
using Dto.ApiResponses;
using Dto.ApiResponses.ConfigResponses;

namespace WebApp.Controllers
{
    public class SmsConfigController : SmartApiController
    {
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
