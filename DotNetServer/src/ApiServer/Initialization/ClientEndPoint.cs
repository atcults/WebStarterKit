using System;
using Common;
using Core;
using GemBox.Spreadsheet;
using NServiceBus;
using StructureMap;
using StructureMap.Graph;
using WebApp.Initialization.Automapper;

namespace WebApp.Initialization
{
    public class ClientEndPoint
    {
        private static bool _initialized;
        public readonly static IContainer Container = new Container();
        private static readonly object Lock = new object();

        public static void Initialize()
        {
            if (_initialized) return;
            lock (Lock)
            {
                if (_initialized) return;
                ConfigureStructureMap();
                _initialized = true;
            }
        }

        private static void ConfigureStructureMap()
        {
            Container.Configure(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<CommonRegistry>();
                scan.AssemblyContainingType<CoreRegistry>();
                scan.AssemblyContainingType<UiRegistry>();
                scan.WithDefaultConventions();
                scan.LookForRegistries();
            }));

            AutoMapperConfiguration.Configure(Container.GetInstance);

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var configuration = new BusConfiguration();
            configuration.UseSerialization<JsonSerializer>();
            configuration.Transactions().Enable();
            configuration.AutoSubscribe().AutoSubscribePlainMessages();
            configuration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(Container));
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.Conventions().DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("NSBus.Dto.Commands"));
            configuration.Conventions().DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("NSBus.Dto.Events"));
            configuration.Conventions().DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("NSBus.Dto.Messages"));
            configuration.PurgeOnStartup(true);
            configuration.EndpointName("ApiServer");
            configuration.EnableInstallers();

            Bus.Create(configuration);//this will run the installers

            Console.Write(Container.WhatDoIHave());
        }
        
    }
}