namespace Shared.Messaging;
public static class HostBuilderExtensions
{
    public static IHostBuilder UseNServiceBus(this IHostBuilder hostBuilder, string endpointName)
    {
        hostBuilder = hostBuilder.UseNServiceBus(context =>
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.UseTransport<LearningTransport>();
            return endpointConfiguration;
        });

        return hostBuilder;
    }
}