using AutoMapper;
using Core.ReadWrite;
using Core.ReadWrite.Impl;
using StructureMap.Configuration.DSL;

namespace NSBus.Server.Initialization
{
    public class NBusServerRegistry : Registry
    {
        public NBusServerRegistry()
        {
            Scan(x => x.AssemblyContainingType<IMappingEngine>());

            For<IUnitOfWork>().Use<UnitOfWork>();

            For<IMappingEngine>().Use(ctx => Mapper.Engine);
        }
    }
}