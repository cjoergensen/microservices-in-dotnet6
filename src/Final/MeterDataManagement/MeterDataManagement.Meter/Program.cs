using AcmePowerSolutions.MeterDataManagement.Meter.Services;
using AcmePowerSolutions.MeterDataManagement.Proto;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Logging;
using Shared.Telemetry;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;



var hostBuilder = Host.CreateDefaultBuilder(args);
hostBuilder.UseLogging();
hostBuilder.ConfigureServices((context,services) =>
{
    services.AddHostedService<AutomaticMeterReadingService>();
    services.AddTelemetry("SmartMeter", "1.0.0");
 
    var httpClientBuilder = services.AddGrpcClient<PowerMeterReading.PowerMeterReadingClient>(o =>
    {
        o.Address = new Uri("https://host.docker.internal:8010");
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

    if (context.HostingEnvironment.IsDevelopment())
    {
        httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                if (sslPolicyErrors != SslPolicyErrors.RemoteCertificateChainErrors)
                    return sender.RequestUri?.Host == "host.docker.internal";

                return false;
            }
        });
    }
});

await hostBuilder.RunConsoleAsync();