using Microsoft.Extensions.Hosting;

namespace SmartMeter;

internal class MeterReadingService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Random consumptionRandom = new();
        double value = 0;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumption = Math.Round(consumptionRandom.NextDouble(), 2);
            value += consumption;
            value = Math.Round(value, 2);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTimeOffset.Now:G}: {value}");
            

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                value += 20;
                value = Math.Round(value, 2);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DateTimeOffset.Now:G}: {value}");
                Console.ResetColor();
            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}
