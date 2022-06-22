using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Shared.Telemetry;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddTelemetry(this IServiceCollection services, string serviceName, string serviceVersion)
    {
        // Configure important OpenTelemetry settings, the console exporter, and automatic instrumentation
        services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
            .AddSource(serviceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .AddHttpClientInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddNServiceBusInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddJaegerExporter(o =>
            {
                o.AgentHost = "localhost";
                o.AgentPort = 9411; // use port number here
            });
        });

        return services;
    }
}