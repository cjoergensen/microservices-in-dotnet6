using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MeterReadingService.Contracts.v1_0.Events;
using MeterReadingService.Data;
using MeterReadingService.Models;
using NServiceBus;
using static PowerMeterReading;

namespace MeterReadingService.WebApi.Services.v1_0;

public class PowerMeterReadingService : PowerMeterReadingBase
{
    private readonly IMeterReadingRepository repository;
    private readonly IMessageSession messageSession;

    public PowerMeterReadingService(IMeterReadingRepository repository, IMessageSession messageSession)
    {
        this.repository = repository;
        this.messageSession = messageSession;
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

    public async override Task<Empty> AbnormalPowerConsumptionDetected(PowerMeterReadingMessage request, ServerCallContext context)
    {
        await messageSession.Publish(
            new AbnormalConsumptionDetected(request.CustomerId, request.MeterId, request.ReadingTime.ToDateTimeOffset(), request.Value, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), request.ReadingTime.ToDateTimeOffset()));
        return new Empty();
    }
}