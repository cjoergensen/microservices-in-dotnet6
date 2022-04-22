using Grpc.Net.Client;
using MeterDataManagement.SmartMeterAggregator;
using Microsoft.Extensions.Hosting;

namespace MeterDataManagement.SmartMeter;

internal class MeterReadingService : BackgroundService
{
    private readonly PowerMeterReading.PowerMeterReadingClient client;

    public MeterReadingService()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:8003");
        this.client = new MeterDataManagement.SmartMeterAggregator.PowerMeterReading.PowerMeterReadingClient(channel);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random consumptionRandom = new();
        double value = 0;
        
        var stream = client.StreamPowerReadings(cancellationToken: stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumption = Math.Round(consumptionRandom.NextDouble(), 2);
            value += consumption;
            value = Math.Round(value, 2);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTimeOffset.Now:G}: {value}");
            
            await stream.RequestStream.WriteAsync(new PowerMeterReadingMessage
            {
                CustomerId = 1,
                MeterId = Guid.NewGuid().ToString(),
                ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                Value = value
            });

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                value += 20;
                value = Math.Round(value, 2);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTimeOffset.Now:G}: {value}");

                await client.AbnormalPowerConsumptionDetectedAsync(new PowerMeterReadingMessage
                {
                    CustomerId = 1,
                    MeterId = Guid.NewGuid().ToString(),
                    ReadingTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now),
                    Value = value
                }, cancellationToken: stoppingToken);
                Console.ResetColor();
            }
            await Task.Delay(1000, stoppingToken);
        }

        await stream.RequestStream.CompleteAsync();
    }
}
