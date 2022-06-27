using Grpc.Core;
using MeterReadingService.WebApi;
using Microsoft.Extensions.Hosting;
using static MeterReadingService.WebApi.PowerMeterReading;

namespace SmartMeter;

internal class MeterReadingService : BackgroundService
{
    private readonly PowerMeterReadingClient client;

    public MeterReadingService(PowerMeterReadingClient client)
    {
        this.client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random consumptionRandom = new();
        double value = 0;
        
        //var stream = client.StreamPowerReadings(cancellationToken: stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Reading meter value");
            Console.WriteLine($"Date:\t\t\t\t{DateTimeOffset.Now:G}");

            var consumption = Math.Round(consumptionRandom.NextDouble(), 2);
            value += consumption;
            value = Math.Round(value, 2);

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                value += 20;
                value = Math.Round(value, 2);

                Console.ForegroundColor = ConsoleColor.Red;
                await client.AbnormalPowerConsumptionDetectedAsync(new PowerMeterReadingMessage
                {
                    CustomerId = 1,
                    MeterId = Guid.NewGuid().ToString(),
                    ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                    Value = value
                }, cancellationToken: stoppingToken);
            }

            Console.WriteLine($"Value:\t\t\t\t{value}");
            Console.ResetColor();
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
                Console.WriteLine($"Data transmission:\t\tOK {await GetRetryCount(call.ResponseHeadersAsync)}");
            }
            catch (RpcException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Data transmission:\t\tFAIL - {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("--------------");
            await Task.Delay(5000, stoppingToken);
        }
    }

    private static async Task<string> GetRetryCount(Task<Metadata> responseHeadersTask)
    {
        var headers = await responseHeadersTask;
        var previousAttemptCount = headers.GetValue("grpc-previous-rpc-attempts");
        return previousAttemptCount != null ? $" - Retry count: {previousAttemptCount}" : string.Empty;
    }
}