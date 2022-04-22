using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
namespace MeterDataManagement.SmartMeterAggregator.Services;

public class PowerMeterReadingAggregatorService : MeterDataManagement.SmartMeterAggregator.PowerMeterReading.PowerMeterReadingBase
{
    public override async Task<Empty> StreamPowerReadings(IAsyncStreamReader<PowerMeterReadingMessage> requestStream, ServerCallContext context)
    {
        await foreach (var message in requestStream.ReadAllAsync())
        {
            Console.WriteLine(message);
        }

        return new Empty();
    }

    public override Task<Empty> AddPowerReading(MeterDataManagement.SmartMeterAggregator.PowerMeterReadingMessage request, ServerCallContext context)
    {
        // TODO: Store data
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> AbnormalPowerConsumptionDetected(PowerMeterReadingMessage request, ServerCallContext context)
    {
        // TODO: Raise event
        Console.WriteLine("!!");
        return Task.FromResult(new Empty());
    }
}