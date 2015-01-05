using System;
using Common;
using Core;
using GemBox.Spreadsheet;
using NSBus.Server.Initialization;
using NSBus.Server.Initialization.Automapper;
using NServiceBus;
using StructureMap;
using StructureMap.Graph;

namespace NSBus.Server
{
    public class NBusServerEndPoint : IWantToRunWhenBusStartsAndStops, IConfigureThisEndpoint, AsA_Server
    {
        public readonly static IContainer Container = new Container();

        public void Customize(BusConfiguration configuration)
        {
            Container.Configure(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<CommonRegistry>();
                scan.AssemblyContainingType<CoreRegistry>();
                scan.AssemblyContainingType<NBusServerRegistry>();
                scan.WithDefaultConventions();
                scan.LookForRegistries();
            }));

            AutoMapperConfiguration.Configure(Container.GetInstance);

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            configuration.UseSerialization<JsonSerializer>();
            configuration.Transactions().Enable();
            configuration.EndpointName("ApiServerService");
            configuration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(Container));
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.Conventions().DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("NSBus.Dto.Commands"));
            configuration.Conventions().DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("NSBus.Dto.Events"));
            configuration.Conventions().DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("NSBus.Dto.Messages"));
            configuration.PurgeOnStartup(true);
        }

        public void Start()
        {
            Console.WriteLine("Service Start");
        }

        public void Stop()
        {
            Console.WriteLine("Stop Service Called");
        }
    }
}
