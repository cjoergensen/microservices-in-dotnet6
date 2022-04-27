using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MeterReadingService.Contracts.Events.v1_0;
using MeterReadingService.Data;
using MeterReadingService.Models;
using NServiceBus;
using static MeterReadingService.WebApi.PowerMeterReading;

namespace MeterReadingService.WebApi.Services;

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
        await messageSession.Publish(new AbnormalConsumptionDetected(request.CustomerId, request.MeterId, request.ReadingTime.ToDateTimeOffset(), request.Value));
        return new Empty();
    }
}