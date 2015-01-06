using System;
using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using WebApp.Filters;
using WebApp.Formatters;

[assembly: OwinStartup(typeof(WebApp.Initialization.OwinStartup))]

namespace WebApp.Initialization
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            
            ClientEndPoint.Initialize();
            
            RegisterWebApi();

            ConfigureOAuth(app);
            
          //  KeepAlive.Start();
        }

        private static void RegisterWebApi()
        {
            var configuration = GlobalConfiguration.Configuration;
            configuration.MapHttpAttributeRoutes();
            configuration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            
            configuration.Filters.Add(new DeviceAuthenticationAttribute()); 
            
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
            };

            configuration.Formatters.Insert(0, new JsonNetFormatter(jsonSettings));

            configuration.Services.Replace(typeof(IHttpControllerActivator), new SmartHttpControllerActivator());

            configuration.EnsureInitialized();
        }

        private static void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = ClientEndPoint.Container.GetInstance<IOAuthAuthorizationServerProvider>(),
                RefreshTokenProvider = ClientEndPoint.Container.GetInstance<IAuthenticationTokenProvider>()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
