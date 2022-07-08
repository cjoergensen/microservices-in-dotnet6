using AcmePowerSolutions.MeterDataManagement.Meter.Services;
using AcmePowerSolutions.MeterDataManagement.Proto;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Logging;
using Shared.Telemetry;

var hostBuilder = new HostBuilder();
hostBuilder.UseLogging();
hostBuilder.ConfigureServices(services =>
{
    services.AddHostedService<AutomaticMeterReadingService>();
    services.AddTelemetry("SmartMeter", "1.0.0");
    services.AddGrpcClient<PowerMeterReading.PowerMeterReadingClient>(o =>
    {
        o.Address = new Uri("https://localhost:8003");
        o.ChannelOptionsActions.Add(options =>
        {
            options.ServiceConfig = new ServiceConfig
            {
                MethodConfigs =
                {
                    new MethodConfig
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
                    }
                }
            };
        });
    });
});
await hostBuilder.RunConsoleAsync();