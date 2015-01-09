using System.Net.Http;
using AutoMapper;
using Common.Service.Impl;
using Common.SystemSettings;
using Dto.ApiRequests.ConfigForms;
using Dto.ApiResponses;
using Dto.ApiResponses.ConfigResponses;

namespace WebApp.Controllers
{
    public class EmailConfigController : SmartApiController
    {
        public HttpResponseMessage Get()
        {
            var emailConfig = ConfigProvider.GetEmailConfig();
            var response = Mapper.Map<EmailConfig, EmailConfigResponse>(emailConfig);
            return Content(response);
        }

        public HttpResponseMessage Put(EmailConfigForm form)
        {
            var response = new WebApiResponseBase();
            var config = Mapper.Map<EmailConfigForm, EmailConfig>(form);
            ConfigProvider.SetEmailConfig(config, true);
            return Content(response);
        }
    }
}
