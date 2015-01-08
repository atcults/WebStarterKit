using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using WebApp.Filters;
using WebApp.Formatters;
using WebApp.Initialization;
using WebApp.Services;

[assembly: OwinStartup(typeof(OwinStartup))]

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
                Provider = ClientEndPoint.Container.GetInstance<IAuthorizationServerProvider>(),
                AccessTokenProvider = ClientEndPoint.Container.GetInstance<IAccessTokenProvider>(),
                RefreshTokenProvider = ClientEndPoint.Container.GetInstance<IRefreshTokenProvider>()
            };

            // TokenHash Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            // Token validator
            var bearerAuthenticationOptions = new OAuthBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                Provider = ClientEndPoint.Container.GetInstance<IBearerAuthenticationProvider>(),
                AccessTokenProvider = ClientEndPoint.Container.GetInstance<IAccessTokenProvider>()
            };

            app.UseOAuthBearerAuthentication(bearerAuthenticationOptions);

            app.UseCors(CorsOptions.AllowAll);
        }
    }
}
