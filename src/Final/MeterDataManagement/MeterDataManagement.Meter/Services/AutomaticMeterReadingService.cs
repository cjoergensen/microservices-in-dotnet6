using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AcmePowerSolutions.MeterDataManagement.Proto;

namespace AcmePowerSolutions.MeterDataManagement.Meter.Services;

internal class AutomaticMeterReadingService : BackgroundService
{
    private readonly ILogger<AutomaticMeterReadingService> logger;
    private readonly PowerMeterReading.PowerMeterReadingClient client;

    public AutomaticMeterReadingService(ILogger<AutomaticMeterReadingService> logger, PowerMeterReading.PowerMeterReadingClient client)
    {
        this.logger = logger;
        this.client = client;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("MeterReadingService is starting");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("AutomaticMeterReadingService is executing");

        Random consumptionRandom = new();
        double value = 0;
        
        //var stream = client.StreamPowerReadings(cancellationToken: stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumption = Math.Round(consumptionRandom.NextDouble(), 2);
            value += consumption;
            value = Math.Round(value, 2);

            if (!Console.IsInputRedirected && Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                value += 20;
                value = Math.Round(value, 2);
                await client.AbnormalPowerConsumptionDetectedAsync(new PowerMeterReadingMessage
                {
                    CustomerId = 1,
                    MeterId = Guid.NewGuid().ToString(),
                    ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                    Value = value
                }, cancellationToken: stoppingToken);
            }

            logger.LogInformation("Meter value: {value}", value);
            try
            {
                var call = client.AddPowerReadingAsync(new PowerMeterReadingMessage
                {
                    CustomerId = 1,
                    MeterId = Guid.NewGuid().ToString(),
                    ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                    Value = value
                }, cancellationToken: stoppingToken);                

                await call;
            }
            catch (RpcException ex)
            {
                logger.LogError(ex, "Unable to send power reading");
            }
            await Task.Delay(5000, stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("AutomaticMeterReadingService is stopping");
        return base.StopAsync(cancellationToken); 
    }
}