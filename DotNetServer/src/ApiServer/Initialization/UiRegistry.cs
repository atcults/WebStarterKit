using AutoMapper;
using Core.ReadWrite;
using Core.ReadWrite.Impl;
using StructureMap.Configuration.DSL;
using StructureMap.Web;

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
        }
    }
}