using Core.Commands;
using Core.ReadWrite;
using Core.ReadWrite.Impl;
using Core.ViewOnly;
using Core.ViewOnly.Impl;
using StructureMap.Configuration.DSL;

namespace Core
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            Scan(x =>
            {
                x.AssemblyContainingType<ICommand>();
                x.WithDefaultConventions();
                x.ConnectImplementationsToTypesClosing(typeof (ICommandProcessor<>));
            });

            For(typeof (IRepository<>)).Add(typeof (Repository<>));
            For(typeof (IViewRepository<>)).Add(typeof (ViewRepository<>));
        }

    }
}