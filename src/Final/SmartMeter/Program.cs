using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Telemetry;

var hostBuilder = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHostedService<SmartMeter.MeterReadingService>();
        services.AddTelemetry("SmartMeter", "1.0.0");
    });

await hostBuilder.RunConsoleAsync();