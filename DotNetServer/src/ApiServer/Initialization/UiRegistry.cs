using AutoMapper;
using Core.ReadWrite;
using Core.ReadWrite.Impl;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using StructureMap.Configuration.DSL;
using StructureMap.Web;
using WebApp.Services.Impl;

namespace WebApp.Initialization
{
    public class UiRegistry : Registry
    {
        public UiRegistry()
        {
            Scan(x => x.AssemblyContainingType<IMappingEngine>());

            For<IUnitOfWork>()
                .Singleton()
                .HybridHttpOrThreadLocalScoped()
                .Use<UnitOfWork>();

            For<IMappingEngine>().Use(ctx => Mapper.Engine);

            For<IOAuthAuthorizationServerProvider>().Use<AuthorizationServerProvider>().Singleton();

            For<IAuthenticationTokenProvider>().Use<RefreshTokenProvider>().Singleton();
        }
    }
}