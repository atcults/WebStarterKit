using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace WebApp.Initialization
{
    public class UnicastBusConfiguration : IProvideConfiguration<UnicastBusConfig>
    {
        public UnicastBusConfig GetConfiguration()
        {
            var config = new UnicastBusConfig {ForwardReceivedMessagesTo = "audit"};
            var endPoint = new MessageEndpointMapping {AssemblyName = "NSBus.Dto", Endpoint = "ApiServerService"};
            config.MessageEndpointMappings.Add(endPoint);
            return config;
        }
    }
}