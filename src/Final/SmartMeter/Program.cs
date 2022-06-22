// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// Define some important constants to initialize tracing with
var serviceName = "SmartMeter";
var serviceVersion = "1.0.0";

var hostBuilder = new HostBuilder()
  .ConfigureServices(services =>
  {
      services.AddHostedService<SmartMeter.MeterReadingService>();
      services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
            .AddConsoleExporter()
            .AddSource(serviceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .AddGrpcClientInstrumentation()
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation();
          //.AddSqlClientInstrumentation();
        });
  });

await hostBuilder.RunConsoleAsync();
