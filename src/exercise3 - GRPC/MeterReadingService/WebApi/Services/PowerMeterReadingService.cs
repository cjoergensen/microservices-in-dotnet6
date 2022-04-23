using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MeterReadingService.Data;
using MeterReadingService.Models;
using static MeterReadingService.WebApi.PowerMeterReading;

namespace MeterReadingService.WebApi.Services;

public class PowerMeterReadingService : PowerMeterReadingBase
{
    private readonly IMeterReadingRepository repository;

    public PowerMeterReadingService(IMeterReadingRepository repository)
    {
        this.repository = repository;
    }

    public override async Task<Empty> StreamPowerReadings(IAsyncStreamReader<PowerMeterReadingMessage> requestStream, ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            repository.Add(
                new MeterReading(request.CustomerId, request.MeterId, request.ReadingTime.ToDateTimeOffset(), request.Value));
        }

        return new Empty();
    }

    public override Task<Empty> AddPowerReading(PowerMeterReadingMessage request, ServerCallContext context)
    {
        repository.Add(
            new MeterReading(request.CustomerId, request.MeterId, request.ReadingTime.ToDateTimeOffset(), request.Value));

        return Task.FromResult(new Empty());
    }

    public override Task<Empty> AbnormalPowerConsumptionDetected(PowerMeterReadingMessage request, ServerCallContext context)
    {
        // TODO: Raise event
        Console.WriteLine("!!");
        return Task.FromResult(new Empty());
    }
}