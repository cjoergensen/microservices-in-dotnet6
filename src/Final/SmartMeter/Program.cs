using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Telemetry;
using static MeterReadingService.WebApi.PowerMeterReading;


var defaultMethodConfig = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(30),
        MaxBackoff = TimeSpan.FromSeconds(300),
        BackoffMultiplier = 2,
        RetryableStatusCodes =
        {
            // Whatever status codes you want to look for
            StatusCode.Unauthenticated, StatusCode.NotFound, StatusCode.Unavailable,
        },
    }
};

var hostBuilder = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHostedService<SmartMeter.MeterReadingService>();
        services.AddTelemetry("SmartMeter", "1.0.0");
        services.AddGrpcClient<PowerMeterReadingClient>(o => {
            o.Address = new Uri("https://localhost:8003");
            o.ChannelOptionsActions.Add(options =>
            {
                options.ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } };
            });
        });
    });

await hostBuilder.RunConsoleAsync();